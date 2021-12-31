using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //The health of the player
    public int health;

    //The level of the player (upgrade skilltree based on this)
    public int level;

    //Image picture of the player (display on the right)
    public Image image;

    //Speed of its movement
    [SerializeField] private float speed;

    //rotationSpeed
    [SerializeField] private float rotationSpeed;

    Animator animationController;

    private bool allowMove = false;

    private Vector3 pointToMoveTo;

    private int fightStreak;

    private int skillPoints;

    private Image playerHealthBar;
    private TextMeshProUGUI playerSkillPointText;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthBar = transform.FindDeepChild("Fill").GetComponent<Image>();
        playerSkillPointText = transform.FindDeepChild("SkillPointText").GetComponent<TextMeshProUGUI>();
        playerSkillPointText.text = "1";
        animationController = this.GetComponent<Animator>();

        fightStreak = 0;

        skillPoints = 1;
        //Variable initialization
        health = 100;
        rotationSpeed = 10.0f;
        speed = 1.0f;
        pointToMoveTo = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        //Delete characterInformationPanel on left click
        if (Input.GetMouseButtonDown(0))
        {
            GraphicRaycaster gr = GameObject.FindGameObjectWithTag("UIController").transform.FindDeepChild("Canvas").GetComponent<GraphicRaycaster>();
            //Create the PointerEventData with null for the EventSystem
            PointerEventData ped = new PointerEventData(null);
            //Set required parameters, in this case, mouse position
            ped.position = Input.mousePosition;
            //Create list to receive all results
            List<RaycastResult> results = new List<RaycastResult>();
            //Raycast it
            gr.Raycast(ped, results);
            bool found = false;
            foreach(RaycastResult hit in results)
            {
                if (hit.gameObject.transform.name == "CharacterInformationPanel")
                {
                    found = true;
                    break;
                }
            }
            if (!found) Destroy(GameObject.Find("CharacterInformationPanel"));
            
        }

        //Move player
        if (Input.GetMouseButtonDown(1))
        {
            allowMove = true;
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                if(hit.transform.name.Length > 2)
                {
                    allowMove = false;
                    animationController.SetBool("Walk", false);
                }
                pointToMoveTo = hit.point;
            }
        }

        if (allowMove)
        {
            if (gameObject.transform.position == pointToMoveTo)
            {
                animationController.SetBool("Walk",false);
                allowMove = false;
            }
            else
            {
                animationController.SetBool("Walk", true);
                Move(pointToMoveTo);
            }
        }
    }

    private void Move(Vector3 pointToMoveTo)
    {
        //Rotate towards target point
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(gameObject.transform.forward, pointToMoveTo - gameObject.transform.position, singleStep, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDir);

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, pointToMoveTo, Time.deltaTime * speed);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        playerHealthBar.fillAmount = health / 100.0f;

        if (health <= 0)
        {
            //Game Over TODO
        }
    }

    Vector3 calculateWorldPoint()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Vector3 offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        cursorPosition.y = 0.0f;

        return cursorPosition;
    }

    public int GetStreak()
    {
        return fightStreak;
    }

    public void AddStreak(int amount)
    {
        fightStreak += amount;
    }

    public void SetStreak(int amount)
    {
        fightStreak = amount;
    }

    public int GetHealth()
    {
        return health;
    }
    public void AddSkillPoints(int amount)
    {
        skillPoints += amount;
        playerSkillPointText.text = skillPoints.ToString();
    }
    public int GetSkillPoints()
    {
        return skillPoints;
    }

    public void AddHealth(int amount)
    {
        health += amount;
    }

}
