using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RefreshShop : MonoBehaviour
{
    private TextMeshProUGUI goldText;
    private TextMeshProUGUI nextIncomeText;
    private GameObject goldInterestBar;
    private Button refreshShopButton;
    private TextMeshProUGUI playerStreakText;

    private void Awake()
    {
        goldText = transform.FindDeepChild("GoldText").gameObject.GetComponent<TextMeshProUGUI>();
        nextIncomeText = transform.FindDeepChild("NextIncomeText").gameObject.GetComponent<TextMeshProUGUI>();
        goldInterestBar = transform.FindDeepChild("GoldInterestBar").gameObject;
        refreshShopButton = transform.FindDeepChild("RefreshShopButton").gameObject.GetComponent<Button>();
        playerStreakText = transform.FindDeepChild("StreakText").gameObject.GetComponent<TextMeshProUGUI>();
        playerStreakText.text = "0";
    }

    public void UpdateUI(int gold, int nextIncome, int maxIncome, int playerStreak)
    {
        goldText.text = gold.ToString();
        nextIncomeText.text = nextIncome.ToString();
        playerStreakText.text = Mathf.Abs(playerStreak).ToString();
        playerStreakText.color = playerStreak >= 0 ? Color.red : Color.blue;

        for (int i = 0; i < maxIncome/10; i++)
        {
            if (i + 1 <= Mathf.Floor(gold / 10)) goldInterestBar.transform.FindDeepChild("GoldInterestBar" + (i + 1)).GetComponent<Image>().color = Color.yellow;
            else goldInterestBar.transform.FindDeepChild("GoldInterestBar" + (i + 1)).GetComponent<Image>().color = Color.white;
        }

        SetButtonInteractable(gold >= 2);
    }

    private void SetButtonInteractable(bool interactable)
    {
        refreshShopButton.interactable = interactable;
    }

    public void AddBarToInterestBar()
    {
        GameObject inst = Instantiate(Resources.Load("Prefabs/GoldInterestBar")as GameObject, goldInterestBar.transform);
        inst.name = "GoldInterestBar6";
    }
}
