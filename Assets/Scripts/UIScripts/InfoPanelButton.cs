using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoPanelButton : MonoBehaviour, IPointerClickHandler
{
    public UIController uiController;

    private void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable)
        {
            uiController.OnRightInfoPanelButtonClicked(this.name);
        }
    }
}
