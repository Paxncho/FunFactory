using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour {

    public Inventory.IItem item;

    public Image icon;
    public Text nameText;
    public Text quantityText;

    public virtual void UpdateUI() {
        icon.sprite = item.Sprite;
        icon.color = item.Color;
        nameText.text = item.Name;

        if (quantityText != null)
            quantityText.text = item.Quantity.ToString();
    }
}
