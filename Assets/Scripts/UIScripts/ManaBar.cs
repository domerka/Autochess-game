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


    private void Update()
    {
        slider.value = CalculateMana();

        if (currentMana > maxMana) currentMana = maxMana;
    }
    private float CalculateMana()
    {
        return currentMana / maxMana;
    }

    public void setMaxMana(int _maxMana)
    {
        maxMana = _maxMana;

        currentMana = maxMana;
    }

    public bool castReady()
    {
        return currentMana == maxMana;
    }

    public void setMana(int amount)
    {
        currentMana = amount;
    }

    public void addMana(int amount)
    {
        currentMana += amount;
    }

}
