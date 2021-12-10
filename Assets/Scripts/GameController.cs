using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//github test change
public class GameController : MonoBehaviour
{
    //-------------------Basic variables
    private int level;
    private int gold;
    private int teamSize;
    [SerializeField]private bool fightIsOn;
    private int xp;
    private int numberOfChampionsOnBoard;
    private int[] xpForNextLevel;

    private int nextIncome;


    private string[] characterNames;

    private Dictionary<string, int> championPool;
    private Dictionary<int, string[]> spawnEnemyDictionary;

    public GameObject _prefab;

    private List<CharacterController> savedAlliesBeforeFight;

    private UIController uiController;

    private PlayerController player;

    private int[] fightStreakGold;

    private Dictionary<int, int[]> shopOdds;

    private void Awake()
    {
        spawnEnemyDictionary = new Dictionary<int, string[]>();
        characterNames = new string[2] { "boxer", "archer" };
        
        level = 1;
        gold = 33;
        teamSize = 1;
        numberOfChampionsOnBoard = 0;
        nextIncome = 5;
        xp = 2;

        fightStreakGold = new int[6] { 0, 1, 1, 2, 3, 4 };
        xpForNextLevel = new int[] { 4, 16, 24, 36, 60, 84, 100, 120, 132, 140 };
        shopOdds = new Dictionary<int, int[]>();
        savedAlliesBeforeFight = new List<CharacterController>();

        CreateShopOdss();
        CreateCharacterPool();
        CreateSpawnEnemydictionary();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        numberOfChampionsOnBoard = GameObject.FindGameObjectsWithTag("Ally").Length;
        uiController.UpdateTeamSizeUI();
    }

    //-------------------------------------------Initialising functions
    private void CreateSpawnEnemydictionary()
    {   //From 0-5 weakest, 5-10 stronger and so on... 
        //TODO
        string[] position = new string[] {"00","01","04"};
        spawnEnemyDictionary.Add(0, position);
        position = new string[] { "00", "01", "04" };
        spawnEnemyDictionary.Add(1, position);
    }
    private void CreateCharacterPool()
    {
        championPool = new Dictionary<string, int>();
        championPool.Add("boxer", 30);
        championPool.Add("archer", 30);
    }
    private void CreateShopOdss()
    {
        shopOdds.Add(1, new int[] { 100, 0, 0, 0, 0 });
        shopOdds.Add(2, new int[] { 100, 0, 0, 0, 0 });
        shopOdds.Add(3, new int[] { 75, 15, 10, 0, 0 });
        shopOdds.Add(4, new int[] { 50, 30, 20, 0, 0 });
        shopOdds.Add(5, new int[] { 35, 30, 30, 5, 0 });
        shopOdds.Add(6, new int[] { 20, 35, 35, 10, 0 });
        shopOdds.Add(7, new int[] { 10, 30, 44, 15, 1 });
        shopOdds.Add(8, new int[] { 5, 15, 50, 25, 5 });
        shopOdds.Add(9, new int[] { 5, 15, 30, 35, 15 });
        shopOdds.Add(10, new int[] { 0, 5, 30, 40, 25 });
    }

    //-------------------------------------------UI helper game logic functions
    public bool LevelUp()
    {
        if (gold >= 4)
        {
            xp += 4;
            gold -= 4;
            if (xp >= xpForNextLevel[level - 1])
            {
                player.AddSkillPoints(1);
                teamSize++;
                level++;
                xp = xp - xpForNextLevel[level - 2];
            }
            return true;
        }
        return false;
    }
    public bool RefreshShop()
    {
        if (gold >= 2)
        {
            gold -= 2;
            return true;
        }
        return false;
    }
    public bool BuyCharacter(int cost)
    {
        if (gold >= cost)
        {
            gold -= cost;
            return true;
        }

        return false;
    }
    public void UpdateDamageDealt(CharacterController character)
    {
        uiController.UpdateDamageLayout(character);
    }

