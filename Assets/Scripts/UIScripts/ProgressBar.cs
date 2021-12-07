using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private int maximumXp;
    private int currentXp;

    private int maximumGold;
    private int currentGold;

    private int maximumStrength;
    private int currentStrength;

    public int type;

    public Image mask;

    // Start is called before the first frame update
    void Start()
    {
        maximumXp = 5;
        currentXp = 0;

        maximumGold = 100;
        currentGold = 100;

        maximumStrength = 5;
        currentStrength = 1;

        if(type == 0)
        {
            mask.fillAmount = (float)currentXp / (float)maximumXp;
        }
        else if(type == 1)
        {
            mask.fillAmount = (float)currentGold / (float)maximumGold;
        }
        else
        {
            mask.fillAmount = (float)currentStrength / (float)maximumStrength;
        }
    }

    public void SetFillAmount(int current, int max)
    {
        mask.fillAmount = (float)current / (float)max;
    }


}
