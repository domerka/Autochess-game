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

            if (teamCombinations.ContainsKey(character.traitName)) teamCombinations[character.traitName]++;
            else teamCombinations.Add(character.traitName, 1);

            if (teamCombinations.ContainsKey(character.className)) teamCombinations[character.className]++;
            else teamCombinations.Add(character.className, 1);
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
                if (teamCombinations[character.traitName] == 1) teamCombinations.Remove(character.traitName);
                else teamCombinations[character.traitName]--;
                if (teamCombinations[character.className] == 1) teamCombinations.Remove(character.className);
                else teamCombinations[character.className]--;
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
