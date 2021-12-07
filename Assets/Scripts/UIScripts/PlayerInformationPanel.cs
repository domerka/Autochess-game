using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerInformationPanel : MonoBehaviour
{
    private Image playerHealthBar;
    private TextMeshProUGUI playerStreakText;
    private TextMeshProUGUI playerHealthText;
    private TextMeshProUGUI playerSkillPointText;

    private void Awake()
    {
        playerHealthBar = transform.FindDeepChild("PlayerHealthBar").GetComponent<Image>();
        playerStreakText = transform.FindDeepChild("PlayerStreakText").GetComponent<TextMeshProUGUI>();
        playerHealthText = transform.FindDeepChild("PlayerHealthText").GetComponent<TextMeshProUGUI>();
        playerSkillPointText = transform.FindDeepChild("PlayerSkillPointText").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateUI(int playerStreak, int playerHealth, int playerSkillPoints, int health)
    {
        playerStreakText.text = playerStreak.ToString();
        playerHealthText.text = playerHealth.ToString();
        playerSkillPointText.text = playerSkillPoints.ToString();
        playerHealthBar.fillAmount = health / 100.0f;
    }
}
