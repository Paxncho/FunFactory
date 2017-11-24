using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviourSingleton<Shop> {

    //Interface for the "Shop" items
    public interface IItem {
        string Name { get; }
        string Code { get; }
        int Price   { get; }

        Sprite Sprite { get; }
        Color Color   { get; }

        Inventory.Type Type { get; }

        //"To Inventory"
        Inventory.IItem InventoryItem { get; }
    }

        //Attributes of the Shop.

    //Dictionaries to store the items to sell.
    public Dictionary<string, IItem> materials;
    public Dictionary<string, IItem> workers;

    //XML which will be used to load the items.
    public string MaterialsToSellPath = "Assets/Data/materialData.xml";


        //Private Methods for internal (or common) operations;

    //Initialize the Dictionaries
    void Awake() {
        materials = new Dictionary<string, IItem>();
        workers = new Dictionary<string, IItem>();
    }

    void Start() {
        LoadSellingMaterials();
    }

    bool Buy(IItem item, int quantity) {
        int totalPrice = item.Price * quantity;
        int money = Inventory.Instance.Money;

        //Check if there is enough money
        if (totalPrice <= money) {
            Inventory.Instance.Money -= totalPrice;
            Inventory.Instance.Add(item.InventoryItem, quantity, item.Type);
            return true;
        }

        return false;
    }

    void SaveSellingMaterials() {
        DataManager.XMLMarshalling(MaterialsToSellPath, GameMaterial.ExampleData());
    }

    void LoadSellingMaterials() {
        GameMaterial.DataList mdl = DataManager.XMLUnmarshalling<GameMaterial.DataList>(MaterialsToSellPath);
        IItem[] items = new IItem[mdl.materials.Length];

        GameObject prefab = UIManager.Instance.shopUI.materialsPrefab;
        Transform parent = UIManager.Instance.shopUI.materialsParent;

        for (int i = 0; i < items.Length; i++) {
            
            //Add the materials to the Dictionary
            items[i] = GameMaterial.FromDataToShop(mdl.materials[i]);
            materials.Add(items[i].Code, items[i]);

            //Put them in the UI
            GameObject go = ObjectPool.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, parent);

            //Fix the instantiate size error
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.transform.localScale = Vector3.one;

            //Load the data in the UI
            ShopItemUI mui = go.GetComponent<ShopItemUI>();
            mui.item = items[i];
            mui.UpdateUI();
        }
    }

        //Public methods to connect with buttons

    public bool BuyMaterial(string codeItem, int quantity) {
        IItem item = materials[codeItem];
        return Buy(item, quantity);
    }

    public bool HireWorker(string codeWorker) {
        IItem item = workers[codeWorker];
        return Buy(item, 1);
    }
}
