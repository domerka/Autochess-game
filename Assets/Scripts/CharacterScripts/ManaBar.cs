using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public float currentMana;
    private float maxMana;

    public Slider slider;
    private void Start()
    {
        maxMana = gameObject.GetComponent<CharacterController>().maximumMana;
        currentMana = gameObject.GetComponent<CharacterController>().startingMana;
        slider = gameObject.transform.FindDeepChild("ManaBar").GetComponent<Slider>();
    }

    private float CalculateMana()
    {
        return currentMana / maxMana;
    }

    public bool CastReady()
    {
        return currentMana == maxMana;
    }

    public void SetMana(float amount)
    {
        currentMana = amount;
        slider.value = CalculateMana();
    }

    public void AddMana(float amount)
    {
        currentMana += amount; 
        if (currentMana > maxMana) currentMana = maxMana;
        slider.value = CalculateMana();
    }

}
