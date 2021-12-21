using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelController : MonoBehaviour
{
    public Text timerText;
    public Image timerSlider;

    public TextMeshProUGUI stageText;

    private GameController gameController;

    private UIController uiController;

    private int opponentStrength;
    private int stageCounter;
    private float currentTime;
    private float time;
    private int numOfStages;

    //Stage lengths
    [SerializeField]private float preparationStageLength;
    [SerializeField] private float fightLength;
    [SerializeField] private float overTimeLength;
    [SerializeField] private float loadingStageLength;

    private bool isPreparationStage;
    private bool isOvertimeStage;

    private void Awake()
    {
        opponentStrength = 0;
        numOfStages = 5;
        stageCounter = -1;

        preparationStageLength = 15.0f;
        fightLength = 30.0f;
        overTimeLength = 10.0f;
        loadingStageLength = 2.0f;

        SetTime(preparationStageLength);
        isPreparationStage = true;
        isOvertimeStage = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        gameController = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>();
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        ManageOpponentStrength();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        //Update UI
        timerText.text = currentTime.ToString("0");
        timerSlider.fillAmount = (float)currentTime / (float)time;

        //Very first preparation stage
        if (stageCounter == -1)
        {
            gameController.SpawnEnemies(1);
            stageCounter++;
            stageText.text = "Fight number: " + (stageCounter + 1);
        }
        
        //upgrade units after fight, 1 second into preparationstage
        if(currentTime <= preparationStageLength-1.0f && isPreparationStage && currentTime >= preparationStageLength-2.0f) gameController.CheckCharacterUpgrades();
        

        
        if(currentTime <= 0 || (gameController.CheckWhetherFightIsOver() && gameController.GetFightIsOn()))
        {
            //Skip overtime stage if the fight ended in normal time
            if (gameController.CheckWhetherFightIsOver() && gameController.GetFightIsOn() && !isOvertimeStage) stageCounter++;

            stageCounter++;
            stageText.text = "Fight number: " + Mathf.CeilToInt((stageCounter / numOfStages));

            switch(stageCounter % numOfStages)
            {
                //Preparation stage
                case (0):
                    SetPreparationStage();
                    break;
                //Fight preparation stage
                case (1):
                    SetFightPreparationStage();
                    break;
                //Fight stage
                case (2):
                    SetFightStage();
                    break;
                //Overtime stage
                case (3):
                    SetOvertimeStage();
                    break;
                //End of the fight stage
                case (4):
                    SetEndFightStage();
                    break;
            }
        }
    }

    public void LoadSave()
    {

    }

    //------------------------------------Stage controller functions
    private void SetPreparationStage()
    {
        if (Mathf.CeilToInt(stageCounter / numOfStages) % 5 == 0) ManageOpponentStrength();
        SetTime(preparationStageLength);
        gameController.SetPreparationStage();
        gameController.SpawnEnemies(0);
        isPreparationStage = true;
    }
    private void SetFightPreparationStage()
    {
        SetTime(loadingStageLength);
        gameController.SetFightPreparationStage();
    }
    private void SetFightStage()
    {
        SetTime(fightLength);
        gameController.SetFightStage();
        isPreparationStage = false;
    }
    private void SetOvertimeStage()
    {
        isOvertimeStage = true;
        SetTime(overTimeLength);
        gameController.SetOvertimeStage();
    }
    private void SetEndFightStage()
    {
        isOvertimeStage = false;
        SetTime(loadingStageLength);
        gameController.SetEndFightStage();
    }
    //------------------------------------

    //Should be done by gameController
    private void ManageOpponentStrength()
    {
        opponentStrength++;
        uiController.UpdateOSB(opponentStrength);
    }

    private void SetTime(float timeToSet)
    {
        time = timeToSet;
        currentTime = time;
    }

    public void SkipPreparationStage()
    {
        if(isPreparationStage && currentTime > preparationStageLength/2.0f && currentTime < preparationStageLength - 1.0f)
        {
            currentTime = 0.0f;
            gameController.AddGold(1);
        }
    }


}
