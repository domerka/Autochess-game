using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveObject : MonoBehaviour
{
    public GameObject standingTile;
    GameObject tileToMoveTo;

    private List<GameObject> gridList;
    private GameObject[] enemys;
    [SerializeField] private float rotationSpeed;
    private float nextAttack = 0;
    private float attackInterval = 0.7f;

    [SerializeField]public GameObject target;

    private bool move = false;

    private CharacterController character;

    private GameController gameController;

    private Animator anim;

    private GameObject grid;

    private float stunCurrent;
    private float stunDuration;

    private float healDuration;

    private float fightStartOffset;

    private void Awake()
    {
        gridList = new List<GameObject>();
        stunCurrent = 0.0f;
        rotationSpeed = 10.0f;
        healDuration = 1.0f;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        character = gameObject.GetComponent<CharacterController>();
        gameController = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>();
        nextAttack = character.attackSpeed;
        standingTile = character.standingTile;
        FindGrid();
    }

    void Update()
    {


        if(Time.time > stunCurrent && character.stunned)
        {
            stunCurrent += 0.1f;
            if (stunCurrent >= stunDuration)
            {
                character.stunned = false;
                stunCurrent = 0.0f;
            }
        }

        if (character.stunned) return;
        //Starting the fight
        if (gameController.GetFightIsOn() && move == false)
        {
            standingTile = character.standingTile;
            move = true;
            //Finding the closest enemy as the first target
            target = FindClosestEnemy();
            if (target == null)
            {
                move = false;
            }
            else
            {
                tileToMoveTo = PathFinding(target);
            }
            fightStartOffset = Time.time;
        }


        if (move && gameController.GetFightIsOn()) 
        {
            //Choosing a new target
            if (target.GetComponent<CharacterController>() == null) target = FindClosestEnemy();
            if (target == null) target = FindClosestEnemy();
            if (tileToMoveTo == null && !InRange(target))
            {
                tileToMoveTo = PathFinding(target);
            }
            //Choosing a new tile to move to when you arrive at the previous
            if (tileToMoveTo != null && tileToMoveTo.transform.position == gameObject.transform.position)
            {
                target = FindClosestEnemy();
                tileToMoveTo = PathFinding(target);
            }
            //If there is a tileToMoveto move();
            if(tileToMoveTo != null)
            {
                Run();
            }
            //If there isnt one then autoattack
            else
            {
                AutoAttack();
            }
            //Cast ability
            if (character.CastReady())
            {
                CastAbility();
            }

            //Add healing
            if(character.baseHealing != 0)
            {
                if (Time.time - fightStartOffset > healDuration)
                {
                    character.HealDamage(character.baseHealing);
                    healDuration += 1.0f;
                }
            }
            
        }

        //Resetting animations to idle once the fight is over
        if (!gameController.GetFightIsOn())
        {
            move = false;
            anim.SetBool("inRange", false);
            anim.SetTrigger("Idle");
        }

    }

    //-----------------------Character behaviour functions
    private void AutoAttack()
    {
        //animation trigger
        anim.SetBool("inRange", true);
        anim.SetFloat("AttackSpeed", character.attackSpeed);

        attackInterval = anim.GetCurrentAnimatorStateInfo(0).length;

        //Rotate towards the enemy
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(gameObject.transform.forward, target.transform.position - gameObject.transform.position, singleStep, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDir);

        gameController.UpdateDamageDealt(character);

        //Projectile shooting
        if (character.isRanged())
        {
            if (Time.time > nextAttack)
            {
                ShootProjectile();
                nextAttack = Time.time + attackInterval;
            }
        }
    }
    public void InflictDamage()
    {
        float damage = character.attackDamage;
        if (Random.Range(0.0f, 100.0f) <= character.critChance) damage *= 1.5f;
        if (!(target.GetComponent<CharacterController>().armor > damage))
        {
            damage -= target.GetComponent<CharacterController>().armor;
            target.GetComponent<HealthBar>().TakeDamage(damage);
            character.damageDealt += damage;
            CreateDamagePopUp(damage);
            character.AddMana(20);
            if (character.attackDamageHealing != 0)  character.AddHealth(Mathf.FloorToInt((damage/100)*character.attackDamageHealing));
            
        }
        else
        {
            //blocked
        }
    }
    private void Run()
    {
        character.standingTile.tag = "Free";
        character.standingTile.GetComponent<Tile>().isObstacle = false;
        if (character.type == "Ally") tileToMoveTo.tag = "OccupiedByAlly";
        if (character.type == "Enemy") tileToMoveTo.tag = "OccupiedByEnemy";
        anim.SetBool("inRange", false);
        anim.SetTrigger("Run");

        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(gameObject.transform.forward, tileToMoveTo.transform.position - gameObject.transform.position, singleStep, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDir);

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, tileToMoveTo.transform.position, Time.deltaTime * character.moveSpeed);
        character.standingTile = tileToMoveTo;
        standingTile = tileToMoveTo;

        tileToMoveTo.GetComponent<Tile>().isObstacle = true;
    }

    //TODO
    private void CastAbility()
    {
        character.SetMana(0);
        //TODO UPDate magicDamage in mathf.FloorToInt
        if (character.magicDamageHealing != 0) character.AddHealth(Mathf.FloorToInt((character.magicDamage / 100) * character.attackDamageHealing));
        AbilityController.CastAbility(character.name);
    }
    private void ShootProjectile()
    {
        GameObject prefab;
        GameObject inst;
        switch (character.projectileName)
        {
            case ("arrow"):
                prefab = Resources.Load("Prefabs/" + character.projectileName) as GameObject;
                inst = Instantiate(prefab, character.standingTile.transform.position, Quaternion.identity);
                inst.GetComponent<Projectile>().SetValues(target, this, 10.0f);
                inst.tag = "Projectile";
                break;
        }
    }
    //-----------------------Path finding function
    private GameObject PathFinding(GameObject target)
    {
        //If the target is in range returns null (this trigerres the autoattack)
        if (InRange(target)) return null;


        //The tile that the enemy is on is marked as occupied so I need to get the closest neighbour of the target as targetTile
        List<GameObject> neighbours = new List<GameObject>();

        neighbours = GetNeighboursGameObject(target.GetComponent<CharacterController>().standingTile);

        GameObject targetTile = neighbours[0];
        float distance = 200.0f;

        foreach (GameObject go in neighbours)
        {
            if (CalculateDistanceToEnemy(go) < distance && !go.GetComponent<Tile>().isObstacle)
            {
                targetTile = go;
                distance = CalculateDistanceToEnemy(go);
            }
        }

        //Calculates the whole path
        List<Tile> path = AStarPathfinding.FindPath(gameObject.GetComponent<CharacterController>().standingTile.GetComponent<Tile>(), targetTile.GetComponent<Tile>());

        //Returns the first element of the path as the tiletomoveto
        for (int i = 0; i < gridList.Count; i++)
        {
            for (int j = 0; j < path.Count; j++)
            {
                if (gridList[i].GetComponent<Tile>().position == path[1].position)
                {
                    return gridList[i];
                }
            }
        }
        return null;
    }
    //-----------------------Tile/tile neighbour functions
    private void CalculateTileNeighbours()
    {
        List<Tile> reCalculatedTiles = new List<Tile>();
        foreach (GameObject tile in gridList)
        {
            tile.GetComponent<Tile>().adjacentTiles = GetNeighbours(tile);
        }
    }
    private List<Tile> GetNeighbours(GameObject n)
    {
        int nColumn = n.name[0] - '0';
        int nRow = n.name[1] - '0';
        List<Tile> neighbours = new List<Tile>();

        for (int i = 0; i < gridList.Count; i++)
        {
            int column = gridList[i].name[0] - '0';
            int row = gridList[i].name[1] - '0';
            //Left tile
            if (column == nColumn - 1 && row == nRow) neighbours.Add(gridList[i].GetComponent<Tile>());
            //Right tile
            if (column == nColumn + 1 && row == nRow) neighbours.Add(gridList[i].GetComponent<Tile>());

            if (nRow % 2 == 0)
            {
                if (column == nColumn && row == nRow + 1) neighbours.Add(gridList[i].GetComponent<Tile>());

                if (column == nColumn && row == nRow - 1) neighbours.Add(gridList[i].GetComponent<Tile>());

                if (column == nColumn - 1 && row == nRow - 1) neighbours.Add(gridList[i].GetComponent<Tile>());

                if (column == nColumn - 1 && row == nRow + 1) neighbours.Add(gridList[i].GetComponent<Tile>());
            }

            if (nRow % 2 == 1)
            {
                //jobb alul
                if (column == nColumn + 1 && row == nRow + 1) neighbours.Add(gridList[i].GetComponent<Tile>());

                //jobb felul
                if (column == nColumn + 1 && row == nRow - 1) neighbours.Add(gridList[i].GetComponent<Tile>());

                //bal felul
                if (column == nColumn && row == nRow - 1) neighbours.Add(gridList[i].GetComponent<Tile>());

                //bal alul
                if (column == nColumn && row == nRow + 1) neighbours.Add(gridList[i].GetComponent<Tile>());
            }
        }
        return neighbours;
    }
    private List<GameObject> GetNeighboursGameObject(GameObject n)
    {
        int nColumn = n.name[0] - '0';
        int nRow = n.name[1] - '0';
        List<GameObject> neighbours = new List<GameObject>();
        for (int i = 0; i < gridList.Count; i++)
        {
            int column = gridList[i].name[0] - '0';
            int row = gridList[i].name[1] - '0';
            //Left tile
            if (column == nColumn - 1 && row == nRow) neighbours.Add(gridList[i]);
            //Right tile
            if (column == nColumn + 1 && row == nRow) neighbours.Add(gridList[i]);

            if (nRow % 2 == 0)
            {
                if (column == nColumn && row == nRow + 1) neighbours.Add(gridList[i]);

                if (column == nColumn && row == nRow - 1) neighbours.Add(gridList[i]);

                if (column == nColumn - 1 && row == nRow - 1) neighbours.Add(gridList[i]);

                if (column == nColumn - 1 && row == nRow + 1) neighbours.Add(gridList[i]);
            }

            if (nRow % 2 == 1)
            {
                //jobb alul
                if (column == nColumn + 1 && row == nRow + 1) neighbours.Add(gridList[i]);

                //jobb felul
                if (column == nColumn + 1 && row == nRow - 1) neighbours.Add(gridList[i]);

                //bal felul
                if (column == nColumn && row == nRow - 1) neighbours.Add(gridList[i]);

                //bal alul
                if (column == nColumn && row == nRow + 1) neighbours.Add(gridList[i]);
            }
        }
        return neighbours;
    }
    //-----------------------Distance functions
    private bool InRange(GameObject enemy)
    {
        GameObject enemyTile = enemy.GetComponent<CharacterController>().standingTile;

        //Remaking the coordinate system for allied tile
        int allyColumn = character.standingTile.name[0] - '0';
        int allyRow = character.standingTile.name[1] - '0';

        var xAlly = allyColumn - (allyRow - (allyRow & 1)) / 2;
        var zAlly = allyRow;
        var yAlly = -xAlly - zAlly;

        //Remaking the coordinate system for enemy tile
        int colEnemy = enemyTile.name[0] - '0';
        int rowEnemy = enemyTile.name[1] - '0';

        var xEnemy = colEnemy - (rowEnemy - (rowEnemy & 1)) / 2;
        var zEnemy = rowEnemy;
        var yEnemy = -xEnemy - zEnemy;

        //Calculating the distance between them
        int range = (Mathf.Abs(xAlly - xEnemy) + Mathf.Abs(yAlly - yEnemy) + Mathf.Abs(zAlly - zEnemy)) / 2;

        if (range > character.attackRange) return false;

        return true;

    }
    public bool InRangeTarget()
    {
        GameObject enemyTile = target.GetComponent<CharacterController>().standingTile;

        //Remaking the coordinate system for allied tile
        int allyColumn = character.standingTile.name[0] - '0';
        int allyRow = character.standingTile.name[1] - '0';

        var xAlly = allyColumn - (allyRow - (allyRow & 1)) / 2;
        var zAlly = allyRow;
        var yAlly = -xAlly - zAlly;

        //Remaking the coordinate system for enemy tile
        int colEnemy = enemyTile.name[0] - '0';
        int rowEnemy = enemyTile.name[1] - '0';

        var xEnemy = colEnemy - (rowEnemy - (rowEnemy & 1)) / 2;
        var zEnemy = rowEnemy;
        var yEnemy = -xEnemy - zEnemy;

        //Calculating the distance between them
        int range = (Mathf.Abs(xAlly - xEnemy) + Mathf.Abs(yAlly - yEnemy) + Mathf.Abs(zAlly - zEnemy)) / 2;

        if (range > character.attackRange) return false;

        return true;
    }
    private float CalculateDistanceToEnemy(GameObject enemy)
    {
        return Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - character.standingTile.transform.position.x, 2) + Mathf.Pow(enemy.transform.position.z - character.standingTile.transform.position.z, 2));
    }
    //---------------------------------Targeting functions
    public GameObject FindFarthestEnemy()
    {
        //Finding the enemy units for the allied unit
        if (character.type == "Ally") enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //Finding the enemy units for the enemy unit
        if (character.type == "Enemy") enemys = GameObject.FindGameObjectsWithTag("Ally");

        //maximum
        if (enemys.Length > 0)
        {
            float maxDist = CalculateDistanceToEnemy(enemys[0]);
            GameObject farthestEnemy = enemys[0];
            for (int i = 0; i < enemys.Length; i++)
            {
                if (CalculateDistanceToEnemy(enemys[i]) > maxDist)
                {
                    maxDist = CalculateDistanceToEnemy(enemys[i]);
                    farthestEnemy = enemys[i];
                }
            }
            return farthestEnemy;
        }
        return null;
    }
    private GameObject FindClosestEnemy()
    {
        //Finding the enemy units for the allied unit
        if (character.type == "Ally") enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //Finding the enemy units for the enemy unit
        if (character.type == "Enemy") enemys = GameObject.FindGameObjectsWithTag("Ally");

        //Minimum search 
        if (enemys.Length > 0)
        {
            float minDist = CalculateDistanceToEnemy(enemys[0]);
            GameObject closestEnemy = enemys[0];
            for (int i = 0; i < enemys.Length; i++)
            {
                if (CalculateDistanceToEnemy(enemys[i]) < minDist)
                {
                    minDist = CalculateDistanceToEnemy(enemys[i]);
                    closestEnemy = enemys[i];
                }
            }
            return closestEnemy;
        }
        return null;
    }

    //---------------------------------Initialize functions
    private void FindGrid()
    {
        grid = GameObject.FindGameObjectWithTag("Grid");
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject tile = grid.transform.Find(i.ToString() + j.ToString()).gameObject;
                gridList.Add(tile);
            }
        }
        CalculateTileNeighbours();
    }



    //TODO separate non MonoBehaviour class creation for fightPopUp infos
    private void CreateDamagePopUp(float damage)
    {
        FightPopupImage.AttackDamagePopup(target.transform.position,damage);
        //Instantiate(Resources.Load("Prefabs/DamagePopUp") as GameObject, target.transform.position, Quaternion.identity).GetComponent<DamagePopUp>().SetStyle(damage);
    }
    

}
