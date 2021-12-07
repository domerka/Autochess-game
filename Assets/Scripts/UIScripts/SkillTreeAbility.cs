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

                skillTreeAbilityController.AddSkillPoints(-cost);
                skillTreeAbilityController.CheckUpgrades();

                skillTreeAbilityController.SetText();
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

    /*
    private void Update()
    {
        if (IsPointerOverUIElement())
        {
        }
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }*/

}
