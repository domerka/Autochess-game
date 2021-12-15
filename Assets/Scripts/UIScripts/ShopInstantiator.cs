using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ShopInstantiator : MonoBehaviour
{
    private List<GameObject> shopPictures;

    private GameController gameController;

    private void Awake()
    {
        shopPictures = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            shopPictures.Add(transform.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>();
    }

    public void FillShopPictures(Dictionary<List<Sprite>, List<int>> shopPictureInfos, int gold)
    {
        var e = shopPictureInfos.GetEnumerator();
        e.MoveNext();
        for(int i = 0; i < 5; i++)
        {
            shopPictures[i].GetComponent<Image>().sprite = e.Current.Key[i];
            shopPictures[i].GetComponent<CharImageButton>().characterCost = e.Current.Value[i];
            shopPictures[i].GetComponent<Button>().interactable = shopPictures[i].GetComponent<CharImageButton>().characterCost > gold ? false : true;
        }
    }
    public void UpdateUI(int gold)
    {
        foreach (GameObject shopPicture in shopPictures)
        {
            if (shopPicture.GetComponent<Image>().sprite.name == "placeholder") continue;
            shopPicture.GetComponent<Button>().interactable = shopPicture.GetComponent<CharImageButton>().characterCost > gold ? false : true; 
        }
    }
    public void OnImageClicked(Sprite button, int characterCost)
    {
        gameController.OnShopImageClicked(button, characterCost);
    }
}
