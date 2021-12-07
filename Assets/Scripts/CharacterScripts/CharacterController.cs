using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterController : MonoBehaviour
{
    public string characterName;
    
    public string type;//ally/enemy
    
    public string className;//boxer/archer
    public int level = 1;//level 1-2-3
    public int cost;//how much the whole unit costs
    public string abilityDescription;
    public string traitName;
    public int startingMana;
    public int maximumMana;

    public float health = 100f;
    public int attackRange;
    public float attackSpeed = 1.0f;
    public int armor;
    public int magicResist;
    public int magicDamage;
    public int attackDamage;
    public int critChance;

    public float damageDealt;
    public int damageHealed;

    public bool ranged;

    public int upgradePoints;

    public string projectileName;

    public List<string> upgradedStats;

    public int fightsPlayed;

    public bool draggable;

    public bool sellable;

    public GameObject standingTile;

    private void Awake()
    {
        draggable = true;
        sellable = true;
        upgradedStats = new List<string>();
        fightsPlayed = 0;
    }

    private void Start()
    {
        upgradedStats = new List<string>();
    }

    //Needed for database creation
    public CharacterController(
        string _championName, float _health, string _type, int _attackRange, float _attackSpeed, string _className,
        string _traitName, int _level, int _cost, string _abilityDescription, int _maxMana, int _startingMana,
        int _armor, int _magicResist, int _magicDamage, int _attackDamage, int _critChance, bool _ranged, string _projectileName)
    {
        characterName = _championName;
        health = _health;
        type = _type;
        attackRange = _attackRange;
        attackSpeed = _attackSpeed;
        className = _className;
        level = _level;
        cost = _cost;
        abilityDescription = _abilityDescription;
        traitName = _traitName;
        draggable = true;
        maximumMana = _maxMana;
        startingMana = _startingMana;
        armor = _armor;
        magicResist = _magicResist;
        magicDamage = _magicDamage;
        attackDamage = _attackDamage;
        critChance = _critChance;
        ranged = _ranged;
        projectileName = _projectileName;
    }


    public void SetValuesCharacter(CharacterController character)
    {
        characterName = character.characterName;
        health = character.health;
        type = character.type;
        attackRange = character.attackRange;
        attackSpeed = character.attackSpeed;
        className = character.className;
        level = character.level;
        cost = character.cost;
        abilityDescription = character.abilityDescription;
        traitName = character.traitName;
        draggable = true;
        maximumMana = character.maximumMana;
        startingMana = character.startingMana;
        armor = character.armor;
        magicResist = character.magicResist;
        magicDamage = character.magicDamage;
        attackDamage = character.attackDamage;
        critChance= character.critChance;
        upgradedStats = character.upgradedStats;
        upgradePoints = character.upgradePoints;
        standingTile = character.standingTile;
        fightsPlayed = character.fightsPlayed;
        ranged = character.ranged;
        projectileName = character.projectileName;
    }

    public string GetStats(string statName)
    {
        switch (statName)
        {
            case "health":
                return health.ToString();
            case "armor":
                return armor.ToString();
            case "attackSpeed":
                return attackSpeed.ToString();
            case "magicDamage":
                return magicDamage.ToString();
            case "attackDamage":
                return attackDamage.ToString();
            case "magicResist":
                return magicResist.ToString();
            case "attackRange":
                return attackRange.ToString();
            case "critChance":
                return critChance.ToString();
        }
        return null;
    }

    public void OnCharacterInformationButtonClicked(string buttonName)
    {
        upgradePoints --;
        switch (buttonName)
        {
            case "healthButton":
                upgradedStats.Add("health");
                health += 500;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("health",health);
                break;
            case "armorButton":
                upgradedStats.Add("armor");
                armor += 40;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("armor", armor);
                break;
            case "attackSpeedButton":
                upgradedStats.Add("attackSpeed");
                attackSpeed += 1.5f;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("attackSpeed", attackSpeed);
                break;
            case "magicDamageButton":
                upgradedStats.Add("magicDamage");
                magicDamage += 65;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("magicDamage", magicDamage);
                break;
            case "attackDamageButton":
                upgradedStats.Add("attackDamage");
                attackDamage += 75;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("attackDamage", attackDamage);
                break;
            case "magicResistButton":
                upgradedStats.Add("magicResist");
                magicResist += 60;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("magicResist", magicResist);
                break;
            case "attackRangeButton":
                upgradedStats.Add("attackRange");
                attackRange += 2;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("attackRange", attackRange);
                break;
            case "critChanceButton":
                upgradedStats.Add("critChance");
                critChance += 30;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("critChance", critChance);
                break;
        }

        if(upgradedStats.Count == 2) gameObject.GetComponent<CharacterInformationController>().DisableButtons();

        gameObject.GetComponent<CharacterInformationController>().SetImage(buttonName.Substring(0, buttonName.Length - 6));
    }

    public void AddUpgradePoint()
    {
        upgradePoints++;
        fightsPlayed = 0;
    }

    public bool isRanged()
    {
        return ranged;
    }

}