using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelUp : MonoBehaviour
{
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI levelProgressionText;
    private GameObject levelProgressionBar;
    private Button levelUpButton;

    private void Awake()
    {
        levelText = this.transform.FindDeepChild("LevelText").GetComponent<TextMeshProUGUI>();
        levelProgressionText = this.transform.FindDeepChild("LevelProgressionText").GetComponent<TextMeshProUGUI>();
        levelProgressionBar = this.transform.FindDeepChild("LevelProgressionBar").gameObject;
        levelUpButton = this.transform.FindDeepChild("LevelUpButton").GetComponent<Button>();
    }
    
    public void UpdateUI(int level, int xp, int xpForNextLevel, int gold, int maxLevel)
    {
        if(level == maxLevel)
        {
            levelText.text = level.ToString();
            levelProgressionText.text = "Max.";
            SetButtonInteractable(false);
            return;
        }


        levelText.text = level.ToString();
        levelProgressionText.text = xp + "/" + xpForNextLevel;

        GameObject[] levelProgImages = GameObject.FindGameObjectsWithTag("LevelProgressionBarImage");
        //Place them in
        for (int i = 0; i < (xpForNextLevel / 4 - levelProgImages.Length); i++)
        {
            GameObject inst = Instantiate(Resources.Load("Prefabs/ProgressionBarImage"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            inst.transform.SetParent(gameObject.transform.FindDeepChild("LevelProgressionBar").gameObject.transform);
            inst.tag = "LevelProgressionBarImage";
        }
        levelProgImages = GameObject.FindGameObjectsWithTag("LevelProgressionBarImage");
        //Color them
        for (int i = 0; i < levelProgImages.Length; i++)
        {
            if ((i + 1) <= Mathf.Floor(xp / 4))
            {
                //Filled
                levelProgImages[i].transform.Find("Mask").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/LevelProgressionBarImageFilled");
                levelProgImages[i].transform.Find("Mask").GetComponent<Image>().fillAmount = 1.0f;
                continue;
            }
            if ((i + 1) == Mathf.Floor(xp / 4) + 1 && xp % 4 != 0)
            {
                //Half filled
                levelProgImages[i].transform.Find("Mask").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/LevelProgressionBarImageFilled");
                if (xp % 4 == 1) levelProgImages[i].transform.Find("Mask").GetComponent<Image>().fillAmount = 0.25f;
                if (xp % 4 == 2) levelProgImages[i].transform.Find("Mask").GetComponent<Image>().fillAmount = 0.5f;
                if (xp % 4 == 3) levelProgImages[i].transform.Find("Mask").GetComponent<Image>().fillAmount = 0.75f;
                continue;
            }
            //Not filled
            levelProgImages[i].transform.Find("Mask").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/LevelProgressionBarImage");
        }

        SetButtonInteractable(gold >= 4);
    }
    private void SetButtonInteractable(bool interactable)
    {
        levelUpButton.interactable = interactable;
    }
}

