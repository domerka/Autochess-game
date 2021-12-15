using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharImageButton : MonoBehaviour, IPointerClickHandler
{
    public ShopInstantiator shopInstantiator;

    public int characterCost;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable)
        {
            shopInstantiator.OnImageClicked(this.GetComponent<Image>().sprite, characterCost);
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/placeholder");
            this.GetComponent<Button>().interactable = false;
        }
    }

}
