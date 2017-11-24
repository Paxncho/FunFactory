using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour {

    public Shop.IItem item;

    public Text Description;
    public Image Image;

    //TODO
    //ENABLE BUY ITEMS

    public void UpdateUI() {
        string description = item.Name + "\n$" + item.Price;

        Description.text = description;
        Image.sprite = item.Sprite;
        Image.color = item.Color;
    }

    public void Buy() {
        if (item.Type == Inventory.Type.Material) {
            Shop.Instance.BuyMaterial(item.Code, 1);
        } else if (item.Type == Inventory.Type.Worker) {
            Shop.Instance.HireWorker(item.Code);
        }
    }
}
