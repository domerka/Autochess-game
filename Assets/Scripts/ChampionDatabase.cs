using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChampionDatabase
{
    private static Dictionary<int, Dictionary<string, CharacterController>> championDatabase = new Dictionary<int, Dictionary<string, CharacterController>>();

    public static void LoadBatabases()
    {
        LoadDatabase(1, "CharacterLevelOneDatabase");
        LoadDatabase(2, "CharacterLevelTwoDatabase");
        LoadDatabase(3, "CharacterLevelThreeDatabase");
    }

    private static void LoadDatabase(int level, string databaseName)
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Databases/" + databaseName);

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
        championDatabase.Add(level, temp);
    }

    public static CharacterController GetDatabaseCharacterController(int level, string championName)
    {
        return championDatabase[level][championName];
    }
}
