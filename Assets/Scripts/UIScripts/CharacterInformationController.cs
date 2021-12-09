using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class CharacterInformationController : MonoBehaviour
{
    CharacterController characterController;

    public GameObject charInfoPanel;

    private Camera camera1;

    private string[] informationNames;

    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        camera1 = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        informationNames = new string[8] { "health","armor", "attackSpeed","magicDamage","attackDamage","magicResist","attackRange","critChance"};
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 screenPoint = camera1.WorldToScreenPoint(gameObject.transform.position);

            float xFloat = screenPoint.x / Screen.width;
            float yFloat = screenPoint.y / Screen.height;

            float tempX = xFloat * 1920;
            float tempY = yFloat * 1080;

            Vector2 solution = new Vector2(-(1920 - tempX),-( 1080 - tempY));

            GameObject temp = GameObject.Find("CharacterInformationPanel(Clone)");
            if(temp != null) Destroy(temp);
            
            // Right button clicked on this object
            charInfoPanel = Resources.Load("Prefabs/CharacterInformationPanel") as GameObject;
            GameObject inst = Instantiate(charInfoPanel, new Vector3(0,0,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            inst.GetComponent<RectTransform>().anchoredPosition = solution;

            for (int i = 0;i < 8; i++)
            {
                inst.transform.FindDeepChild(informationNames[i] + "Text").GetComponent<TextMeshProUGUI>().text = characterController.GetStatsString(informationNames[i]);
            }

            inst.layer = 5;

            SetupButtons(inst);

            inst.transform.FindDeepChild("middleButton").GetComponent<Button>().onClick.AddListener(MiddleButtonClick);

            //Fill rightPanel
            if(characterController.upgradedStats.Count > 0)
            {
                for(int i = 0; i < characterController.upgradedStats.Count; i++)
                {
                    inst.transform.FindDeepChild("upgradeImage"+(i+1)).GetComponent<Image>().sprite = inst.transform.FindDeepChild(characterController.upgradedStats[i] + "Image").GetComponent<Image>().sprite;
                }
            }
        }
    }

    private void SetupButtons(GameObject inst)
    {
        for(int i = 0;i < 8; i++)
        {
            if(characterController.upgradePoints < 0)
            {
                inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<CharacterInformationButton>().characterController = characterController;
                inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<Button>().interactable = false;
                continue;
            }
            print(characterController.upgradedStats.Count);
            switch(characterController.upgradedStats.Count)
            {
                case (2):
                    inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<Button>().interactable = false;
                    break;
                case (1):
                    if (characterController.upgradedStats[0] == informationNames[i]) 
                        inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<Button>().interactable = false;
                    else 
                        inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<CharacterInformationButton>().characterController = characterController;
                    break;
                default:
                    inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<CharacterInformationButton>().characterController = characterController;
                    break;
            }
        }
    }

    public void UpdateText(string statName,float amount)
    {
        GameObject temp = GameObject.Find("CharacterInformationPanel(Clone)");
        temp.transform.FindDeepChild(statName + "Text").GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public void DisableButtons()
    {
        GameObject temp = GameObject.Find("CharacterInformationPanel(Clone)");
        for(int i = 0;i < informationNames.Length; i++)
        {
            temp.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<Button>().interactable = false;
        }
        
    }

    public void SetImage(string statName)
    {
        GameObject temp = GameObject.Find("CharacterInformationPanel(Clone)");
        if (characterController.upgradedStats.Count == 1)
        {
            temp.transform.FindDeepChild("upgradeImage1").GetComponent<Image>().sprite  = temp.transform.FindDeepChild(statName + "Image").GetComponent<Image>().sprite;
            return;
        }
        temp.transform.FindDeepChild("upgradeImage2").GetComponent<Image>().sprite = temp.transform.FindDeepChild(statName + "Image").GetComponent<Image>().sprite;
        MakeThirdUpgrade();
    }
    //TODO
    private void MakeThirdUpgrade()
    {

    }
    private void MiddleButtonClick()
    {
        GameObject temp = GameObject.Find("CharacterInformationPanel(Clone)");

        if (temp.transform.FindDeepChild("RightPanel").gameObject.activeSelf)
        {
            temp.transform.FindDeepChild("RightPanel").gameObject.SetActive(false);
        }
        else
        {
            temp.transform.FindDeepChild("RightPanel").gameObject.SetActive(true);
        }
        
    }

}
