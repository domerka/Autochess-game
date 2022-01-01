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
        camera1 = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        informationNames = new string[10] { "health","armor", "attackSpeed","magicDamage","attackDamage","magicResist","attackRange","critChance","mana","critDamage"};
    }

    private void OnMouseOver()
    {
        // Right button clicked on this object
        if (Input.GetMouseButtonDown(1))
        {
            characterController = gameObject.GetComponent<CharacterController>();
            Vector2 screenPoint = camera1.WorldToScreenPoint(gameObject.transform.position);

            float xFloat = screenPoint.x / Screen.width;
            float yFloat = screenPoint.y / Screen.height;

            float tempX = xFloat * 1920;
            float tempY = yFloat * 1080;

            Vector2 solution = new Vector2(-(1920 - tempX),-( 1080 - tempY));

            GameObject temp = GameObject.Find("CharacterInformationPanel");
            if(temp != null) Destroy(temp);

            charInfoPanel = Resources.Load("Prefabs/CharacterInformationPanel") as GameObject;
            GameObject inst = Instantiate(charInfoPanel, new Vector3(0,0,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            inst.SetActive(false);
            inst.GetComponent<RectTransform>().anchoredPosition = solution;
            inst.name = "CharacterInformationPanel";

            //Fill name
            inst.transform.FindDeepChild("characterNameText").GetComponent<TextMeshProUGUI>().text = characterController.characterName;

            FillStats(inst);

            FillTraitTexts(inst);

            SetupButtons(inst);

            inst.SetActive(true);
        }
    }

    private void FillStats(GameObject inst)
    {
        for (int i = 0; i < 10; i++)
        {
            if (i == 0)
            {
                inst.transform.FindDeepChild(informationNames[i] + "Text").GetComponent<TextMeshProUGUI>().text = characterController.GetStatsString(informationNames[i]) + "/" + characterController.GetStatsString("maxHealth");
                inst.transform.FindDeepChild("healthFill").GetComponent<Image>().fillAmount = characterController.health / characterController.maxHealth;
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
    }

    private void FillTraitTexts(GameObject inst)
    {
        for(int i = 0; i < characterController.traitNames.Count; i++)
        {
            inst.transform.FindDeepChild("traitName" + i).GetComponent<TextMeshProUGUI>().text = characterController.traitNames[i];
            if (i == 2) return;
        }

        inst.transform.FindDeepChild("traitName2").GetComponent<TextMeshProUGUI>().text = "";
    }

    private void SetupButtons(GameObject inst)
    {
        /*
            if(characterController.upgradePoints < 0)
            {
                inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<CharacterInformationButton>().characterController = characterController;
                inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<Button>().interactable = false;
                continue;
            }*/
        for (int i = 0; i < characterController.upgradedStats.Count; i++)
        {
            inst.transform.FindDeepChild("upgradeImage" + (i+1)).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + characterController.upgradedStats[i] + "Icon");
            inst.transform.FindDeepChild(characterController.upgradedStats[i] + "Button").GetComponent<Button>().interactable = false;
            if(i == 1)
            {
                for (int k = 0; k < 10; k++)
                {
                    inst.transform.FindDeepChild(informationNames[k] + "Button").GetComponent<Button>().interactable = false;
                }
             }
        }

        for (int i = 0; i < 10; i++)
        {
            inst.transform.FindDeepChild(informationNames[i] + "Button").GetComponent<CharacterInformationButton>().characterController = characterController;
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
        for(int i = 0; i < characterController.upgradedStats.Count;i++)
        {
            temp.transform.FindDeepChild("upgradeImage" + characterController.upgradedStats.Count).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + statName + "Icon");
        }
        MakeThirdUpgrade();
    }
    //TODO
    private void MakeThirdUpgrade()
    {

    }

}
