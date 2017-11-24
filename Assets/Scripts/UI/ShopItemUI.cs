using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour {

    public Shop.IItem item;

    public Text description;
    public Image icon;
    public Text quantityText;

    int quantity = 0;

    public virtual void UpdateUI() {
        string description = item.Name + "\n$" + item.Price;

        this.description.text = description;
        icon.sprite = item.Sprite;
        icon.color = item.Color;
    }

    //Buy a Material, not Hire a Worker
    public virtual void Buy() {
        bool test = Shop.Instance.BuyMaterial(item.Code, quantity);

        quantity = 0;
        quantityText.text = quantity.ToString();
    }

    public void ChangeQuantity(int value) {
        int tempQuantity = quantity + value;

        if (tempQuantity >= 0) {
            quantity = tempQuantity;
        }

        quantityText.text = quantity.ToString();
    }
}
