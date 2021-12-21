using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private Button continueButton;
    private Button newGameButton;
    private Button loadGameButton;
    private Button optionsButton;
    private Button quitGameButton;
    private int selectedPanel;

    private void Awake()
    {
        selectedPanel = 0;
        continueButton = transform.FindDeepChild("ContinueButton").GetComponent<Button>();
        newGameButton = transform.FindDeepChild("NewGameButton").GetComponent<Button>();
        loadGameButton = transform.FindDeepChild("LoadGameButton").GetComponent<Button>();
        optionsButton = transform.FindDeepChild("OptionsButton").GetComponent<Button>();
        quitGameButton = transform.FindDeepChild("QuitGameButton").GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        continueButton.onClick.AddListener(ContinueButtonClick);
        newGameButton.onClick.AddListener(NewGameButtonClick);
        loadGameButton.onClick.AddListener(LoadGameButtonClick);
        optionsButton.onClick.AddListener(OptionsButtonClick);
        quitGameButton.onClick.AddListener(QuitGameButtonClick);
    }

    private void ContinueButtonClick()
    {
        selectedPanel = 0;
        //TODO
        //Load the last save

    }
    private void NewGameButtonClick()
    {
        selectedPanel = 1;
        //TODO
        //Open starting character chooser

    }
    private void LoadGameButtonClick()
    {
        selectedPanel = 2;
        //TODO
        //Load saved games

    }
    private void OptionsButtonClick()
    {
        selectedPanel = 3;
        //TODO 
        //Open option menu (graphics, subtitles, volume, etc...)

    }
    private void QuitGameButtonClick()
    {
        selectedPanel = 4;
        //TODO
        //Yes/No question before


        Application.Quit();
    }
}
