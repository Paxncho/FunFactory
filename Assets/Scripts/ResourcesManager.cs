using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInventoryItem {
    int Quantity { get; set; }
    int Price { get; set; }

    string Code { get; set; }
    string Name { get; set; }
    Sprite Sprite { get; }
    Color Color { get; }
}

public class ResourcesManager : MonoBehaviourSingleton<ResourcesManager> {

    public int money;
    float sellItemPercentaje;

    Dictionary<string, IInventoryItem> materials = new Dictionary<string, IInventoryItem>();
    Dictionary<string, IInventoryItem> pieces = new Dictionary<string, IInventoryItem>();
    Dictionary<string, IInventoryItem> workers = new Dictionary<string, IInventoryItem>();
    Dictionary<string, IInventoryItem> toys = new Dictionary<string, IInventoryItem>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool BuyMaterial(string materialCode, int quantity) {
        IInventoryItem gameMaterial = materials[materialCode];
        int totalPrice = gameMaterial.Price * quantity;

        if (totalPrice <= money) {
            money -= totalPrice;
            gameMaterial.Quantity += quantity;
            return true;
        }

        return false;
    }

    public void SellMaterial(string materialCode, int quantity) {
        IInventoryItem gameMaterial = materials[materialCode];

        if (quantity > gameMaterial.Quantity) {
            quantity = gameMaterial.Quantity;
        }

        int moneyToGain = (int) (gameMaterial.Price * quantity * sellItemPercentaje);

        money += moneyToGain;
    }
}
