using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviourSingleton<Inventory> {

    //Interface for the "Inventory" items
    public interface IItem {
        string Name  { get; }
        string Code  { get; }
        int Quantity { get; set; }

        Sprite Sprite { get; }
        Color Color   { get; }
    }


        //Attributes of the Inventory.

    public int Money { get; set; }

    //Dictionaries to store all the items that the user can have.
    Dictionary<string, IItem> materials = new Dictionary<string, IItem>();
    Dictionary<string, IItem> pieces = new Dictionary<string, IItem>();
    Dictionary<string, IItem> workers = new Dictionary<string, IItem>();
    Dictionary<string, IItem> toys = new Dictionary<string, IItem>();


        //Private methods for internal (or common) operations

   void Start() {
        Money = 10000;
        UIManager.Instance.UpdateGUI();
    }

    //Deactive all the items in the GUI
    void DeActiveGUI() {

        //Deactivate all the actual items
        Transform items = UIManager.Instance.inventoryUI.itemsParents;
        for (int i = 0; i < items.childCount; i++) {
            items.GetChild(i).gameObject.SetActive(false);
        }

        //Deactivate all the actual workers
        Transform workers = UIManager.Instance.inventoryUI.workersParent;
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
        InventoryItemUI itemUI = go.GetComponent<InventoryItemUI>();
        itemUI.item = item;
        itemUI.UpdateUI();
    }


    //Public methods to connect with other classes

    //Add an item to the inventory
    public void Add(IItem item, Type type) {
        switch (type) {
            case Type.Material:
                if (materials.ContainsKey(item.Code)) {
                    materials[item.Code].Quantity += item.Quantity;
                } else {
                    materials.Add(item.Code, item);
                }
                break;

            case Type.Piece:
                if (pieces.ContainsKey(item.Code)) {
                    pieces[item.Code].Quantity += item.Quantity;
                } else {
                    pieces.Add(item.Code, item);
                }
                break;

            case Type.Toy:
                if (toys.ContainsKey(item.Code)) {
                    toys[item.Code].Quantity += item.Quantity;
                } else {
                    toys.Add(item.Code, item);
                }
                break;

            case Type.Worker:
                if (workers.ContainsKey(item.Code)) {
                    workers[item.Code].Quantity += item.Quantity;
                } else {
                    workers.Add(item.Code, item);
                }
                break;
        }

        UpdateGUI(type);
    }

    //Update all the UI of the
    public void UpdateGUI(Type type) {

        //Clear the UI first
        DeActiveGUI();

        //Add the Materials or Pieces depending of what choose the user
        GameObject prefab = UIManager.Instance.inventoryUI.itemsPrefab;
        Transform parent = UIManager.Instance.inventoryUI.itemsParents;

        if (type == Type.Piece) {
            foreach(string code in pieces.Keys) {
                IItem item = pieces[code];
                UpdateGUI(item, prefab, parent);
            }
        } else {
            foreach(string code in materials.Keys) {
                IItem item = materials[code];
                UpdateGUI(item, prefab, parent);
            }
        }

        //Add the workers
        prefab = UIManager.Instance.inventoryUI.workersPrefab;
        parent = UIManager.Instance.inventoryUI.workersParent;

        foreach (string code in workers.Keys) {
            IItem worker = workers[code];
            UpdateGUI(worker, prefab, parent);
        }

        UIManager.Instance.UpdateGUI();
    }

    // TODO
    // void LoadInventory();
    // void SaveInventory();

    //Possible Types of Items
    public enum Type { Material, Piece, Worker, Toy }
}
