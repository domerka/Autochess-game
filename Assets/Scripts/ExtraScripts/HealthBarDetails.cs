using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarDetails : MonoBehaviour
{
    public static void Add(GameObject character)
    {
        CharacterController charControl = character.GetComponent<CharacterController>();

        GameObject characterBar = character.transform.FindDeepChild("CharacterBars").gameObject;

        GameObject healthBar = character.transform.FindDeepChild("HealthBar").gameObject;

        for (int i = 1; i < healthBar.transform.childCount; i++) 
        {
            Destroy(healthBar.transform.GetChild(i).gameObject);
        }


        int tickSize = 5;
        float healthChunkSize = 250;

        int numOfTicks = Mathf.FloorToInt((charControl.health - 1) / healthChunkSize);// this '-10' is because of the spacing 
        //print("name: " + character.name + " numOfTick:" + numOfTicks);
        float x = (characterBar.GetComponent<RectTransform>().rect.width - 10 - (tickSize * numOfTicks)) / (charControl.health / healthChunkSize);
        //The 250 here is the localPos of healthBar -------------------Warning
        float offset = tickSize / 2.0f - 250;
        for (int i = 1; i <= numOfTicks; i++)
        {
            GameObject inst = Instantiate(Resources.Load("Prefabs/Tick") as GameObject, new Vector3(0, 0, 0), Quaternion.identity, healthBar.transform);
            inst.GetComponent<RectTransform>().localPosition = new Vector3(i * x + offset, 0.0f, 0.0f);
            inst.GetComponent<RectTransform>().localRotation = Quaternion.identity;
            offset += tickSize;
        }
    }

}
