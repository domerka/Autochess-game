using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] private GameObject startTile;

    public GameObject hitTile;

    RaycastHit[] hits;
    private CharacterController character;
    private Vector3 screenPoint;
    private Vector3 offset;
    private GameController gameController;
    public Camera camera1;

    public Vector3 tempPos;

    private UIController uiController;

    private void Start()
    {
        character = gameObject.GetComponent<CharacterController>();
        gameController = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>();
        camera1 = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }
    /*
    public void ShowBoardAnBench(bool show)
    {
        GameObject grid = GameObject.FindGameObjectWithTag("Grid");
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject tile = grid.transform.Find(i.ToString() + j.ToString()).gameObject;
                //Bottom half of the board
                if (tile.name[1] - '0' > 3 && !gameController.GetFightIsOn()) tile.GetComponent<Renderer>().enabled = show;
                //Bench
                if (i + j < 9) grid.transform.Find((i + j).ToString()).gameObject.GetComponent<Renderer>().enabled = show;
            }
        }
    }*/

    private void OnMouseDown()
    {
        if (!character.draggable) return;

        //Showing the board and bench
        
        //Saving the startile for swapping
        startTile = character.standingTile;
        ShowBoardAndBench.Show(true, gameController.GetFightIsOn());
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        uiController.ShowGarbage(character.cost);

    }

    private void OnMouseUp()
    {
        if (!character.draggable) return;

        if (uiController.isOverGarbage)
        {
            uiController.HideGarbage();
            uiController.isOverGarbage = false;
            ShowBoardAndBench.Show(false, gameController.GetFightIsOn());
            gameController.AddGold(character.cost);
            character.standingTile.tag = character.tag == "Ally" ? "Free" : "FreeBench";
            Destroy(transform.gameObject);
            return;
        }

        


        uiController.HideGarbage();
        ShowBoardAndBench.Show(false, gameController.GetFightIsOn());

        hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_BaseColor", new Vector4(0f, 0.611f, 0.725f, 255));
        hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_Transparency", new Vector4(0.3f, 0, 0, 0));

        startTile.tag = startTile.name.Length < 2 ? "FreeBench" : "Free";

        
        //Evaluating the hittile

        //Swapping
        if(hitTile.tag == "OccupiedByAlly")
        {
            //find the unit who occupies the hittile
            GameObject whoOccupies = null;
            GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
            GameObject[] onBoard = GameObject.FindGameObjectsWithTag("OnBench");
            foreach (GameObject unit in allies) if (unit.GetComponent<CharacterController>().standingTile == hitTile) whoOccupies = unit;
            foreach (GameObject unit in onBoard) if (unit.GetComponent<CharacterController>().standingTile == hitTile) whoOccupies = unit;

            gameObject.transform.position = whoOccupies.transform.position;
            character.standingTile = whoOccupies.GetComponent<CharacterController>().standingTile;

            whoOccupies.transform.position = startTile.transform.position;
            whoOccupies.GetComponent<CharacterController>().standingTile = startTile;
            whoOccupies.GetComponent<CharacterController>().standingTile.tag = "OccupiedByAlly";

            character.standingTile.tag = "OccupiedByAlly";

            if((whoOccupies.tag == "OnBench") && whoOccupies.tag != character.tag)
            {
                whoOccupies.tag = "Ally";
                gameObject.tag = "OnBench";
            }
            else if((whoOccupies.tag == "Ally") && whoOccupies.tag != character.tag)
            {
                whoOccupies.tag = "OnBench";
                gameObject.tag = "Ally";
            }
        }
        //Hit the board
        else if(hitTile.tag == "Free")
        {
            if ((gameController.GetLevel() > gameController.GetNumberOfChampionsOnBoard()) || (gameController.GetLevel() == gameController.GetNumberOfChampionsOnBoard() && character.tag == "Ally"))
            {
                character.tag = hitTile.name.Length < 2 ? "OnBench" : "Ally";
                hitTile.tag = "OccupiedByAlly";
                transform.position = hitTile.transform.position;
                character.standingTile = hitTile;
            }
            else
            {
                transform.position = startTile.transform.position;
                character.standingTile = startTile;
                startTile.tag = "OccupiedByAlly";
                character.tag = "OnBench";
            }
        }
        //Hit the bench
        else
        {
            transform.position = hitTile.transform.position;
            character.standingTile = hitTile;
            hitTile.tag = "OccupiedByAlly";
            character.tag = "OnBench";
        }

        //Add particle
        Destroy(Instantiate(Resources.Load("Particles/CharacterPlacingParticle"), character.transform.position,Quaternion.identity),0.5f);


        if (!gameController.GetFightIsOn()) uiController.ShowTraits();
        
        
    }

    private void OnMouseDrag()
    {
        if (!character.draggable) return;

        hits = Physics.RaycastAll(camera1.ScreenPointToRay(Input.mousePosition), 100.0f);
        
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (!gameController.GetFightIsOn()) 
            {
                //Board hit
                if ((hit.transform.gameObject.tag == "Free" || hit.transform.gameObject.tag == "OccupiedByAlly") && hit.transform.gameObject.name.Length > 1)
                {
                    if (hit.transform.gameObject.name[1] - '0' > 3)
                    {
                        if (hit.transform.gameObject.name != hitTile.name)
                        {
                            hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_BaseColor", new Vector4(0f, 0.611f, 0.725f, 255));
                            hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_Transparency", new Vector4(0.3f, 0, 0, 0));

                        }
                        hitTile = hit.transform.gameObject;
                        hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_BaseColor", Color.white);
                        hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_Transparency", new Vector4(0.5f, 0, 0, 0));
                    }
                }
            }
            //Bench hit
            if ((hit.transform.gameObject.tag == "FreeBench" || hit.transform.gameObject.tag == "OccupiedByAlly") && hit.transform.gameObject.name.Length < 2)
            {
                if (hit.transform.gameObject.name != hitTile.name)
                {
                    hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_BaseColor", new Vector4(0f, 0.611f, 0.725f, 255));
                    hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_Transparency", new Vector4(0.3f, 0, 0, 0));
                }
                hitTile = hit.transform.gameObject;
                hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_BaseColor", Color.white);
                hitTile.transform.GetComponent<MeshRenderer>().material.SetVector("_Transparency", new Vector4(0.5f,0,0,0));
            }
        }

        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        cursorPosition.y = 0.0f;
        transform.position = cursorPosition;
    }


    

}