    //------------------------------------------Preparation stage functions
    public void SetPreparationStage()
    {
        //destroy all units
        DestroyAllUnits();

        ResetBoard();

        //Add gold and Xp
        gold += nextIncome;
        xp += 2;
        if (xp >= xpForNextLevel[level - 1])
        {
            player.AddSkillPoints(1);
            level++;
            xp = xp - xpForNextLevel[level - 2];
        }

        int interest = gold > 50 ? (int)Mathf.Floor((gold - (gold - 50)) / 10) : (int)Mathf.Floor(gold / 10);

        nextIncome = Mathf.Abs(player.GetStreak()) < 6 ? 5 + interest + fightStreakGold[Mathf.Abs(player.GetStreak())] : 5 + interest + fightStreakGold[Mathf.Abs(player.GetStreak() - (player.GetStreak() - 5))];
        uiController.UpdateUI();
    }
    public void SpawnEnemies(int formation)
    {
        GameObject _spawnPosition = null;

        for (int i = 0; i < spawnEnemyDictionary[formation].Length; i++)
        {
            GameObject temp = GameObject.Find(spawnEnemyDictionary[formation][i]);
            if (temp != null)
            {
                _spawnPosition = temp;
                _spawnPosition.tag = "OccupiedByEnemy";
                GameObject inst = Instantiate(_prefab, _spawnPosition.transform.position, Quaternion.identity);
                inst.GetComponent<DragObject>().hitTile = _spawnPosition;
                inst.tag = "Enemy";
                inst.GetComponent<CharacterController>().type = "Enemy";
                inst.GetComponent<CharacterController>().standingTile = _spawnPosition;
                inst.GetComponent<CharacterController>().attackRange = 1;
                inst.GetComponentInChildren<Renderer>().material.color = Color.red;
                inst.name = "Enemy: " + i;
                Destroy(inst.GetComponent<DragObject>());
                inst.GetComponent<CharacterController>().health = 1100;
                inst.GetComponent<CharacterController>().attackRange = 5;
                AddHealthBarDetails(inst);
            }
            else
            {
                print("Tile to spawn on not found");
            }
        }
    }
    private void AddHealthBarDetails(GameObject character)
    {
        CharacterController charControl = character.GetComponent<CharacterController>();
        //if (charControl.type == "Enemy") return;
        GameObject healthBar = character.transform.FindDeepChild("CharacterBars").gameObject;

        int tickSize = 5;
        float healthChunkSize = 250;

        int numOfTicks = Mathf.FloorToInt((charControl.health - 1) / healthChunkSize);// this '-10' is because of the spacing 
        float x = (healthBar.GetComponent<RectTransform>().rect.width - 10 - (tickSize * numOfTicks)) / (charControl.health / healthChunkSize);
        float offset = tickSize / 2.0f;
        for (int i = 1; i <= numOfTicks; i++)
        {
            GameObject inst = Instantiate(Resources.Load("Prefabs/Tick") as GameObject, new Vector3(0, 0, 0), Quaternion.identity, character.transform.FindDeepChild("HealthBar"));
            inst.GetComponent<RectTransform>().localPosition = new Vector3(i * x + offset, 0.0f, 0.0f);
            offset += tickSize;
        }

    }
    private void DestroyAllUnits()
    {
        GameObject[] alliesToDestroy = GameObject.FindGameObjectsWithTag("Ally");
        GameObject[] enemysToDestroy = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject unit in alliesToDestroy)
        {
            unit.GetComponent<CharacterController>().standingTile.tag = "Free";
            Destroy(unit);
        }
        foreach (GameObject unit in enemysToDestroy)
        {
            unit.GetComponent<CharacterController>().standingTile.tag = "Free";
            Destroy(unit);
        }
    }
    private void ResetBoard()
    {
        foreach (CharacterController unit in savedAlliesBeforeFight)
        {
            uiController.shopInstantiator.BoardResetInstantiation(unit);
        }
        //delete copies
        GameObject[] copies = GameObject.FindGameObjectsWithTag("Copy");
        foreach (GameObject unit in copies) Destroy(unit);

        savedAlliesBeforeFight.Clear();
    }

    //------------------------------------------Fight preparation stage functions
    public void SetFightPreparationStage()
    {
        GameObject[] onBoardAllies = GameObject.FindGameObjectsWithTag("Ally");
        foreach (GameObject unit in onBoardAllies)
        {
            unit.GetComponent<CharacterController>().draggable = false;
            unit.GetComponent<CharacterController>().sellable = false;
        }
        PutInUnitsAutomatically();


    }
    private void PutInUnitsAutomatically()
    {
        GameObject[] onBoardAllies = GameObject.FindGameObjectsWithTag("Ally");

        if (onBoardAllies.Length == teamSize) return;

        int unitsMissing = teamSize - onBoardAllies.Length;

        GameObject[] onBenchAllies = GameObject.FindGameObjectsWithTag("OnBench");
        if (onBenchAllies.Length == 0) return;

        GameObject grid = GameObject.FindGameObjectWithTag("Grid");

        for (int i = 0; i < Mathf.Max(onBenchAllies.Length, unitsMissing); i++)
        {
            for (int j = 7; j > 4; j--)
            {
                bool found = false;
                for (int k = 0; k < 7; k++)
                {
                    GameObject tile = grid.transform.Find(k.ToString() + j.ToString()).gameObject;
                    if (tile.tag == "Free")
                    {
                        onBenchAllies[i].GetComponent<CharacterController>().standingTile.tag = "FreeBench";
                        onBenchAllies[i].GetComponent<CharacterController>().standingTile = tile;
                        onBenchAllies[i].GetComponent<CharacterController>().draggable = false;
                        onBenchAllies[i].GetComponent<CharacterController>().sellable = false;
                        onBenchAllies[i].transform.position = tile.transform.position;
                        onBenchAllies[i].tag = "Ally";
                        tile.tag = "OccupiedByAlly";
                        found = true;
                        TeamCombinationDatabase.Instance.AddCharacter(onBenchAllies[i].GetComponent<CharacterController>());
                        break;
                    }
                }
                if (found) break;
            }
            unitsMissing--;
            if (unitsMissing == 0) break;
        }
    }
    public void CheckCharacterUpgrades()
    {
        foreach (string name in characterNames)
        {
            for (int i = 1; i <= 2; i++)
            {
                uiController.shopInstantiator.CombineUnits(name, i, false);
            }

        }
    }

    //------------------------------------------Fight stage functions
    public void SetFightStage()
    {
        GameObject[] onBoardAllies = GameObject.FindGameObjectsWithTag("Ally");

        SaveAlliesBeforFight(onBoardAllies);

        foreach (GameObject ob in onBoardAllies)
        {
            ob.GetComponent<CharacterController>().draggable = ob.GetComponent<CharacterController>().draggable == true ? false : true;
            ob.GetComponent<CharacterController>().sellable = false;
        }
        uiController.CreateStatLayout(onBoardAllies, "Ally", "Damage");
        uiController.CreateStatLayout(GameObject.FindGameObjectsWithTag("Enemy"), "Enemy", "Damage");
        uiController.CreateStatLayout(onBoardAllies, "Ally", "Heal");
        uiController.CreateStatLayout(GameObject.FindGameObjectsWithTag("Enemy"), "Enemy", "Heal");

        DestroyMoveObjectOnBench();
        fightIsOn = true;
    }
    private void DestroyMoveObjectOnBench()
    {
        GameObject[] onBenchAllies = GameObject.FindGameObjectsWithTag("OnBench");
        foreach (GameObject unit in onBenchAllies)
        {
            if (unit.GetComponent<MoveObject>() != null)
                Destroy(unit.GetComponent<MoveObject>());
        }
    }
    private void SaveAlliesBeforFight(GameObject[] onBoardAllies)
    {
        foreach (GameObject unit in onBoardAllies)
        {
            if (unit.GetComponent<MoveObject>() == null) unit.AddComponent<MoveObject>();

            GameObject copyGameObject = new GameObject("Copy");
            copyGameObject.tag = "Copy";

            CharacterController toAdd = CopyComponent<CharacterController>(unit.GetComponent<CharacterController>(), copyGameObject);

            savedAlliesBeforeFight.Add(toAdd);
        }
    }

    //------------------------------------------Overtime stage functions
    public void SetOvertimeStage()
    {
        GameObject[] onBoardAllies = GameObject.FindGameObjectsWithTag("Ally");
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject ally in onBoardAllies) ally.GetComponent<CharacterController>().attackSpeed += 2.0f;
        foreach (GameObject enemy in enemys) enemy.GetComponent<CharacterController>().attackSpeed += 2.0f;
    }

    //------------------------------------------End of the fight stage functions
    public void SetEndFightStage()
    {

        fightIsOn = false;

        DestroyProjectiles();


        GameObject[] onBoardAllies = GameObject.FindGameObjectsWithTag("Ally");
        foreach (GameObject ob in onBoardAllies)
        {
            ob.GetComponent<CharacterController>().draggable = ob.GetComponent<CharacterController>().draggable == true ? false : true;
            ob.GetComponent<CharacterController>().sellable = true;
            ob.GetComponent<CharacterController>().fightsPlayed++;
            if (ob.GetComponent<CharacterController>().fightsPlayed == 3) ob.GetComponent<CharacterController>().AddUpgradePoint();
            ob.GetComponent<CharacterController>().damageDealt = 0;
        }

        GameObject[] survivedEnemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (survivedEnemys != null) player.TakeDamage(survivedEnemys.Length);

        if (onBoardAllies.Length > survivedEnemys.Length)
        {
            if (player.GetStreak() < 0) player.SetStreak(1);
            else player.AddStreak(1);
        }
        else
        {
            if (player.GetStreak() > 0) player.SetStreak(-1);
            else player.AddStreak(-1);
        }


        //Readding the MoveObject script for units on the bench
        GameObject[] onBenchAllies = GameObject.FindGameObjectsWithTag("OnBench");

        foreach (GameObject unit in onBenchAllies) unit.AddComponent<MoveObject>();
    }
    private void DestroyProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject projectile in projectiles) Destroy(projectile);
    }
    //TODO
    //-----------------------------------------Team combination functions
    public void AddTeamCombinationBonuses()
    {

    }

    //Important function
    public List<Sprite> GetShopPictures()
    {
        List<Sprite> sprites = new List<Sprite>();
        for (int i = 0; i < 5; i++)
        {
            Sprite spriteToAdd = (int)Random.Range(0, 2) == 0 ? Resources.Load<Sprite>("Images/archer") : Resources.Load<Sprite>("Images/boxer");

            sprites.Add(spriteToAdd);
        }

        return sprites;
    }


    //Makes a copy of the units on board before the fight starts
    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }


    //Getter - setter functions------------------------------
    public bool CheckWhetherFightIsOver()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (allies.Length == 0) return true;
        if (enemys.Length == 0) return true;
        return false;
    }

    public int GetXpForNextLevel()
    {
        return xpForNextLevel[level-1];
    }

    public int GetXp()
    {
        return xp;
    }

    public int GetGold()
    {
        return gold;
    }

    public int GetLevel()
    {
        return level;
    }

    public bool GetFightIsOn()
    {
        return fightIsOn;
    }

    public int GetNumberOfChampionsOnBoard()
    {
        return numberOfChampionsOnBoard;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        uiController.UpdateUIOnGoldSpent();
    }

    public void AddXp(int amount)
    {
        xp += amount;
        uiController.UpdateUIOnLevelUp();
    }

    public void SetFightIsOn(bool set)
    {
        fightIsOn = set;
    }

    public int GetNextIncome()
    {
        int interest = gold > 50 ? (int)Mathf.Floor((gold - (gold - 50)) / 10) : (int)Mathf.Floor(gold / 10);
        nextIncome = Mathf.Abs(player.GetStreak()) < 6 ? 5 + interest + fightStreakGold[Mathf.Abs(player.GetStreak())] : 5 + interest + fightStreakGold[Mathf.Abs(player.GetStreak() - (player.GetStreak() - 5))];
        return nextIncome;
    }

    public int GetTeamSize()
    {
        return teamSize;
    }

    public void AddTeamSize(int amount)
    {
        teamSize += amount;
    }

    public int [] GetShopOdds()
    {
        return shopOdds[level];
    }
}

