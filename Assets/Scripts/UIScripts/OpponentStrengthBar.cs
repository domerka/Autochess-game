using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpponentStrengthBar : MonoBehaviour
{
    private Image mask;
    private void Awake()
    {
        mask = transform.FindDeepChild("Mask").gameObject.GetComponent<Image>();
    }
    
    public void UpdateUI(float amount)
    {
        mask.fillAmount = amount / 5;
    }
}
