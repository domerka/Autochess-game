using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveObject : MonoBehaviour
{
    public GameObject standingTile;
    GameObject tileToMoveTo;

    private List<GameObject> gridList;
    private GameObject[] enemys;

    [SerializeField] private float runningSpeed = 1.0f;
    [SerializeField] private float rotationSpeed;
    private float nextAttack = 0;
    private float attackInterval = 0.7f;

    [SerializeField]public GameObject target;

    private bool move = false;

    private CharacterController character;

    private GameController gameController;

    private Animator anim;

    private ManaBar manaBar;

    GameObject grid;

    // Start is called before the first frame update
    void Start()
    {
        gridList = new List<GameObject>();
        anim = GetComponent<Animator>();
        character = gameObject.GetComponent<CharacterController>();
        manaBar = gameObject.GetComponent<ManaBar>();
        gameController = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>();
        runningSpeed = 2.0f;
        nextAttack = character.attackSpeed;
        standingTile = character.standingTile;
        rotationSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Find the grid
        if (grid == null)
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
            if (manaBar.castReady())
            {
                CastAbility();
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
    //TODO
    private void CastAbility()
    {
        manaBar.setMana(0);
        AbilityController.CastAbility(character.name);
    }

    //Makes the character autoattack
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
        

        if (character.isRanged())
        {
            if(Time.time > nextAttack)
            {
                ShootProjectile();
                nextAttack = Time.time + attackInterval;
            }
        }
    }

    //Shoot projectile
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

    public void InflictDamage()
    {
        float damage = character.attackDamage;
        if (Random.Range(0.0f, 100.0f) <= character.critChance) damage *= 1.5f;
        if (!(target.GetComponent<CharacterController>().armor > damage))
        {
            damage -= target.GetComponent<CharacterController>().armor;
            target.GetComponent<HealthBar>().currentHealth -= damage ;
            character.damageDealt += damage;
            CreateDamagePopUp(damage);
            manaBar.addMana(20);
        }
        else
        {
            //blocked
        }
    }

    private void CreateDamagePopUp(float damage)
    {
        Instantiate(Resources.Load("Prefabs/DamagePopUp") as GameObject, target.transform.position, Quaternion.identity).GetComponent<DamagePopUp>().SetStyle(damage);
    }

    //Makes the character run
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

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, tileToMoveTo.transform.position, Time.deltaTime * runningSpeed);
        character.standingTile = tileToMoveTo;
        standingTile = tileToMoveTo;

        tileToMoveTo.GetComponent<Tile>().isObstacle = true;
    }

    //Tells whether the target is in attack range or not
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
    //Function to find the distance between two objects(used when looking for the closest enemy)
    private float CalculateDistanceToEnemy(GameObject enemy)
    {
        return Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - character.standingTile.transform.position.x, 2) + Mathf.Pow(enemy.transform.position.z - character.standingTile.transform.position.z, 2));
    }

    //Looks for the tile, which is a neighbour of the character and is the closest to the target
    private GameObject PathFinding(GameObject target)
    {
        //If the target is in range returns null (this trigerres the autoattack)
        if (InRange(target)) return null;


        //The tile that the enemy is on is marked as occupied so I need to get the closest neighbour of the target as targetTile
        List<GameObject> neighbours = new List<GameObject>();

        neighbours = GetNeighboursGameObject(target.GetComponent<CharacterController>().standingTile);

        GameObject targetTile = neighbours[0];
        float distance = 200.0f;

        foreach(GameObject go in neighbours)
        {
            if(CalculateDistanceToEnemy(go) < distance && !go.GetComponent<Tile>().isObstacle)
            {
                targetTile = go;
                distance = CalculateDistanceToEnemy(go);
            }
        }
        
        //Calculates the whole path
        List<Tile> path = AStarPathfinding.FindPath(gameObject.GetComponent<CharacterController>().standingTile.GetComponent<Tile>(), targetTile.GetComponent<Tile>());

        /*
        //For fun-------------------------------------------------------------------------------
        List<GameObject> pathToHighlight = new List<GameObject>();
        if(player.name == "Enemy: 0")
        {
            for (int i = 0; i < gridList.Count; i++)
            {
                for (int j = 0; j < path.Count; j++)
                {
                    if (gridList[i].GetComponent<Tile>().position == path[j].position)
                    {
                        gridList[i].GetComponent<Renderer>().enabled = true;
                        gridList[i].GetComponent<Renderer>().material.color = Color.green;
                        pathToHighlight.Add(gridList[i]);
                    }
                }
            }

            for (int i = 0; i < gridList.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < path.Count; j++)
                {
                    if (gridList[i].GetComponent<Tile>().position == path[j].position)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    gridList[i].GetComponent<Renderer>().enabled = false;
                }
            }
        }
        //For fun-------------------------------------------------------------------------------
        */
        
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

    private void CalculateTileNeighbours()
    {
        List<Tile> reCalculatedTiles = new List<Tile>();
        foreach(GameObject tile in gridList)
        {
            tile.GetComponent<Tile>().adjacentTiles = GetNeighbours(tile);
        }
    }

    //Finds the distance between two vectors
    private float h(Vector3 n, Vector3 end)
    {
        return Mathf.Sqrt(Mathf.Pow(n.x - end.x,2) + Mathf.Pow(n.z - end.z, 2));
    }

    //Finds the neighbouring tiles for 'n' tile
    private List<Tile> GetNeighbours(GameObject n)
    {
        int nColumn = n.name[0] - '0';
        int nRow = n.name[1] - '0';
        List<Tile> neighbours = new List<Tile>();

        for(int i = 0; i < gridList.Count; i++)
        {
            int column = gridList[i].name[0] - '0';
            int row = gridList[i].name[1] - '0';
            //Left tile
            if ( column == nColumn -1 && row == nRow) neighbours.Add(gridList[i].GetComponent<Tile>());
            //Right tile
            if(column == nColumn + 1 && row == nRow) neighbours.Add(gridList[i].GetComponent<Tile>());

            if(nRow %2 == 0)
            {
                if (column == nColumn && row == nRow + 1) neighbours.Add(gridList[i].GetComponent<Tile>());
                
                if (column == nColumn && row == nRow - 1) neighbours.Add(gridList[i].GetComponent<Tile>());
                
                if (column == nColumn - 1 && row == nRow - 1) neighbours.Add(gridList[i].GetComponent<Tile>());
                
                if (column == nColumn - 1 && row == nRow + 1) neighbours.Add(gridList[i].GetComponent<Tile>());
            }

            if (nRow % 2 == 1)
            {
                //jobb alul
                if (column == nColumn +1 && row == nRow + 1) neighbours.Add(gridList[i].GetComponent<Tile>());
                
                //jobb felul
                if (column == nColumn +1 && row == nRow - 1) neighbours.Add(gridList[i].GetComponent<Tile>());
                
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

    //Target finder (finds the closest enemy)
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

}
