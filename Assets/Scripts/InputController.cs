using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour 
{
    private string refreshShopInput;
    private string levelUpInput;
    private KeyCode openRightSidePanelInput;
    private KeyCode openSettingsInput;
    private string switchRightSidePanelPanelInput;
    private string sellCharacterInput;

    private UIController uiController;
    private GameController gameController;


    private void Awake()
    {
        refreshShopInput = "q";
        levelUpInput = "w";
        openRightSidePanelInput = KeyCode.Tab;
        openSettingsInput = KeyCode.Escape;
        switchRightSidePanelPanelInput = "1";
        sellCharacterInput = "e";

    }

    private void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        gameController = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>();
    }

    private void Update()
    {
        //Refresh shop 'q'
        if (Input.GetKeyDown(refreshShopInput)) uiController.RefreshShopButton();
        //Level up 'w'
        if (Input.GetKeyDown(levelUpInput)) uiController.LevelUp();
        //Open right side panel 'Tab'
        if (Input.GetKeyDown(openRightSidePanelInput)) uiController.OpenRightSideInfoPanel();
        //Switch between right side panels '1'
        if (Input.GetKeyDown(switchRightSidePanelPanelInput)) uiController.SwitchPanel();
        //Sell character 'e'
        if (Input.GetKeyDown(sellCharacterInput)) SellCharacter();
        //Open settings menu 'Esc'
        if (Input.GetKeyDown(openSettingsInput)) uiController.OpenSettingsPanel();
    }

    private void SellCharacter()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100.0f);
        foreach (RaycastHit hit in hits)
        {
            GameObject character = hit.transform.gameObject;
            if ((character.tag == "Ally" && character.GetComponent<CharacterController>().sellable) || character.tag == "OnBench")
            {
                character.GetComponent<CharacterController>().standingTile.tag = character.tag == "Ally" ? "Free" : "FreeBench";
                gameController.AddGold(character.GetComponent<CharacterController>().cost);
                Destroy(character);
                break;
            }
        }
    }


}
