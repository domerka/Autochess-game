using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopOdds : MonoBehaviour
{
    private List<TextMeshProUGUI> oddsTexts;

    private void Awake()
    {
        oddsTexts = new List<TextMeshProUGUI>();
        for (int i = 1; i <= 5; i++) oddsTexts.Add(transform.FindDeepChild("Odds" + i).gameObject.GetComponent<TextMeshProUGUI>());
    }

    public void UpdateUI(int[] odds)
    {
        for(int i = 0; i < 5; i++) oddsTexts[i].text = odds[i].ToString() + "%";
    }
}
