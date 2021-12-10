using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Garbage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UIController uiController;

    private void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        uiController.SetIsOverGarbage(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print(false);
        uiController.SetIsOverGarbage(false);
    }
}
