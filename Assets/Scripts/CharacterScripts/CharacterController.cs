using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterController : MonoBehaviour
{
    public int id;

    public string characterName;

    public List<string> traitNames = new List<string>();

    public string type;

    public float shield;

    public float maxHealth;
    public float health;
    public int attackRange;
    public float attackSpeed;
    public int armor;
    public int magicResist;
    public int magicDamage;
    public int attackDamage;
    public int critChance;
    public int critDamage;

    public int attackDamageHealing;
    public int magicDamageHealing;

    public float moveSpeed;

    public int level = 1;
    public int cost;
    public string abilityDescription;
    
    public float damageDealt;
    public int damageHealed;
    public float baseHealing;

    public bool sellable;
    public bool ranged;
    public bool stunned;

    public List<string> upgradedStats = new List<string>();
    public int upgradePoints;
    public int fightsPlayed;


    public string projectileName;
    public GameObject standingTile;

    private HealthBar healthBar;
    private ManaBar manaBar;
    public int startingMana;
    public int maximumMana;

    public bool putDown;

    private void Awake()
    {
        sellable = true;
        putDown = false;
        fightsPlayed = 0;
        baseHealing = 0;
        shield = 0;
        healthBar = transform.gameObject.GetComponent<HealthBar>();
        manaBar = transform.gameObject.GetComponent<ManaBar>();
       
        moveSpeed = 2.0f;
        critDamage = 100;
    }
    //Needed for database creation
    public CharacterController(
        string _championName, float _health, string _type, int _attackRange, float _attackSpeed, string _traitName_0,
        string _traitName_1,string _traitName_2, int _level, int _cost, string _abilityDescription, int _maxMana, int _startingMana,
        int _armor, int _magicResist, int _magicDamage, int _attackDamage, int _critChance, bool _ranged, string _projectileName, int _critDamage)
    {
        upgradedStats = new List<string>();
        traitNames = new List<string>();
        maxHealth = _health;
        characterName = _championName;
        health = _health;
        type = _type;
        attackRange = _attackRange;
        attackSpeed = _attackSpeed;
        traitNames.Add(_traitName_0);
        traitNames.Add(_traitName_1);
        traitNames.Add(_traitName_2);
        level = _level;
        cost = _cost;
        abilityDescription = _abilityDescription;
        shield = 0;
        maximumMana = _maxMana;
        startingMana = _startingMana;
        armor = _armor;
        magicResist = _magicResist;
        magicDamage = _magicDamage;
        attackDamage = _attackDamage;
        critChance = _critChance;
        ranged = _ranged;
        projectileName = _projectileName;
        putDown = false;
        critDamage = _critDamage;
    }


    public void SetValuesCharacter(CharacterController character)
    {
        id = character.id;
        upgradedStats = new List<string>();
        traitNames = new List<string>();
        maxHealth = character.health;
        characterName = character.characterName;
        health = character.health;
        type = character.type;
        attackRange = character.attackRange;
        attackSpeed = character.attackSpeed;
        traitNames = character.traitNames;
        level = character.level;
        cost = character.cost;
        abilityDescription = character.abilityDescription;
        maximumMana = character.maximumMana;
        startingMana = character.startingMana;
        armor = character.armor;
        magicResist = character.magicResist;
        magicDamage = character.magicDamage;
        attackDamage = character.attackDamage;
        critChance= character.critChance;
        upgradedStats = character.upgradedStats;
        upgradePoints = character.upgradePoints;
        shield = 0;
        standingTile = character.standingTile;
        fightsPlayed = character.fightsPlayed;
        ranged = character.ranged;
        projectileName = character.projectileName;
        critDamage = character.critDamage;
    }

    public string GetStatsString(string statName)
    {
        switch (statName)
        {
            case "maxHealth":
                return maxHealth.ToString();
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
            case "maxMana":
                return maximumMana.ToString();
            case "mana":
                return manaBar.GetComponent<ManaBar>().currentMana.ToString();
            case "critDamage":
                return critDamage.ToString();
        }
        return null;
    }

    public void OnCharacterInformationButtonClicked(string buttonName)
    {
        if (upgradedStats == null) upgradedStats = new List<string>();
        upgradePoints --;
        switch (buttonName)
        {
            case "healthButton":
                upgradedStats.Add("health");
                maxHealth += 500;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("health",maxHealth);
                traitNames.Add("some");
                HealthBarDetails.Add(gameObject);
                break;
            case "manaButton":
                upgradedStats.Add("mana");
                maximumMana -= 30;
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
            case "critDamageButton":
                upgradedStats.Add("critDamage");
                critDamage += 30;
                gameObject.GetComponent<CharacterInformationController>().UpdateText("critDamage", critDamage);
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

    public void TakeDamage(float amount)
    {
        healthBar.TakeDamage(amount);
    }

    public void HealDamage(float amount)
    {
        healthBar.HealDamage(amount);
    }

    public void SetMana(float amount)
    {
        manaBar.SetMana(amount);
    }

    public void AddMana(float amount)
    {
        manaBar.AddMana(amount);
    }
    public bool CastReady()
    {
        return manaBar.CastReady();
    }

    public void Stun()
    {
        stunned = true;
    }

    public void KnockUp()
    {
        stunned = true;

    }

    public void SlowAttackSpeed(float amount)
    {
        attackSpeed -= amount;
    }

    public void SlowMovementSpeed(float amount)
    {
        moveSpeed -= amount;
    }

    public void SetBaseHealing(float amount)
    {
        baseHealing = amount;
    }

    public void AddDamage(int amount)
    {
        attackDamage += amount;
    }

    public void AddHealth(int amount)
    {
        health += amount;
    }

    public void AddMagicDamage(int amount)
    {
        magicDamage += amount;
    }

    public void HealHealth(int amount)
    {
        if(health < maxHealth)
        {
            if(health + amount > maxHealth)  health = maxHealth;
            else  health += amount;
        }
    }

    public float GetCurrentMana()
    {
        return manaBar.GetComponent<ManaBar>().currentMana;
    }

    public void SetAttackDamageHealing(int amount)
    {
        attackDamageHealing = amount;
    }
    public void SetMagicDamageHealing(int amount)
    {
        magicDamageHealing = amount;
    }

    public void AddShield(float amount)
    {
        shield += amount;
        healthBar.AddShield(amount);
    }

    public HealthBar GetHealthBar()
    {
        return healthBar;
    }

}
