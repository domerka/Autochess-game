using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeAbilityController : MonoBehaviour
{
    List<GameObject> skillTreeAbilities;

    public bool skillTreeSetup = false;

    private UIController uiController;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
    }

    public void LoadSave(List<GameObject> skillTreeAbilities)
    {

    }

    public void SetupSkillTree(List<int> serialNumbers)
    {
        skillTreeAbilities = new List<GameObject>();
        for (int i = 0; i < 12; i++)
        {
            skillTreeAbilities.Add(transform.GetChild(i).gameObject);
            skillTreeAbilities[i].GetComponent<SkillTreeAbility>().abilityDescription = i.ToString();
            if (i == 0)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities = null;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection1-8").gameObject);
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().SetUpgradable(true);
                skillTreeAbilities[i].GetComponent<Button>().interactable = true;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 1)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities = null;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection2-8").gameObject);
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().SetUpgradable(true);
                skillTreeAbilities[i].GetComponent<Button>().interactable = true;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 2)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities = null;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection3-9").gameObject);
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().SetUpgradable(true);
                skillTreeAbilities[i].GetComponent<Button>().interactable = true;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 3)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities = null;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection4-9").gameObject);
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().SetUpgradable(true);
                skillTreeAbilities[i].GetComponent<Button>().interactable = true;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 4)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities = null;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection5-10").gameObject);
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().SetUpgradable(true);
                skillTreeAbilities[i].GetComponent<Button>().interactable = true;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 5)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities = null;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection6-10").gameObject);
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().SetUpgradable(true);
                skillTreeAbilities[i].GetComponent<Button>().interactable = true;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 6)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities = null;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection7-10").gameObject);
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().SetUpgradable(true);
                skillTreeAbilities[i].GetComponent<Button>().interactable = true;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 7)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[0].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[1].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection8-11").gameObject);
                skillTreeAbilities[i].GetComponent<Button>().interactable = false;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 8)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[2].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[3].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection9-11").gameObject);
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection9-12").gameObject);
                skillTreeAbilities[i].GetComponent<Button>().interactable = false;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 9)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[4].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[5].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[6].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().connections.Add(transform.FindDeepChild("Connection10-12").gameObject);
                skillTreeAbilities[i].GetComponent<Button>().interactable = false;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 10)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[7].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[8].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<Button>().interactable = false;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }
            if (i == 11)
            {
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[8].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().previousAbilities.Add(skillTreeAbilities[9].GetComponent<SkillTreeAbility>());
                skillTreeAbilities[i].GetComponent<Button>().interactable = false;
                skillTreeAbilities[i].GetComponent<SkillTreeAbility>().serialNumber = serialNumbers[i];
                continue;
            }

        }
        transform.GetChild(12).gameObject.GetComponent<TextMeshProUGUI>().text = player.GetSkillPoints().ToString();
    }
    
    public void CheckUpgrades()
    {
        foreach(GameObject ob in skillTreeAbilities)
        {
            bool upgradable = true;
            if (ob.GetComponent<SkillTreeAbility>().previousAbilities != null)
            {
                foreach (SkillTreeAbility ability in ob.GetComponent<SkillTreeAbility>().previousAbilities)
                {
                    if (!ability.upgraded) upgradable = false;
                }

                if (upgradable  && !ob.GetComponent<SkillTreeAbility>().upgraded) 
                {
                    ob.GetComponent<SkillTreeAbility>().SetUpgradable(true);
                    if(ob.GetComponent<SkillTreeAbility>().GetCost() <= player.GetSkillPoints())  ob.GetComponent<Button>().interactable = true; 
                }
            }
        }
    }

    public void SetText()
    {
        if (skillTreeSetup)
        {
            foreach (GameObject ability in skillTreeAbilities)
            {
                if (!ability.GetComponent<SkillTreeAbility>().upgraded && ability.GetComponent<SkillTreeAbility>().GetCost() <= player.GetSkillPoints() && 
                    ability.GetComponent<SkillTreeAbility>().GetUpgradable()) ability.GetComponent<Button>().interactable = true;
            }
        }
        uiController.UpdatePlayerInformationPanel();
        transform.GetChild(12).gameObject.GetComponent<TextMeshProUGUI>().text = player.GetSkillPoints().ToString();
    }
    public int GetSkillPoints()
    {
        return player.GetSkillPoints();
    }

    public void AddSkillPoints(int amount)
    {
        player.AddSkillPoints(amount);
        if(player.GetSkillPoints() <= 0)
        {
            foreach (GameObject ability in skillTreeAbilities)
            {
                ability.GetComponent<Button>().interactable = false;
            }
        }
        uiController.UpdatePlayerInformationPanel();
    }

    public void AddSkillTreeBonus(int serialNumber)
    {
        uiController.AddSkillTreeBonus(serialNumber);
    }

   

   
}
