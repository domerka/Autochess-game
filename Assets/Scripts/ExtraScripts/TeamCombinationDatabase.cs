using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TeamCombinationDatabase 
{
    private static readonly TeamCombinationDatabase instance = new TeamCombinationDatabase();

    private Dictionary<string,int> characterNames = new Dictionary<string, int>();
    private static Dictionary<string, int> teamCombinations = new Dictionary<string, int>();
    
    static TeamCombinationDatabase() {}
    private TeamCombinationDatabase() {}


    public static TeamCombinationDatabase Instance
    {
        get
        {
            return instance;
        }
    }

    public void AddCharacter(CharacterController character)
    {
        if (!characterNames.ContainsKey(character.characterName))
        {
            characterNames.Add(character.characterName,1);

            foreach(string traitName in character.traitNames)
            {
                if (teamCombinations.ContainsKey(traitName)) teamCombinations[traitName]++;
                else teamCombinations.Add(traitName, 1);
            }
        }
        else
        {
            characterNames[character.characterName]++;
        }
    }

    public void RemoveCharacter(CharacterController character)
    {
        if (characterNames.ContainsKey(character.characterName))
        {
            if (characterNames[character.characterName] == 1)
            {
                foreach(string traitName in character.traitNames)
                {
                    if (teamCombinations[traitName] == 1) teamCombinations.Remove(traitName);
                    else teamCombinations[traitName]--;
                }
            }
            else
            {
                characterNames[character.characterName]--;
            }
        }
    }

    public Dictionary<string,int> GetTeamCombinations()
    {
        return teamCombinations;
    }

}
