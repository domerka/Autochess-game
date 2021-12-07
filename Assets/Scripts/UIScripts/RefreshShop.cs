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
    private void Awake()
    {
        goldText = this.transform.FindDeepChild("GoldText").gameObject.GetComponent<TextMeshProUGUI>();
        nextIncomeText = this.transform.FindDeepChild("NextIncomeText").gameObject.GetComponent<TextMeshProUGUI>();
        goldInterestBar = this.transform.FindDeepChild("GoldInterestBar").gameObject;
        refreshShopButton = this.transform.FindDeepChild("RefreshShopButton").gameObject.GetComponent<Button>();
    }

    public void UpdateUI(int gold, int nextIncome)
    {
        goldText.text = gold.ToString();
        nextIncomeText.text = nextIncome.ToString();

        for (int i = 0; i < 5; i++)
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
}