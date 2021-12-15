using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//github test change
public class GameController : MonoBehaviour
{
    //-------------------Basic variables
    private int level;
    private int gold;
    [SerializeField]private int teamSize;
    private int nextIncome;
    private int xp;
    [SerializeField]private bool fightIsOn;
    private bool showBoard;
    private bool fightPreparation;
    
    private int[] xpForNextLevel;

    private Dictionary<string, int> championPool;
    private Dictionary<int, string[]> spawnEnemyDictionary;

    private List<CharacterController> savedAlliesBeforeFight;

    private UIController uiController;

    private PlayerController player;

    private int[] fightStreakGold;

    private Dictionary<int, int[]> shopOdds;

    [SerializeField] private List<GameObject> onBoardAllies;
    [SerializeField] private List<GameObject> onBenchAllies;

    private void Awake()
    {
        spawnEnemyDictionary = new Dictionary<int, string[]>();
        
        level = 1;
        gold = 33;
        teamSize = 1;
        nextIncome = 5;
        showBoard = true;
        fightPreparation = false;
        xp = 2;

        fightStreakGold = new int[6] { 0, 1, 1, 2, 3, 4 };
        xpForNextLevel = new int[] { 4, 16, 24, 36, 60, 84, 100, 120, 132, 140 };
        shopOdds = new Dictionary<int, int[]>();
        savedAlliesBeforeFight = new List<CharacterController>();
        onBenchAllies = new List<GameObject>();
        onBoardAllies = new List<GameObject>();

        CreateShopOdss();
        CreateCharacterPool();
        CreateSpawnEnemydictionary();
        ChampionDatabase.LoadBatabases();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
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
    public void UpdateDamageDealt(CharacterController character)
    {
        uiController.UpdateDamageLayout(character);
    }

    //------------------------------------------Preparation stage functions
    public void SetPreparationStage()
    {
        DestroyAllUnits();

        ResetBoard();

        AddXPAfterFight();

        CalculateNextIncome();
        
        uiController.UpdateUI();
    }
    private void AddXPAfterFight()
    {
        xp += 2;
        if (xp >= xpForNextLevel[level - 1])
        {
            player.AddSkillPoints(1);
            level++;
            xp = xp - xpForNextLevel[level - 2];
        }
    }
    private void CalculateNextIncome()
    {
        gold += nextIncome;
        int interest = gold > 50 ? (int)Mathf.Floor((gold - (gold - 50)) / 10) : (int)Mathf.Floor(gold / 10);

        nextIncome = Mathf.Abs(player.GetStreak()) < 6 ? 5 + interest + fightStreakGold[Mathf.Abs(player.GetStreak())] : 5 + interest + fightStreakGold[Mathf.Abs(player.GetStreak() - (player.GetStreak() - 5))];
    }
    private void DestroyAllUnits()
    {
        //Destroy allies
        foreach (GameObject unit in onBoardAllies)
        { 
            unit.GetComponent<CharacterController>().standingTile.tag = "Free";
            Destroy(unit);
        }
        //Destroy enemys
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            unit.GetComponent<CharacterController>().standingTile.tag = "Free";
            Destroy(unit);
        }
        onBoardAllies.Clear();
    }
    private void ResetBoard()
    {
        //Reset Tile isObstacles
        GameObject grid = GameObject.FindGameObjectWithTag("Grid");
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject tile = grid.transform.Find(i.ToString() + j.ToString()).gameObject;
                tile.GetComponent<Tile>().isObstacle = false;
            }
        }
        //Readd units to OG position
        foreach (CharacterController unit in savedAlliesBeforeFight) onBoardAllies.Add(CharacterInstantiator.InstantiateAfterFight(unit));
        //Delete copies
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Copy")) Destroy(unit);

        savedAlliesBeforeFight.Clear();
    }
    public void SpawnEnemies(int formation)
    {
        for (int i = 0; i < spawnEnemyDictionary[formation].Length; i++)
        {
            GameObject spawnTile = GameObject.Find(spawnEnemyDictionary[formation][i]);
            CharacterInstantiator.InstantiateEnemy(spawnTile,"boxer");
        }
    }

    //------------------------------------------Fight preparation stage functions
    public void SetFightPreparationStage()
    {
        showBoard = false;
        fightPreparation = true;
        foreach (GameObject unit in onBoardAllies)
        {
            unit.GetComponent<CharacterController>().putDown = true;
            unit.GetComponent<DragObject>().enabled = false;
            unit.GetComponent<CharacterController>().sellable = false;
            unit.GetComponent<CharacterController>().standingTile.GetComponent<Tile>().isObstacle = true;
        }
        PutInUnitsAutomatically();
        ShowBoardAndBench.Show(false, showBoard);
    }
    private void PutInUnitsAutomatically()
    {
        if (onBenchAllies.Count == 0) return;
        if (onBoardAllies.Count == teamSize) return;

        int unitsMissing = teamSize - onBoardAllies.Count;
        int iterateNumber = Mathf.Max(onBenchAllies.Count, unitsMissing);
        GameObject grid = GameObject.FindGameObjectWithTag("Grid");

        for (int i = 0; i < iterateNumber; i++)
        {
            for (int j = 7; j > 4; j--)
            {
                bool found = false;
                for (int k = 0; k < 7; k++)
                {
                    GameObject tile = grid.transform.Find(k.ToString() + j.ToString()).gameObject;
                    if (tile.tag == "Free")
                    {
                        onBenchAllies[0].GetComponent<CharacterController>().standingTile.tag = "FreeBench";
                        onBenchAllies[0].GetComponent<CharacterController>().standingTile = tile;
                        onBenchAllies[0].GetComponent<CharacterController>().putDown = true;
                        onBenchAllies[0].GetComponent<DragObject>().enabled = false;
                        onBenchAllies[0].GetComponent<CharacterController>().sellable = false;
                        onBenchAllies[0].transform.position = tile.transform.position;
                        onBenchAllies[0].tag = "Ally";
                        tile.tag = "OccupiedByAlly";
                        tile.GetComponent<Tile>().isObstacle = true;
                        found = true;
                        TeamCombinationDatabase.Instance.AddCharacter(onBenchAllies[0].GetComponent<CharacterController>());
                        AddCharacterOnBoard(onBenchAllies[0]);
                        RemoveCharacterOnBench(onBenchAllies[0]);
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
        //Needed because it gets modified
        GameObject[] temp = onBoardAllies.ToArray();
        foreach (GameObject unit in temp)
        {
            for (int i = 1; i <= 2; i++)
            {
                CombineUnits(unit.GetComponent<CharacterController>().characterName, i, false);
            }
        }
    }

    //------------------------------------------Fight stage functions
    public void SetFightStage()
    {
        uiController.ResetStatLayout();
        SaveAlliesBeforFight(onBoardAllies);
        uiController.CreateStatLayout(onBoardAllies, "Ally", "Damage");
        uiController.CreateStatLayout(GameObject.FindGameObjectsWithTag("Enemy").ToList(), "Enemy", "Damage");
        uiController.CreateStatLayout(onBoardAllies, "Ally", "Heal");
        uiController.CreateStatLayout(GameObject.FindGameObjectsWithTag("Enemy").ToList(), "Enemy", "Heal");

        DisableMoveObjectOnBench();
        fightIsOn = true;
        fightPreparation = false;
    }
    private void DisableMoveObjectOnBench()
    {
        GameObject[] onBenchAllies = GameObject.FindGameObjectsWithTag("OnBench");
        foreach (GameObject unit in onBenchAllies)
        {
            if (unit.GetComponent<MoveObject>().enabled)
                unit.GetComponent<MoveObject>().enabled = false;
        }
    }
    private void SaveAlliesBeforFight(List<GameObject> onBoardAllies)
    {
        foreach (GameObject unit in onBoardAllies)
        {
            unit.GetComponent<MoveObject>().enabled = true;
            unit.GetComponent<DragObject>().enabled = false;
            unit.GetComponent<CharacterController>().sellable = false;

            GameObject copyGameObject = new GameObject("Copy");
            copyGameObject.tag = "Copy";

            CharacterController toAdd = CopyComponent<CharacterController>(unit.GetComponent<CharacterController>(), copyGameObject);

            savedAlliesBeforeFight.Add(toAdd);
        }
    }

    //------------------------------------------Overtime stage functions
    public void SetOvertimeStage()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject ally in onBoardAllies) ally.GetComponent<CharacterController>().attackSpeed += 2.0f;
        foreach (GameObject enemy in enemys) enemy.GetComponent<CharacterController>().attackSpeed += 2.0f;
    }

    //------------------------------------------End of the fight stage functions
    public void SetEndFightStage()
    {
        fightIsOn = false;
        showBoard = true;
        DestroyProjectiles();

        GameObject[] survivedEnemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (survivedEnemys != null) player.TakeDamage(survivedEnemys.Length);

        PrepareOnBoardAllies(onBoardAllies);
        CalculateFightStreak(onBoardAllies, survivedEnemys);
        
    }
    private void DestroyProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject projectile in projectiles) Destroy(projectile);
    }
    private void PrepareOnBoardAllies(List<GameObject> onBoardAllies)
    {
        foreach (GameObject ob in onBoardAllies)
        {
            ob.GetComponent<DragObject>().enabled = true;
            ob.GetComponent<CharacterController>().sellable = true;
            ob.GetComponent<CharacterController>().putDown = false;
            ob.GetComponent<CharacterController>().fightsPlayed++;
            if (ob.GetComponent<CharacterController>().fightsPlayed == 3) ob.GetComponent<CharacterController>().AddUpgradePoint();
            ob.GetComponent<CharacterController>().damageDealt = 0;
        }

    }
    private void CalculateFightStreak(List<GameObject> onBoardAllies, GameObject[] survivedEnemys)
    {
        if (onBoardAllies.Count > survivedEnemys.Length)
        {
            if (player.GetStreak() < 0) player.SetStreak(1);
            else player.AddStreak(1);
        }
        else
        {
            if (player.GetStreak() > 0) player.SetStreak(-1);
            else player.AddStreak(-1);
        }
    }
    //-----------------------------------------Database functions
    public void OnShopImageClicked(Sprite button, int cost)
    {
        gold -= cost;
        uiController.UpdateUIOnGoldSpent();
        GameObject unit = CharacterInstantiator.InstantiateFromShop(button.name);
        if (unit == null) return;//TODO "not enough space on bench
        AddCharacterOnBench(unit);
        CombineUnits(button.name, 1, uiController.GetFightIsOn());
    }
    private void CombineUnits(string name, int level, bool fightIsOn)
    {
        List<GameObject> foundUnits = new List<GameObject>();

        if (!fightIsOn && !fightPreparation)
        {
            for (int i = 0; i < onBoardAllies.Count; i++)
            {
                if (onBoardAllies[i].GetComponent<CharacterController>().characterName == name && onBoardAllies[i].GetComponent<CharacterController>().level == level)
                    foundUnits.Add(onBoardAllies[i]);
            }
        }

        for (int i = 0; i < onBenchAllies.Count; i++)
        {
            if (onBenchAllies[i].GetComponent<CharacterController>().characterName == name && onBenchAllies[i].GetComponent<CharacterController>().level == level)
                foundUnits.Add(onBenchAllies[i]);
        }
        if (foundUnits.Count >= 3)
        {
            for (int i = 1; i < 3; i++)
            {
                foundUnits[i].GetComponent<CharacterController>().standingTile.tag = foundUnits[i].GetComponent<CharacterController>().tag == "OnBench" ? "FreeBench" : "Free";

                if (foundUnits[i].GetComponent<CharacterController>().tag == "OnBench") RemoveCharacterOnBench(foundUnits[i]);
                else RemoveCharacterOnBoard(foundUnits[i]);

                Destroy(foundUnits[i]);
            }
            GameObject tileToSet = foundUnits[0].GetComponent<CharacterController>().standingTile;
            foundUnits[0].GetComponent<CharacterController>().SetValuesCharacter(ChampionDatabase.GetDatabaseCharacterController(++level, name));
            foundUnits[0].GetComponent<CharacterController>().standingTile = tileToSet;
            foundUnits[0].GetComponent<CharacterController>().level += 1;
            foundUnits[0].name = foundUnits[0].GetComponent<CharacterController>().characterName;
            foundUnits[0].gameObject.transform.localScale *= 1.1f;
            HealthBarDetails.Add(foundUnits[0]);

            CombineUnits(name, level, fightIsOn);
        }
    }
    
    

    //TODO
    //-----------------------------------------Team combination functions
    public void AddTeamCombinationBonuses()
    {

    }

    //Important function
    public Dictionary<List<Sprite>, List<int>> GetShopPictures()
    {
        Dictionary<List<Sprite>, List<int>> shopPictureInfos = new Dictionary<List<Sprite>, List<int>>();
        List<Sprite> sprites = new List<Sprite>();
        List<int> costs = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            Sprite spriteToAdd = (int)Random.Range(0, 2) == 0 ? Resources.Load<Sprite>("Images/archer") : Resources.Load<Sprite>("Images/boxer");
            costs.Add(1);
            sprites.Add(spriteToAdd);
        }

        shopPictureInfos.Add(sprites, costs);

        return shopPictureInfos;
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
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (onBoardAllies.Count == 0) return true;
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
        return onBoardAllies.Count;
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

    public bool GetShowBoard()
    {
        return showBoard;
    }

    public void AddCharacterOnBoard(GameObject character)
    {
        onBoardAllies.Add(character);
        uiController.UpdateTeamSizeUI();
    }
    public void AddCharacterOnBench(GameObject character)
    {
        onBenchAllies.Add(character);
        uiController.UpdateTeamSizeUI();
    }
    public void RemoveCharacterOnBoard(GameObject character)
    {
        onBoardAllies.Remove(character);
        uiController.UpdateTeamSizeUI();
    }
    public void RemoveCharacterOnBench(GameObject character)
    {
        onBenchAllies.Remove(character);
        uiController.UpdateTeamSizeUI();
    }



}

