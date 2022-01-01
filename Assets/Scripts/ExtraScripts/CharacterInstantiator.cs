using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstantiator: MonoBehaviour
{
    public static GameObject InstantiateFromShop(string characterName)
    {
        CharacterController character = ChampionDatabase.GetDatabaseCharacterController(1, characterName);

        GameObject prefab;
        prefab = Resources.Load("Prefabs/" + characterName) as GameObject;

        GameObject[] bench = ResetBench();

        if (bench[0] != null)
        {
            bench[0].tag = "OccupiedByAlly";
            GameObject inst = Instantiate(prefab, bench[0].transform.position, Quaternion.identity);
            inst.GetComponent<DragObject>().hitTile = bench[0];
            inst.GetComponent<CharacterController>().SetValuesCharacter(character);
            inst.GetComponent<CharacterController>().upgradedStats = new List<string>();
            inst.GetComponent<CharacterController>().standingTile = bench[0];
            inst.name = inst.GetComponent<CharacterController>().characterName;
            inst.tag = "OnBench";
            inst.GetComponent<MoveObject>().enabled = false;
            HealthBarDetails.Add(inst);
            return inst;
        }
        else
        {
            return null;
        }
    }
    public static GameObject InstantiateAfterFight(CharacterController toInst)
    {
        GameObject inst = Instantiate(Resources.Load("Prefabs/" + toInst.characterName) as GameObject, toInst.standingTile.transform.position, Quaternion.identity);
        if (toInst.level > 1) toInst.transform.localScale *= Mathf.Pow(1.1f, toInst.level - 1);
        inst.GetComponent<CharacterController>().SetValuesCharacter(toInst);
        inst.GetComponent<DragObject>().hitTile = toInst.standingTile;
        inst.tag = "Ally";
        inst.name = inst.GetComponent<CharacterController>().characterName;
        HealthBarDetails.Add(inst);
        return inst;
    }
    public static GameObject InstantiateLoadGame(CharacterController toInst,string tag)
    {
        GameObject inst = Instantiate(Resources.Load("Prefabs/" + toInst.characterName) as GameObject, toInst.standingTile.transform.position, Quaternion.identity);
        if (toInst.level > 1) toInst.transform.localScale *= Mathf.Pow(1.1f, toInst.level - 1);
        inst.GetComponent<CharacterController>().SetValuesCharacter(ChampionDatabase.GetDatabaseCharacterController(toInst.level, toInst.GetComponent<CharacterController>().characterName));
        inst.GetComponent<DragObject>().hitTile = toInst.standingTile;
        inst.GetComponent<CharacterController>().standingTile = toInst.standingTile;
        inst.GetComponent<CharacterController>().upgradedStats = toInst.upgradedStats;
        inst.GetComponent<CharacterController>().upgradePoints = toInst.upgradePoints;
        inst.GetComponent<CharacterController>().fightsPlayed = toInst.fightsPlayed;
        inst.tag = tag;
        inst.name = inst.GetComponent<CharacterController>().characterName;
        HealthBarDetails.Add(inst);
        return inst;
    }
    public static void InstantiateEnemy(GameObject spawnTile, string enemyName)
    {
        GameObject _spawnPosition = spawnTile;
        _spawnPosition.tag = "OccupiedByEnemy";
        GameObject inst = Instantiate(Resources.Load("Prefabs/"+enemyName) as GameObject, _spawnPosition.transform.position, Quaternion.identity);
        inst.GetComponent<CharacterController>().standingTile = _spawnPosition;
        inst.tag = "Enemy";
        inst.GetComponent<CharacterController>().type = "Enemy";
        inst.name = "Enemy";
        _spawnPosition.GetComponent<Tile>().isObstacle = true;
        inst.GetComponent<CharacterController>().attackRange = 1;
        inst.GetComponent<CharacterController>().attackDamage = 150;
        inst.GetComponentInChildren<Renderer>().material.color = Color.red;
        Destroy(inst.GetComponent<DragObject>());
        inst.GetComponent<CharacterController>().health = 1100;
        HealthBarDetails.Add(inst);
    }


    //----------------------------------------Bench maipulation functions
    private static GameObject[] ResetBench()
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
    private static int SortByName(GameObject o1, GameObject o2)
    {
        return int.Parse(o1.name).CompareTo(int.Parse(o2.name));
    }

}
