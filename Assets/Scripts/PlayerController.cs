using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




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

    Camera cam;

    private bool allowMove = false;

    private Vector3 pointToMoveTo;

    private int fightStreak;

    private int skillPoints;

    //healthBar


    // Start is called before the first frame update
    void Start()
    {
        animationController = this.GetComponent<Animator>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

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
        {/*
            GameObject temp = GameObject.Find("CharacterInformationPanel(Clone)");
            Destroy(temp);*/
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

        GameObject.FindGameObjectWithTag("UIController").gameObject.transform.FindDeepChild("PlayerHealthBar").GetComponent<Image>().fillAmount = health / 100.0f;

        GameObject.FindGameObjectWithTag("UIController").gameObject.transform.FindDeepChild("PlayerHealthText").GetComponent<TextMeshProUGUI>().text = health.ToString();

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
    }
    public int GetSkillPoints()
    {
        return skillPoints;
    }

}