using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject skillTree;
    public GameObject damagePanel;
    public GameObject healPanel;
    public GameObject rightSideInfoPanel;
    private GameController gameController;

    private List<GameObject> allyDamageDealt;
    private List<GameObject> enemyDamageDealt;
    private List<GameObject> allyDamageHealed;
    private List<GameObject> enemyDamageHealed;

    private OpponentStrengthBar osb;
    private LevelUp levelUp;
    private RefreshShop refreshShop;
    public ShopInstantiator shopInstantiator;
    private PlayerInformationPanel playerInformationPanel;
    private TextMeshProUGUI teamSizeText;
    private PlayerController player;
    private GameObject garbageLeft;
    private GameObject garbageRight;
    private ShopOdds shopOdds;

    public GameObject settingsPanel;

    private bool isOverGarbage;

    private int panelOpen;

    private void Awake()
    {
        allyDamageDealt = new List<GameObject>();
        enemyDamageDealt = new List<GameObject>();
        allyDamageHealed = new List<GameObject>();
        enemyDamageHealed = new List<GameObject>();

        osb = transform.FindDeepChild("OpponentStrengthBar").gameObject.GetComponent<OpponentStrengthBar>();
        levelUp = transform.FindDeepChild("LevelUp").gameObject.GetComponent<LevelUp>();
        refreshShop = transform.FindDeepChild("RefreshShop").gameObject.GetComponent<RefreshShop>();
        shopInstantiator = transform.FindDeepChild("ShopPictures").gameObject.GetComponent<ShopInstantiator>();
        playerInformationPanel = transform.FindDeepChild("PlayerInformationPanel").gameObject.GetComponent<PlayerInformationPanel>();
        player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        garbageLeft = transform.FindDeepChild("GarbageLeft").gameObject;
        garbageRight = transform.FindDeepChild("GarbageRight").gameObject;
        shopOdds = transform.FindDeepChild("ShopOdds").gameObject.GetComponent<ShopOdds>();
        teamSizeText = transform.FindDeepChild("TeamSizeText").gameObject.GetComponent<TextMeshProUGUI>();
        panelOpen = 0;
        damagePanel.SetActive(false);
        healPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>();
        UpdateUI();
    }

    //-----------------------------RightSideInfoPanel functions
    public void OpenRightSideInfoPanel()
    {

        if (!skillTree.GetComponent<SkillTreeAbilityController>().skillTreeSetup)
        {
            skillTree.GetComponent<SkillTreeAbilityController>().SetupSkillTree();
            skillTree.GetComponent<SkillTreeAbilityController>().skillTreeSetup = true;
        }

        float skillTreeStratingPoint = 385;
        //Open
        if (rightSideInfoPanel.GetComponent<RectTransform>().anchoredPosition.x == skillTreeStratingPoint)
        {
            gameObject.transform.FindDeepChild("RightSidePanelOpenerButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/SkillTreeOpenerRight");
            LeanTween.moveX(rightSideInfoPanel.GetComponent<RectTransform>(), -skillTreeStratingPoint, 0.25f);
        }
        //Close
        else
        {
            gameObject.transform.FindDeepChild("RightSidePanelOpenerButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/SkillTreeOpenerLeft");
            LeanTween.moveX(rightSideInfoPanel.GetComponent<RectTransform>(), skillTreeStratingPoint, 0.25f);
        }
    }
    private void SwitchRightSidePanel()
    {
        switch (panelOpen)
        {
            case (0):
                this.transform.FindDeepChild("SkillTreePanelButton").GetComponent<Button>().interactable = false;
                this.transform.FindDeepChild("DamagePanelButton").GetComponent<Button>().interactable = true;
                this.transform.FindDeepChild("HealPanelButton").GetComponent<Button>().interactable = true;
                damagePanel.SetActive(false);
                healPanel.SetActive(false);
                skillTree.SetActive(true);
                panelOpen = 0;
                break;
            case (1):
                this.transform.FindDeepChild("SkillTreePanelButton").GetComponent<Button>().interactable = true;
                this.transform.FindDeepChild("DamagePanelButton").GetComponent<Button>().interactable = false;
                this.transform.FindDeepChild("HealPanelButton").GetComponent<Button>().interactable = true;
                damagePanel.SetActive(true);
                healPanel.SetActive(false);
                skillTree.SetActive(false);
                panelOpen = 1;
                break;
            case (2):
                this.transform.FindDeepChild("SkillTreePanelButton").GetComponent<Button>().interactable = true;
                this.transform.FindDeepChild("DamagePanelButton").GetComponent<Button>().interactable = true;
                this.transform.FindDeepChild("HealPanelButton").GetComponent<Button>().interactable = false;
                damagePanel.SetActive(false);
                healPanel.SetActive(true);
                skillTree.SetActive(false);
                panelOpen = 2;
                break;

        }
    }
    public void OnRightInfoPanelButtonClicked(string buttonName)
    {
        switch (buttonName)
        {
            case ("SkillTreePanelButton"):
                this.transform.FindDeepChild(buttonName).GetComponent<Button>().interactable = false;
                this.transform.FindDeepChild("DamagePanelButton").GetComponent<Button>().interactable = true;
                this.transform.FindDeepChild("HealPanelButton").GetComponent<Button>().interactable = true;
                damagePanel.SetActive(false);
                healPanel.SetActive(false);
                skillTree.SetActive(true);
                panelOpen = 0;
                break;

            case ("DamagePanelButton"):
                this.transform.FindDeepChild(buttonName).GetComponent<Button>().interactable = false;
                this.transform.FindDeepChild("SkillTreePanelButton").GetComponent<Button>().interactable = true;
                this.transform.FindDeepChild("HealPanelButton").GetComponent<Button>().interactable = true;
                damagePanel.SetActive(true);
                healPanel.SetActive(false);
                skillTree.SetActive(false);
                panelOpen = 1;
                break;
            case ("HealPanelButton"):
                this.transform.FindDeepChild(buttonName).GetComponent<Button>().interactable = false;
                this.transform.FindDeepChild("SkillTreePanelButton").GetComponent<Button>().interactable = true;
                this.transform.FindDeepChild("DamagePanelButton").GetComponent<Button>().interactable = true;
                damagePanel.SetActive(false);
                healPanel.SetActive(true);
                skillTree.SetActive(false);
                panelOpen = 2;
                break;
        }
    }
    public void SwitchPanel()
    {
        panelOpen++;
        if (panelOpen >= 3)
        {
            panelOpen = 0;
        }
        SwitchRightSidePanel();
    }
    public void CreateStatLayout(GameObject[] characters, string characterType, string type)
    {
        GameObject layout = this.transform.FindDeepChild(type + "Layout" + characterType).gameObject;
        for (int i = 0; i < layout.transform.childCount; i++)
        {
            Destroy(layout.transform.GetChild(i).gameObject);
        }


        foreach (GameObject ally in characters)
        {
            GameObject inst = Instantiate(Resources.Load("Prefabs/TextPrefab") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
            inst.transform.SetParent(layout.transform);
            inst.GetComponent<StoreValue>().value = 0;
            if (type == "Heal")
            {
                inst.name = ally.GetComponent<CharacterController>().name;
                inst.GetComponent<TextMeshProUGUI>().text = ally.GetComponent<CharacterController>().name + ": " + ally.GetComponent<CharacterController>().damageHealed;
                if (characterType == "Ally")
                {
                    allyDamageHealed.Add(inst);
                }
                else
                {
                    inst.GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Right;
                    enemyDamageHealed.Add(inst);
                }
            }
            else
            {
                inst.name = ally.GetComponent<CharacterController>().name;
                inst.GetComponent<TextMeshProUGUI>().text = ally.GetComponent<CharacterController>().name + ": " + ally.GetComponent<CharacterController>().damageDealt;
                if (characterType == "Ally")
                {
                    allyDamageDealt.Add(inst);
                }
                else
                {
                    inst.GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Right;
                    enemyDamageDealt.Add(inst);
                }
            }
        }
    }
    public void UpdateDamageLayout(CharacterController character)
    {
        this.transform.FindDeepChild(character.name).gameObject.GetComponent<TextMeshProUGUI>().text = character.name + ": " + character.damageDealt;
        this.transform.FindDeepChild(character.name).gameObject.GetComponent<StoreValue>().value = character.damageDealt;

        if (character.type == "Ally")
        {
            allyDamageDealt.Sort(CompareSort);
            for (int i = 0; i < allyDamageDealt.Count; i++)
            {
                allyDamageDealt[i].transform.SetSiblingIndex(allyDamageDealt.Count - 1 - i);
            }
        }
        else
        {
            enemyDamageDealt.Sort(CompareSort);
            for (int i = 0; i < allyDamageDealt.Count; i++)
            {
                enemyDamageDealt[i].transform.SetSiblingIndex(enemyDamageDealt.Count - 1 - i);
            }
        }
    }
    //-----------------------------Basic UI update functions
    public void UpdateUI()
    {
        UpdateUIOnGoldSpent();
        UpdateUIOnLevelUp();
        UpdatePlayerInformationPanel();
        RefreshShop();
    }
    public void UpdateUIOnLevelUp()
    {
        levelUp.UpdateUI(gameController.GetLevel(),gameController.GetXp(), gameController.GetXpForNextLevel(),gameController.GetGold());

        //Skill points
        skillTree.GetComponent<SkillTreeAbilityController>().SetText();

        shopOdds.UpdateUI(gameController.GetShopOdds());

        UpdateTeamSizeUI();
    }
    public void UpdateUIOnGoldSpent()
    {
        UpdateUIOnLevelUp();
        refreshShop.UpdateUI(gameController.GetGold(), gameController.GetNextIncome());
        shopInstantiator.UpdateUI(gameController.GetGold());
    }
    public void UpdatePlayerInformationPanel()
    {
        playerInformationPanel.UpdateUI(player.GetStreak(), player.GetHealth(), player.GetSkillPoints(),player.GetHealth());
    }
    public void UpdateOSB(float amount)
    {
        osb.UpdateUI(amount);
    }
    //TODO
    public void UpdateTeamCombinationsUI()
    {

    }
    //-----------------------------Bottom UI functions
    public void RefreshShop()
    {
        shopInstantiator.FillShopPictures(gameController.GetShopPictures(), gameController.GetGold());
    }
    public void RefreshShopButton()
    {
        if (gameController.RefreshShop())
            shopInstantiator.FillShopPictures(gameController.GetShopPictures(), gameController.GetGold());

        UpdateUIOnGoldSpent();
    }
    public void LevelUp()
    {
        if (gameController.LevelUp())
        {
            UpdateUIOnLevelUp();
            UpdateUIOnGoldSpent();
        }
    }
    public void BuyCharacter(int cost)
    {
        if (gameController.BuyCharacter(cost))
        {
            UpdateUIOnGoldSpent();
        }
    }
    public void UpdateTeamSizeUI()
    {
        teamSizeText.text = gameController.GetNumberOfChampionsOnBoard() + "/" + gameController.GetTeamSize();
    }
    public void OpenSettingsPanel()
    {
        if (settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
        else
        {
            settingsPanel.SetActive(true);
        }

    }
    //-----------------------------Garbage functions
    public void ShowGarbage(int cost)
    {
        garbageLeft.SetActive(true);
        garbageRight.SetActive(true);

        garbageLeft.transform.FindDeepChild("GarbageText").GetComponent<TextMeshProUGUI>().text = "Sell for \n" + cost + " gold";
        garbageRight.transform.FindDeepChild("GarbageText").GetComponent<TextMeshProUGUI>().text = "Sell for \n" + cost + " gold";
    }
    public void HideGarbage()
    {
        garbageLeft.SetActive(false);
        garbageRight.SetActive(false);
    }
    //-----------------------------Helper functions
    private int CompareSort(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<StoreValue>().value.CompareTo(p2.GetComponent<StoreValue>().value);
    }
    public bool GetFightIsOn()
    {
        return gameController.GetFightIsOn();
    }
    public bool GetIsOverGarbage()
    {
        return isOverGarbage;
    }
    public void SetIsOverGarbage(bool set)
    {
        isOverGarbage = set;
    }



    //TODO
    private void DisplayUIInformation()
    {
        //Displays help and information on UI elements (onMouseOver)

    }
    

    

}
