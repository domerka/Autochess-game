using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeAbility : MonoBehaviour
{
    [SerializeField]public string abilityDescription;

    [SerializeField] public List<SkillTreeAbility> previousAbilities;

    [SerializeField] public List<GameObject> connections;

    [SerializeField] public bool upgraded;

    [SerializeField] private bool upgradable;

    [SerializeField] private Button button;

    [SerializeField] private int cost;

    public int serialNumber;

    private void Awake()
    {
        connections = new List<GameObject>();
        previousAbilities = new List<SkillTreeAbility>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        upgraded = false;
        upgradable = false;
        cost = 1;
    }


    void TaskOnClick()
    {
        if (gameObject.GetComponent<Button>().interactable)
        {
            SkillTreeAbilityController skillTreeAbilityController = gameObject.transform.parent.GetComponent<SkillTreeAbilityController>();
            if (skillTreeAbilityController.GetSkillPoints() >= cost)
            {
                upgraded = true;
                upgradable = false;

                foreach (GameObject connection in connections)
                {
                    for (int i = 0; i < connection.transform.childCount; i++)
                    {
                        connection.transform.FindDeepChild("Image" + (i + 1)).GetComponent<Image>().color = Color.yellow;
                    }
                }
                skillTreeAbilityController.OnSkillTreeAbilityClicked(-cost,serialNumber);
            }
        }
    }

    public int GetCost()
    {
        return cost;
    }

    public bool GetUpgradable()
    {
        return upgradable;
    }

    public void SetUpgradable(bool set)
    {
        upgradable = set;
    }

}
