using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ShopInstantiator : MonoBehaviour
{
    private UIController uiController;

    private Dictionary<int,Dictionary<string, CharacterController>> championDatabase = new Dictionary<int, Dictionary<string, CharacterController>>();

    private List<GameObject> shopPictures;

    int counter;

    private void Awake()
    {
        shopPictures = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            shopPictures.Add(transform.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();

        LoadDatabase(1, "CharacterLevelOneDatabase");
        LoadDatabase(2, "CharacterLevelTwoDatabase");
        LoadDatabase(3, "CharacterLevelThreeDatabase");
    }

    public void FillShopPictures(List<Sprite> sprites, int gold)
    {
        for(int i = 0; i < sprites.Count; i++)
        {
            shopPictures[i].GetComponent<Image>().sprite = sprites[i];
            shopPictures[i].GetComponent<CharImageButton>().characterCost = championDatabase[1][sprites[i].name].cost;
            shopPictures[i].GetComponent<Button>().interactable = shopPictures[i].GetComponent<CharImageButton>().characterCost > gold ? false : true;
        }
    }

    public void UpdateUI(int gold)
    {
        foreach (GameObject shopPicture in shopPictures)
        {
            if (shopPicture.GetComponent<Image>().sprite.name == "placeholder") continue;
            shopPicture.GetComponent<Button>().interactable = shopPicture.GetComponent<CharImageButton>().characterCost > gold ? false : true; 
        }
    }


    public void OnImageClicked(Sprite button)
    {
        CharacterController character = championDatabase[1][button.name];

        uiController.BuyCharacter(character.cost);

        if(button != Resources.Load<Sprite>("Images/placeholder"))
        {
            GameObject prefab;
            prefab = Resources.Load("Prefabs/" + button.name) as GameObject;

            GameObject[] bench = ResetBoard();

            if (bench[0] != null)
            {
                bench[0].tag = "OccupiedByAlly";
                GameObject inst = Instantiate(prefab, bench[0].transform.position, Quaternion.identity);
                inst.GetComponent<DragObject>().hitTile = bench[0];
                inst.tag = "OnBench";

                inst.GetComponent<CharacterController>().SetValuesCharacter(character);

                inst.GetComponent<CharacterController>().standingTile = bench[0];

                inst.name = button.name + counter++;

                AddHealthBarDetails(inst);

                Destroy(inst.GetComponent<MoveObject>());

                CombineUnits(button.name,1,uiController.GetFightIsOn());
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

        int numOfTicks = Mathf.FloorToInt((charControl.health-1) / healthChunkSize);// this '-10' is because of the spacing 
        float x = (healthBar.GetComponent<RectTransform>().rect.width -10- (tickSize*numOfTicks))/ (charControl.health/healthChunkSize);
        float offset = tickSize/2.0f;
        for (int i = 1; i <= numOfTicks; i++)
        {
            GameObject inst = Instantiate(Resources.Load("Prefabs/Tick") as GameObject, new Vector3(0, 0, 0), Quaternion.identity, character.transform.FindDeepChild("HealthBar"));
            inst.GetComponent<RectTransform>().localPosition = new Vector3(i * x + offset, 0.0f , 0.0f);
            offset += tickSize;
        }

    }

    
    public void CombineUnits(string name,int level, bool fightIsOn)
    {
        GameObject[] onBenchAllies = GameObject.FindGameObjectsWithTag("OnBench");
        List<GameObject> foundUnits = new List<GameObject>();

        if (!fightIsOn)
        {
            GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
            for (int i = 0; i < allies.Length; i++)
            {
                if (allies[i].GetComponent<CharacterController>().characterName == name && allies[i].GetComponent<CharacterController>().level == level) 
                    foundUnits.Add(allies[i]);
            }
        }

        for (int i = 0; i < onBenchAllies.Length; i++)
        {
            if (onBenchAllies[i].GetComponent<CharacterController>().characterName == name && onBenchAllies[i].GetComponent<CharacterController>().level == level)
                foundUnits.Add(onBenchAllies[i]);
        }
        if (foundUnits.Count >= 3)
        {
            for (int i = 1; i < 3; i++)
            {
                foundUnits[i].GetComponent<CharacterController>().standingTile.tag = foundUnits[i].GetComponent<CharacterController>().tag == "OnBench" ? "FreeBench" : "Free";

                Destroy(foundUnits[i]);
            }
            foundUnits[0].gameObject.transform.localScale *= 1.1f;
            GameObject tileToSet = foundUnits[0].GetComponent<CharacterController>().standingTile;

            foundUnits[0].GetComponent<CharacterController>().SetValuesCharacter(championDatabase[++level][name]);

            foundUnits[0].GetComponent<CharacterController>().standingTile = tileToSet;

            foundUnits[0].GetComponent<CharacterController>().level+=1;

            AddHealthBarDetails(foundUnits[0]);

            CombineUnits(name, level, fightIsOn);
        }
    }
    public void BoardResetInstantiation(CharacterController toInst)
    {
        GameObject prefab = null;

        string path = "Prefabs/" + toInst.className;
        prefab = Resources.Load(path) as GameObject;

        GameObject inst = Instantiate(prefab, toInst.standingTile.transform.position, Quaternion.identity);
        inst.GetComponent<DragObject>().hitTile = toInst.standingTile;
        inst.GetComponent<CharacterController>().standingTile = toInst.standingTile;
        inst.tag = "Ally";
        
        if (toInst.level == 2) toInst.transform.localScale *= Mathf.Pow(1.1f, toInst.level-1);

        inst.GetComponent<CharacterController>().SetValuesCharacter(toInst);

        AddHealthBarDetails(inst);

    }
    private void LoadDatabase(int level,string databaseName)
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Databases/"+databaseName);

        Dictionary<string, CharacterController> temp = new Dictionary<string, CharacterController>();

        for (var i = 0; i < data.Count; i++)
        {
            string name = data[i]["name"].ToString();
            int attackRange = int.Parse(data[i]["attackRange"].ToString(), System.Globalization.NumberStyles.Integer);
            float attackSpeed = float.Parse(data[i]["attackSpeed"].ToString(), System.Globalization.NumberStyles.Float);
            int health = int.Parse(data[i]["health"].ToString(), System.Globalization.NumberStyles.Integer);
            string className = data[i]["className"].ToString();
            string traitName = data[i]["traitName"].ToString();
            int cost = int.Parse(data[i]["cost"].ToString(), System.Globalization.NumberStyles.Integer);
            string abilityDescription = data[i]["abilityDescription"].ToString();
            int maxMana = int.Parse(data[i]["maxMana"].ToString(), System.Globalization.NumberStyles.Integer);
            int startingMana = int.Parse(data[i]["startingMana"].ToString(), System.Globalization.NumberStyles.Integer);
            int armor = int.Parse(data[i]["armor"].ToString(), System.Globalization.NumberStyles.Integer);
            int magicResist = int.Parse(data[i]["magicResist"].ToString(), System.Globalization.NumberStyles.Integer);
            int magicDamage = int.Parse(data[i]["magicDamage"].ToString(), System.Globalization.NumberStyles.Integer);
            int attackDamage = int.Parse(data[i]["attackDamage"].ToString(), System.Globalization.NumberStyles.Integer);
            int critChance = int.Parse(data[i]["critChance"].ToString(), System.Globalization.NumberStyles.Integer);
            string projectileName = data[i]["projectileName"].ToString();
            bool ranged = int.Parse(data[i]["armor"].ToString(), System.Globalization.NumberStyles.Integer) == 0 ? false : true;

            CharacterController character = new CharacterController(name, health, "Ally", attackRange, attackSpeed, className, traitName, 
                                                                    1, cost, abilityDescription, maxMana, startingMana, armor, magicResist, 
                                                                    magicDamage, attackDamage, critChance, ranged, projectileName);
            temp.Add(name, character);
        }
        championDatabase.Add(level,temp);
    }


    //Bench manipulation
    private GameObject[] ResetBoard()
    {
        GameObject[] bench = GameObject.FindGameObjectsWithTag("FreeBench");
        List<GameObject> forSort = new List<GameObject>();

        for (int i = 0; i < bench.Length; i++)
        {
            forSort.Add(bench[i]);
        }

        forSort.Sort(SortByName);

        for (int i = 0; i < bench.Length; i++)
        {
            bench[i] = forSort[i];
        }

        return bench;
    }
    static int SortByName(GameObject o1, GameObject o2)
    {
        return int.Parse(o1.name).CompareTo(int.Parse(o2.name));
    }


}
