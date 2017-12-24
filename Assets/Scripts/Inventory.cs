using System.Collections;

using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Inventory : MonoBehaviourSingleton<Inventory> {

    /*
     * Data to save:
     * all the dictionaries (by id)
     */ 


    //Interface for the "Inventory" items
    public interface IItem {
        string Name  { get; }
        string Code  { get; }
        int Quantity { get; set; }

        Sprite Sprite { get; }
        Color Color   { get; }

        DataManager.IData Data { get; }
    }

    public static string saveFileName = "inventory.fundata";

        //Attributes of the Inventory.

    public int Money { get; set; }

    //Dictionaries to store all the items that the user can have.
    public Dictionary<string, IItem> materials = new Dictionary<string, IItem>();
    public Dictionary<string, IItem> pieces = new Dictionary<string, IItem>();
    public Dictionary<string, IItem> workers = new Dictionary<string, IItem>();
    public List<Toy.ToyData> toys = new List<Toy.ToyData>();

    //References to Ensambler and Factory
    public Ensambler ensambler;
    public Factory factory;

    private float lastTimeSaved;

        //Private methods for internal (or common) operations

   void Start() {
        Money = 10000;
        LoadInventoryAndFactories();
        UIManager.Instance.UpdateMoney();
        lastTimeSaved = Time.time;
    }

    void Update() {
        if (Time.time - lastTimeSaved >= 60) {
            TrySave();
            lastTimeSaved = Time.time;
        }
    }

    InventoryData SaveInventory() {
        InventoryData data = new InventoryData();
        data.Money = Money;

        List<InventoryItemData> items = new List<InventoryItemData>();

        foreach(KeyValuePair<string, IItem> entry in materials) {
            items.Add(new InventoryItemData() {
                id = entry.Value.Code,
                quantity = entry.Value.Quantity,
                type = Type.Material
            });
        }

        foreach (KeyValuePair<string, IItem> entry in pieces) {
            items.Add(new InventoryItemData() {
                id = entry.Value.Code,
                quantity = entry.Value.Quantity,
                type = Type.Piece
            });
        }

        foreach (KeyValuePair<string, IItem> entry in workers) {
            items.Add(new InventoryItemData() {
                id = entry.Value.Code,
                quantity = entry.Value.Quantity,
                type = Type.Worker
            });
        }

        List<Toy.TData> toysData = new List<Toy.TData>();

        foreach (Toy.ToyData toyData in toys) {
            toysData.Add(toyData.ToData());
        }

        data.items = items.ToArray();
        data.toys = toysData.ToArray();
        return data;
    }
    void TrySave() {
        string filePath = Application.persistentDataPath + "/" + saveFileName;
        DataManager.XMLMarshalling(filePath, SaveInventory());

        ensambler.TrySave();
        factory.TrySave();
    }
    void LoadInventory(InventoryData data) {
        Money = data.Money;

        for (int i = 0; i < data.items.Length; i++) {
            switch (data.items[i].type) {
                case Type.Material:
                    IItem material = Shop.Instance.materials[data.items[i].id].InventoryItem;
                    material.Quantity = data.items[i].quantity;
                    materials.Add(material.Code, material);
                    break;

                case Type.Piece:
                    IItem piece = Recipes.Instance.recipes[data.items[i].id].ToInventory();
                    piece.Quantity = data.items[i].quantity;
                    pieces.Add(piece.Code, piece);
                    break;

                case Type.Worker:
                    IItem worker = Shop.Instance.workers[data.items[i].id].InventoryItem;
                    worker.Quantity = data.items[i].quantity;
                    workers.Add(worker.Code, worker);
                    Shop.Instance.workers.Remove(worker.Code);
                    break;
            }
        }

        for (int i = 0; i < data.toys.Length; i++) {
            Toy.ToyData toy = new Toy.ToyData();
            toy.pieces = new Piece[data.toys[i].idPieces.Length];

            for (int j = 0; j < data.toys[i].idPieces.Length; j++) {
                toy.pieces[j] = Recipes.Instance.recipes[data.toys[i].idPieces[j]];
            }

            toys.Add(toy);
            MiniGameManager.Instance.GenerateToy(toy);
        }

        UIManager.Instance.UpdateMoney();
    }
    void LoadInventoryAndFactories() {
        string filePath = Application.persistentDataPath + "/" + saveFileName;

        if (System.IO.File.Exists(filePath)) {
            InventoryData data = DataManager.XMLUnmarshalling<InventoryData>(filePath);
            LoadInventory(data);
        } else {
            //Bear toy: P02 + P01 + P04 + P03
            List<Piece> bearPieces = new List<Piece>();

            bearPieces.Add(Recipes.Instance.recipes["P02"]);
            bearPieces.Add(Recipes.Instance.recipes["P01"]);
            bearPieces.Add(Recipes.Instance.recipes["P04"]);
            bearPieces.Add(Recipes.Instance.recipes["P03"]);

            for (int i = 0; i < 10; i++) {
                Toy.ToyData bear = new Toy.ToyData() { pieces = bearPieces.ToArray() };

                toys.Add(bear);
                MiniGameManager.Instance.GenerateToy(bear);
            }

            IItem piece = Recipes.Instance.recipes["P02"].ToInventory();
            piece.Quantity = 12;
            pieces.Add(piece.Code, piece);

            piece = Recipes.Instance.recipes["P01"].ToInventory();
            piece.Quantity = 12;
            pieces.Add(piece.Code, piece);

            piece = Recipes.Instance.recipes["P04"].ToInventory();
            piece.Quantity = 12;
            pieces.Add(piece.Code, piece);

            piece = Recipes.Instance.recipes["P03"].ToInventory();
            piece.Quantity = 12;
            pieces.Add(piece.Code, piece);
        }

        ensambler.TryLoad();
        factory.TryLoad();

        Shop.Instance.UpdateGUI();
        UpdateGUI(Type.Worker);
        Recipes.Instance.UpdateGUI();
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

    public void Add(Toy.ToyData toy) {
        toys.Add(toy);
    }

    public void Remove(Toy.ToyData toy) {
        toys.Remove(toy);
    }

    public void Remove(string code, int quantity, Type type) {
        switch (type) {
            case Type.Material:
                materials[code].Quantity -= quantity;
                break;
            case Type.Piece:
                pieces[code].Quantity -= quantity;
                break;
            case Type.Worker:
                workers[code].Quantity -= quantity;
                break;
        }
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

        UIManager.Instance.UpdateMoney();
    }

    public Piece GetRandomPiece() {

        if (pieces.Count <= 0)
            return null;

        List<IItem> values = System.Linq.Enumerable.ToList(pieces.Values);
        int size = values.Count;

        return ((Piece.InventoryPiece) values[Random.Range(0, size)]).piece;
        
    }

    void OnApplicationQuit() {
        TrySave();
    }

    //Possible Types of Items
    public enum Type { Material, Piece, Worker, Toy }

    [System.Serializable] public class InventoryItemData : DataManager.IData {
        [XmlAttribute] public string id;
        [XmlAttribute] public int quantity;
        [XmlAttribute] public Type type;
    }
    [XmlRoot] public class InventoryData {
        [XmlAttribute] public int Money;
        [XmlArray, XmlArrayItem] public InventoryItemData[] items;
        [XmlArray, XmlArrayItem] public Toy.TData[] toys;
        public InventoryData() { }
    }
}
