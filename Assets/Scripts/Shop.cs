﻿using System.Collections.Generic;
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
    public Dictionary<string, IItem> materials = new Dictionary<string, IItem>();
    public Dictionary<string, IItem> workers = new Dictionary<string, IItem>();

    //XML which will be used to load the items.
    public string MaterialsToSellPath = "Assets/Data/materialsToSell.xml";
    public string WorkersToHirePath = "Assets/Data/workersToHire.xml";


        //Private Methods for internal (or common) operations;

    void Start() {
        //DataManager.XMLMarshalling(MaterialsToSellPath, GameMaterial.ExampleData());
        Load();
    }

    bool Buy(IItem item, int quantity) {
        int totalPrice = item.Price * quantity;
        int money = Inventory.Instance.Money;

        //Check if there is enough money
        if (totalPrice <= money) {
            Inventory.IItem inventoryItem = item.InventoryItem;
            inventoryItem.Quantity = quantity;

            Inventory.Instance.Money -= totalPrice;
            Inventory.Instance.Add(inventoryItem, item.Type);
            return true;
        }

        return false;
    }

    //TODO
    //ACTUALLY SAVE THE QUANTITY LEFT OF "ITEMS"
    void SaveMaterialsToSell() {
        DataManager.XMLMarshalling(MaterialsToSellPath, GameMaterial.ExampleData());
    }

    //Load all the shop items;
    void Load() {
        LoadMaterialsToSell();
        LoadWorkersToHire();
        UpdateGUI();
    }

    //Load Materials
    void LoadMaterialsToSell() {
        GameMaterial.MaterialDataList mdl = DataManager.XMLUnmarshalling<GameMaterial.MaterialDataList>(MaterialsToSellPath);
        IItem[] items = new IItem[mdl.materials.Length];

        for (int i = 0; i < items.Length; i++) {
            //Add the materials to the Dictionary
            items[i] = GameMaterial.FromDataToShop(mdl.materials[i]);
            materials.Add(items[i].Code, items[i]);
        }
    }

    //Load Workers
    void LoadWorkersToHire() {
        Worker.WorkerDataList wdl = DataManager.XMLUnmarshalling<Worker.WorkerDataList>(WorkersToHirePath);
        IItem[] items = new IItem[wdl.workers.Length];
        
        for (int i = 0; i < items.Length; i++) {
            //Add the workers to the Dictionary
            items[i] = Worker.FromDataToShop(wdl.workers[i]);
            workers.Add(items[i].Code, items[i]);
        }
    }

    //Deactive all the items in the GUI
    void DeActiveGUI() {

        //Deactivate all the actual items
        Transform materials = UIManager.Instance.shopUI.materialsParent;
        for (int i = 0; i < materials.childCount; i++) {
            materials.GetChild(i).gameObject.SetActive(false);
        }

        //Deactive all the workers
        Transform workers = UIManager.Instance.shopUI.workersParent;
        for (int i = 0; i < workers.childCount; i++) {
            workers.GetChild(i).gameObject.SetActive(false);
        }
    }

    //Put a new item in the UI
    void UpdateGUI(IItem item, GameObject prefab, Transform parent) {
        
        //Instantiate the gui item
        GameObject go = ObjectPool.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, parent);

        //Fix the scale error
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.transform.localScale = Vector3.one;

        //Load the data in the UI
        ShopItemUI itemUI = go.GetComponent<ShopItemUI>();
        itemUI.item = item;
        itemUI.UpdateUI();
    }


        //Public methods to connect with buttons and other classes

    public bool BuyMaterial(string codeItem, int quantity) {
        IItem item = materials[codeItem];
        return Buy(item, quantity);
    }

    public bool HireWorker(string codeWorker) {
        IItem item = workers[codeWorker];
        workers.Remove(codeWorker);
        UpdateGUI();
        return Buy(item, 1);
    }

    //Update all the UI
    public void UpdateGUI() {

        //Deactive the items in order to clear the gui first
        DeActiveGUI();

        //Add the Materials
        GameObject prefab = UIManager.Instance.shopUI.materialsPrefab;
        Transform parent = UIManager.Instance.shopUI.materialsParent;

        foreach (string code in materials.Keys) {
            IItem item = materials[code];
            UpdateGUI(item, prefab, parent);
        }

        //Add the Workers
        prefab = UIManager.Instance.shopUI.workersPrefab;
        parent = UIManager.Instance.shopUI.workersParent;

        foreach (string code in workers.Keys) {
            IItem worker = workers[code];
            UpdateGUI(worker, prefab, parent);
        }
    }
}
