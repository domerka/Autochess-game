using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtraDatabases
{
    private static Dictionary<int, string> abilityDescriptions = new Dictionary<int, string>();

    public static void LoadDatabases()
    {
        LoadAbilityDescriptions("AbilityDescriptions");
    }

    private static void LoadAbilityDescriptions(string databaseName)
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Databases/" + databaseName);

        for (var i = 0; i < data.Count; i++)
        {
            int serialNumber = int.Parse(data[i]["serialNumber"].ToString(), System.Globalization.NumberStyles.Integer);
            string description = data[i]["description"].ToString();
            string title = data[i]["title"].ToString();
            abilityDescriptions.Add(serialNumber,title + "/" + description);
        }

    }


    public static string GetAbilityDescriptions(int serialNumber) 
    {
        return abilityDescriptions[serialNumber];
    }

}
