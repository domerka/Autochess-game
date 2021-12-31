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
        informationNames = new string[10] { "health","armor", "attackSpeed","magicDamage","attackDamage","magicResist","attackRange","critChance","mana","critDamage"};
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

            GameObject temp = GameObject.Find("CharacterInformationPanel");
            if(temp != null) Destroy(temp);
            
            // Right button clicked on this object
            charInfoPanel = Resources.Load("Prefabs/CharacterInformationPanel") as GameObject;
            GameObject inst = Instantiate(charInfoPanel, new Vector3(0,0,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            inst.GetComponent<RectTransform>().anchoredPosition = solution;
            inst.name = "CharacterInformationPanel";

            for (int i = 0;i < 10; i++)
            {
                if (i == 0)
                {
                    inst.transform.FindDeepChild(informationNames[i] + "Text").GetComponent<TextMeshProUGUI>().text = characterController.GetStatsString(informationNames[i]) + "/" + characterController.GetStatsString("maxHealth");
                    inst.transform.FindDeepChild("healthFill").GetComponent<Image>().fillAmount = characterController.health/characterController.maxHealth;
                    continue;
                }
                if (i == 8) 
                { 
                    inst.transform.FindDeepChild(informationNames[i] + "Text").GetComponent<TextMeshProUGUI>().text = characterController.GetStatsString(informationNames[i]) + "/" + characterController.GetStatsString("maxMana");
                    inst.transform.FindDeepChild("manaFill").GetComponent<Image>().fillAmount = characterController.GetCurrentMana() / characterController.maximumMana;
                    continue;
                }
                inst.transform.FindDeepChild(informationNames[i] + "Text").GetComponent<TextMeshProUGUI>().text = characterController.GetStatsString(informationNames[i]);
            }

            SetupButtons(inst);
        }
    }

    private void SetupButtons(GameObject inst)
    {
        for(int i = 0;i < 10; i++)
        {
            /*
            if(characterController.upgradePoints < 0)
            {
                inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<CharacterInformationButton>().characterController = characterController;
                inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<Button>().interactable = false;
                continue;
            }*/
            switch(characterController.upgradedStats.Count)
            {
                case (2):
                    inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<Button>().interactable = false;
                    inst.transform.FindDeepChild("upgradeImage1").GetComponent<Image>().sprite = inst.transform.FindDeepChild(characterController.upgradedStats[0] + "Image").GetComponent<Image>().sprite;
                    inst.transform.FindDeepChild("upgradeImage2").GetComponent<Image>().sprite = inst.transform.FindDeepChild(characterController.upgradedStats[1] + "Image").GetComponent<Image>().sprite;
                    break;
                case (1):
                    inst.transform.FindDeepChild("upgradeImage1").GetComponent<Image>().sprite = inst.transform.FindDeepChild(characterController.upgradedStats[0] + "Image").GetComponent<Image>().sprite;
                    if (characterController.upgradedStats[0] == informationNames[i]) 
                        inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<Button>().interactable = false;
                    else 
                        inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<CharacterInformationButton>().characterController = characterController;
                    break;
                case(0):
                    inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<CharacterInformationButton>().characterController = characterController;
                    break;
            }
        }
    }

    public void UpdateText(string statName,float amount)
    {
        GameObject temp = GameObject.Find("CharacterInformationPanel");
        if(statName == "health")
        {
            temp.transform.FindDeepChild(statName + "Text").GetComponent<TextMeshProUGUI>().text = characterController.GetStatsString(informationNames[0]) + "/" + characterController.GetStatsString("maxHealth"); ;
            return;
        }
        temp.transform.FindDeepChild(statName + "Text").GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public void DisableButtons()
    {
        GameObject temp = GameObject.Find("CharacterInformationPanel");
        for(int i = 0;i < informationNames.Length; i++)
        {
            temp.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<Button>().interactable = false;
        }
        
    }

    public void SetImage(string statName)
    {
        GameObject temp = GameObject.Find("CharacterInformationPanel");
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

}
