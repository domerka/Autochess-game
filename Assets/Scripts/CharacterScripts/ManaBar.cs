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

    public float CalculateMana()
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

        if (gameObject.GetComponent<CharacterController>().id != CharacterInformationPanel.id) return;
        gameObject.GetComponent<CharacterInformationController>().UpdateManaBar(CalculateMana());
        gameObject.GetComponent<CharacterInformationController>().UpdateManaText((int)currentMana);
    }

    public void AddMana(float amount)
    {
        currentMana += amount; 
        if (currentMana > maxMana) currentMana = maxMana;
        slider.value = CalculateMana();

        if (gameObject.GetComponent<CharacterController>().id != CharacterInformationPanel.id) return;
        gameObject.GetComponent<CharacterInformationController>().UpdateManaBar(CalculateMana());
        gameObject.GetComponent<CharacterInformationController>().UpdateManaText((int)currentMana);
    }

}
