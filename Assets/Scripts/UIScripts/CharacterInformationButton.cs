using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterInformationButton : MonoBehaviour, IPointerClickHandler
{
    public CharacterController characterController;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.GetComponent<Button>().interactable) characterController.OnCharacterInformationButtonClicked(this.name);
        this.GetComponent<Button>().interactable = false;
    }
}
