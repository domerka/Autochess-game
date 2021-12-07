using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public float currentHealth;
    private float maxHealth;
    public Slider slider;
    bool dead = false;
    float animProgress = 0.00f;
    float offset = 0.0f;

    private void Start()
    {
        maxHealth = gameObject.GetComponent<CharacterController>().health;
        currentHealth = maxHealth;
        slider = gameObject.transform.FindDeepChild("HealthBar").GetComponent<Slider>();
        slider.value = CalculateHealth() ;
    }


    private void Update()
    {
        slider.value = CalculateHealth();

        if(currentHealth <= 0 && !dead)
        {
            SetupAccessories();
            gameObject.GetComponent<CharacterController>().standingTile.tag = "Free";
            offset = Time.time;
            animProgress = 0.0f;
            dead = true;
        }

        if(dead && Time.time-offset > animProgress)
        {
            animProgress += 0.1f;
            gameObject.transform.FindDeepChild("Cube").GetComponent<SkinnedMeshRenderer>().material.SetFloat("_AnimSpeed", animProgress);
            if(animProgress >= 1.0f) Destroy(gameObject);
        }

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

    }

    


    private void SetupAccessories()
    {
        gameObject.tag = "Dead";
        Destroy(gameObject.GetComponent<DragObject>());
        Destroy(gameObject.GetComponent<MoveObject>());
        Destroy(gameObject.GetComponent<CharacterController>());
        Destroy(gameObject.GetComponent<ManaBar>());
        Destroy(gameObject.GetComponent<CharacterInformationController>());
        Destroy(gameObject.transform.FindDeepChild("CharacterBars").gameObject);
        Destroy(gameObject.transform.FindDeepChild("sword").gameObject);
        gameObject.transform.FindDeepChild("Cube").GetComponent<SkinnedMeshRenderer>().material = Resources.Load("Materials/DeathShader") as Material;
    }

    private float CalculateHealth()
    {
        return currentHealth / maxHealth;
    }

    public void setMaxHealth(int amount)
    {
        maxHealth = amount;
    }


}
