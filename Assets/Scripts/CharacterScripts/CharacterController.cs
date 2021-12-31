using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterController : MonoBehaviour
{
    public string characterName;
    public string className;
    public string traitName;

    public string type;

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
        healthBar = transform.gameObject.GetComponent<HealthBar>();
        manaBar = transform.gameObject.GetComponent<ManaBar>();
        moveSpeed = 2.0f;
        critDamage = 100;
    }
    /*private void Start()
    {
        fightsPlayed = 0;
    }*/
    //Needed for database creation
    public CharacterController(
        string _championName, float _health, string _type, int _attackRange, float _attackSpeed, string _className,
        string _traitName, int _level, int _cost, string _abilityDescription, int _maxMana, int _startingMana,
        int _armor, int _magicResist, int _magicDamage, int _attackDamage, int _critChance, bool _ranged, string _projectileName)
    {
        maxHealth = _health;
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
    }


    public void SetValuesCharacter(CharacterController character)
    {
        maxHealth = character.health;
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

}
