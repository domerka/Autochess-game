using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameData
{
    public int gold;
    public int level;
    public int xp;
    public List<CharacterController> characters;
    public List<string> tags;
    public int boardNumber;
    public Dictionary<List<Sprite>, List<int>> shopInfos;
    public int stageCounter;
    public int opponentStrength;
    public int health;
    public int fightStreak;
    public int skillPoints;
    public int teamSize;
    public List<GameObject> skillTreeAbilities;



    public void SaveGame(int _gold, int _level, int _xp,
                            List<CharacterController> _characters, List<string> _tags, int _boardNumber,
                            Dictionary<List<Sprite>, List<int>> _shopInfos, int _stageCounter, int _opponentStrength,
                            int _health, int _fightStreak, int _skillPoints, int _teamSize, List<GameObject> _skillTreeAbilities)
    {
        characters = new List<CharacterController>();
        tags = new List<string>();
        shopInfos = new Dictionary<List<Sprite>, List<int>>();
        skillTreeAbilities = new List<GameObject>();
        gold = _gold;
        level = _level;
        xp = _xp;
        characters = _characters;
        tags = _tags;
        boardNumber = _boardNumber;
        shopInfos = _shopInfos;
        stageCounter = _stageCounter;
        opponentStrength = _opponentStrength;
        health = _health;
        fightStreak = _fightStreak;
        skillPoints = _skillPoints;
        teamSize = _teamSize;
        skillTreeAbilities = _skillTreeAbilities;

        Save();
    }

    public void Save()
    {

    }
}
