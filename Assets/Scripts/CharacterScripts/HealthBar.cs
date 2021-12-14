using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth;
    private Slider slider;
    private bool dead = false;
    private float deathAnimProgress = 0.00f;
    private float deathTimerOffset = 0.0f;

    private void Start()
    {
        maxHealth = gameObject.GetComponent<CharacterController>().health;
        currentHealth = maxHealth;
        slider = gameObject.transform.FindDeepChild("HealthBar").GetComponent<Slider>();
        slider.value = CalculateHealth() ;
    }


    private void Update()
    {
        if (dead && Time.time - deathTimerOffset > deathAnimProgress)
        {
            deathAnimProgress += 0.1f;
            gameObject.transform.FindDeepChild("Cube").GetComponent<SkinnedMeshRenderer>().material.SetFloat("_AnimSpeed", deathAnimProgress);
            if (deathAnimProgress >= 1.0f) Destroy(gameObject);
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

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        slider.value = CalculateHealth();

        if (currentHealth <= 0 && !dead)
        {
            SetupAccessories();
            gameObject.GetComponent<CharacterController>().standingTile.tag = "Free";
            gameObject.GetComponent<CharacterController>().standingTile.GetComponent<Tile>().isObstacle = false;
            deathTimerOffset = Time.time;
            deathAnimProgress = 0.0f;
            dead = true;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void HealDamage(float amount)
    {
        currentHealth += amount;
    }

}
