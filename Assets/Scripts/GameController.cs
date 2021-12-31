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
    private int maxLevel;
    [SerializeField]private int teamSize;
    private int nextIncome;
    private int xp;
    [SerializeField]private bool fightIsOn;
    private bool showBoard;
    private bool fightPreparation;

    private int opponentStrength;
    
    private int[] xpForNextLevel;

    private int boardNumber;

    private Dictionary<string, int> championPool;
    private Dictionary<int, string[]> spawnEnemyDictionary;

    [SerializeField]private List<CharacterController> savedAlliesBeforeFight;

    private UIController uiController;

    private PlayerController player;

    private int[] fightStreakGold;

    private Dictionary<int, int[]> shopOdds;

    [SerializeField] private List<GameObject> onBoardAllies;
    [SerializeField] private List<GameObject> onBenchAllies;

    private List<int> activeSkillTreeNumbers;

    private List<int> upgradedSkillTreeNumbers;

    private int maxIncome;


    private int numberOfWins;


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
        maxIncome = 50;
        maxLevel = 10;
        numberOfWins = 0;
        opponentStrength = 0;

        fightStreakGold = new int[6] { 0, 1, 1, 2, 3, 4 };
        xpForNextLevel = new int[] { 4, 16, 24, 36, 60, 84, 100, 120, 132, 140,180 };
        shopOdds = new Dictionary<int, int[]>();
        savedAlliesBeforeFight = new List<CharacterController>();
        onBenchAllies = new List<GameObject>();
        onBoardAllies = new List<GameObject>();
        activeSkillTreeNumbers = new List<int>();
        upgradedSkillTreeNumbers = new List<int>();

        CreateShopOdss();
        CreateCharacterPool();
        CreateSpawnEnemydictionary();
        ChampionDatabase.LoadBatabases();
        ExtraDatabases.LoadDatabases();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
    }

    public void LoadSave(SaveGameData saveGameData)
    {

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
        shopOdds.Add(11, new int[] { 1, 5, 20, 24, 50 });
    }

    //-------------------------------------------UI helper game logic functions
    public bool LevelUp()
    {
        if (gold >= 4)
        {
            if(upgradedSkillTreeNumbers.Contains(16))xp += 1;
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
        GameObject[] survivedEnemys = GameObject.FindGameObjectsWithTag("Enemy");
        int damageToPlayer = upgradedSkillTreeNumbers.Contains(26) ? survivedEnemys.Length - 1 + opponentStrength : survivedEnemys.Length + opponentStrength;
        if (survivedEnemys != null) player.TakeDamage(damageToPlayer);

        if (player.health < 0 && upgradedSkillTreeNumbers.Contains(25)) player.health = 10;
        if (upgradedSkillTreeNumbers.Contains(14) && survivedEnemys.Length == 0) gold += onBoardAllies.Count;
        if (survivedEnemys.Length == 0) numberOfWins++;
        DestroyAllUnits();

        ResetBoard();

        AddXp(2);

        CalculateNextIncome();
        //-----------------------------------------Skill tree ability upgrades
        if(upgradedSkillTreeNumbers.Contains(4)) player.AddHealth(1);
        if (upgradedSkillTreeNumbers.Contains(5)) foreach (GameObject unit in onBoardAllies) unit.GetComponent<CharacterController>().AddHealth(50);
        if (upgradedSkillTreeNumbers.Contains(6)) foreach (GameObject unit in onBoardAllies) unit.GetComponent<CharacterController>().AddDamage(10);
        if (upgradedSkillTreeNumbers.Contains(7)) foreach (GameObject unit in onBoardAllies) unit.GetComponent<CharacterController>().AddMagicDamage(10);
        if (upgradedSkillTreeNumbers.Contains(11)) AddXp(1);
        if (upgradedSkillTreeNumbers.Contains(12)) if(onBenchAllies.Count == 0) AddXp(2);
        if (upgradedSkillTreeNumbers.Contains(13)) gold += Mathf.FloorToInt(onBoardAllies.Count / 2);


        uiController.UpdateUI();
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
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Ally"))
        { 
            unit.GetComponent<CharacterController>().standingTile.tag = "Free";
            Destroy(unit);
            RemoveCharacterOnBoard(unit);
        }
        //Destroy enemys
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            unit.GetComponent<CharacterController>().standingTile.tag = "Free";
            Destroy(unit);
        }
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
        foreach (CharacterController unit in savedAlliesBeforeFight) AddCharacterOnBoard(CharacterInstantiator.InstantiateAfterFight(unit));
        //Delete copies
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Copy")) Destroy(unit);

        savedAlliesBeforeFight.Clear();
    }
    public void SpawnEnemies(int formation)
    {
        //TODO dependent on the opponent strength

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

        print(GameObject.FindGameObjectWithTag("Copy").GetComponent<CharacterController>().upgradedStats[0]);
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

    public void AddSkillTreeBonus(int serialNumber)
    {
        switch (serialNumber)
        {
            //LEVEL 1
            case (1)://Gain 10 gold
                gold += 10;
                break;
            case (2)://Gain 10 xp
                AddXp(10);
                break;
            case (3)://Gain 2 more skill points
                player.AddSkillPoints(2);
                break;
            case (4)://Heal yourself for 1 health after every round
                upgradedSkillTreeNumbers.Add(4);
                break;
            case (5)://Your units onBoard gain 50 health permanently
                upgradedSkillTreeNumbers.Add(5);
                break;
            case (6)://Your units onBoard gain 10 AD permanently
                upgradedSkillTreeNumbers.Add(6);
                break;
            case (7)://Your units onBoard gain 10 MD permanently
                upgradedSkillTreeNumbers.Add(7);
                break;
            case (8)://Add baseHealing
                foreach (GameObject unit in onBoardAllies) unit.GetComponent<CharacterController>().SetBaseHealing(80);
                upgradedSkillTreeNumbers.Add(8);
                break;
            case (9)://Add healing based off attackDamge
                foreach (GameObject unit in onBoardAllies) unit.GetComponent<CharacterController>().SetAttackDamageHealing(10);
                upgradedSkillTreeNumbers.Add(9);
                break;
            case (10)://Add healing based off magicDamge
                foreach (GameObject unit in onBoardAllies) unit.GetComponent<CharacterController>().SetMagicDamageHealing(10);
                upgradedSkillTreeNumbers.Add(10);
                break;
            case (11)://Every turn gain one xp
                upgradedSkillTreeNumbers.Add(11);
                break;
            case (12)://Every turn gain two xp if bench is clear
                upgradedSkillTreeNumbers.Add(12);
                break;
            case (13)://Every turn gain one gold for every two surviving units onBoard
                upgradedSkillTreeNumbers.Add(13);
                break;
            case (14)://If you win the round gain 1 gold for every unit
                upgradedSkillTreeNumbers.Add(14);
                break;
            case (15)://Increase max income to 6
                maxIncome += 10;
                uiController.AddBarToInterestBar();
                break;
            case (16)://Gain one more xp for levelling
                upgradedSkillTreeNumbers.Add(16);
                break;
            case (17)://Remove enemy armor

                break;
            case (18)://Remove enemy healing

                break;
            case (19)://Remove enemy magic resist

                break;
            case (20):

                break;
            //LEVEL 2
            case (21):// Get gold level 2
                gold += 20;
                break;
            case (22):// You can reach level 11
                maxLevel += 1;
                break;
            case (23)://Player heals 10 Hp
                player.AddHealth(10);
                break;
            case (24)://Trade health for gold one time

                break;
            case (25)://Sudden death
                upgradedSkillTreeNumbers.Add(25);
                break;
            case (26)://Take one less damage after fight
                upgradedSkillTreeNumbers.Add(26);
                break;
            case (27):

                break;
            case (28):

                break;
            case (29):

                break;
            case (30):

            //LEVEL 3 
                break;
            case (31)://Your team gains +2 teamSize
                teamSize += 2;
                break;
            case (32)://Get gold level 3
                gold += 30;
                break;
            case (33)://Player heals 20 damage
                player.AddHealth(20);
                break;
            case (34)://Gain a random two star five cost unit ---------TODO

                break;
            case (35)://Your units gain stats based on how many wins you have
                foreach (GameObject unit in onBoardAllies)
                {
                    unit.GetComponent<CharacterController>().AddHealth(500);
                    unit.GetComponent<CharacterController>().AddDamage(50);
                    unit.GetComponent<CharacterController>().AddMagicDamage(50);
                    unit.GetComponent<CharacterController>().armor += 30;
                    unit.GetComponent<CharacterController>().magicDamage += 30;
                    unit.GetComponent<CharacterController>().attackSpeed += 1.5f;
                }
                upgradedSkillTreeNumbers.Add(35);
                break;
            case (36)://Lower opponent strength by one
                opponentStrength--;
                break;
        }
        uiController.UpdateUI();
    }

    public void ManageOpponentStrength()
    {
        opponentStrength++;
        uiController.UpdateOSB(opponentStrength);
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
        if (xp >= xpForNextLevel[level - 1])
        {
            player.AddSkillPoints(1);
            teamSize++;
            level++;
            xp = xp - xpForNextLevel[level - 2];
        }
        uiController.UpdateUIOnLevelUp();
    }

    public void SetFightIsOn(bool set)
    {
        fightIsOn = set;
    }

    public int GetNextIncome()
    {
        int interest = gold > maxIncome ? (int)Mathf.Floor((gold - (gold - maxIncome)) / 10) : (int)Mathf.Floor(gold / 10);
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
        if (upgradedSkillTreeNumbers.Contains(8)) character.GetComponent<CharacterController>().SetBaseHealing(80);
        if (upgradedSkillTreeNumbers.Contains(9)) character.GetComponent<CharacterController>().SetAttackDamageHealing(10);
        if (upgradedSkillTreeNumbers.Contains(10)) character.GetComponent<CharacterController>().SetMagicDamageHealing(10);
        if (upgradedSkillTreeNumbers.Contains(35))
        {
            character.GetComponent<CharacterController>().AddHealth(500);
            character.GetComponent<CharacterController>().AddDamage(50);
            character.GetComponent<CharacterController>().AddMagicDamage(50);
            character.GetComponent<CharacterController>().armor += 30;
            character.GetComponent<CharacterController>().magicDamage += 30;
            character.GetComponent<CharacterController>().attackSpeed += 1.5f;
            HealthBarDetails.Add(character);
        }

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
        if (upgradedSkillTreeNumbers.Contains(8)) character.GetComponent<CharacterController>().SetBaseHealing(0);
        if (upgradedSkillTreeNumbers.Contains(9)) character.GetComponent<CharacterController>().SetAttackDamageHealing(0);
        if (upgradedSkillTreeNumbers.Contains(10)) character.GetComponent<CharacterController>().SetMagicDamageHealing(0);
        if (upgradedSkillTreeNumbers.Contains(35))
        {
            character.GetComponent<CharacterController>().health-= 500;
            character.GetComponent<CharacterController>().attackDamage -= 50;
            character.GetComponent<CharacterController>().magicDamage -= 50;
            character.GetComponent<CharacterController>().armor -= 30;
            character.GetComponent<CharacterController>().magicDamage -= 30;
            character.GetComponent<CharacterController>().attackSpeed -= 1.5f;
            HealthBarDetails.Add(character);
        }
        onBoardAllies.Remove(character);
        uiController.UpdateTeamSizeUI();
    }
    public void RemoveCharacterOnBench(GameObject character)
    {
        onBenchAllies.Remove(character);
        uiController.UpdateTeamSizeUI();
    }

    public void SetBoardNumber(int amount)
    {
        boardNumber = amount;
    }

    public int GetBoardNumber()
    {
        return boardNumber;
    }

    public List<int> GetSkillTreeAbilities()
    {
        for(int i  = 0; i < 7; i++)
        {
            int rand = Random.Range(1, 22);
            while (activeSkillTreeNumbers.Contains(rand))
            {
                rand = Random.Range(1, 22);
            }
            activeSkillTreeNumbers.Add(rand);
        }

        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(22, 31);
            while (activeSkillTreeNumbers.Contains(rand))
            {
                rand = Random.Range(22, 31);
            }
            activeSkillTreeNumbers.Add(rand);
        }

        for (int i = 0; i < 2; i++)
        {
            int rand = Random.Range(31, 37);
            while (activeSkillTreeNumbers.Contains(rand))
            {
                rand = Random.Range(31, 37);
            }
            activeSkillTreeNumbers.Add(rand);
        }

        return activeSkillTreeNumbers;
    }
    public List<string> GetSkillTreeDescriptions()
    {
        List<string> temp = new List<string>();
        foreach (int skillTreeNumber in activeSkillTreeNumbers) temp.Add(ExtraDatabases.GetAbilityDescriptions(skillTreeNumber));

        return temp;
    }

    public int GetMaxIncome()
    {
        return maxIncome;
    }

    public int GetMaxLevel()
    {
        return maxLevel;
    }

}

