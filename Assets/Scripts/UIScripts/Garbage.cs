using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Garbage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UIController uiController;
    public bool isOver = false;

    private void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        print(true);
        uiController.isOverGarbage = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print(false);
        uiController.isOverGarbage = false;
    }
}
