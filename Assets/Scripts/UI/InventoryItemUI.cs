using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryItemUI : MonoBehaviour {

    public Inventory.IItem item;

    public Image icon;
    public Text nameText;
    public Text quantityText;
    public Button button;

    public virtual void UpdateUI() {
        if (item != null) {
            icon.sprite = item.Sprite;
            icon.color = item.Color;
        }

        if (nameText != null)
            nameText.text = item.Name;

        if (quantityText != null)
            quantityText.text = item.Quantity.ToString();
    }

    public virtual void AddListener(UnityAction call) {
        button.onClick.AddListener(delegate { call(); } );
    }

    public virtual void AddListener<T0>(UnityAction<T0> call, T0 parameter) {
        button.onClick.AddListener(delegate { call(parameter); });
    }

    public virtual void AddListener<T0, T1>(UnityAction<T0, T1> call, T0 parameter1, T1 parameter2) {
        button.onClick.AddListener(delegate { call(parameter1, parameter2); });
    }

    public void ResetListeners() {
        button.onClick.RemoveAllListeners();
    }
}
