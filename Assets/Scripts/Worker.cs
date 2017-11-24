using System.Xml.Serialization;

using UnityEngine;

//BaseClass
public class Worker {

        //Attributes of the class
    public string id;
    public string name;
    public Sprite sprite;
    public Color color;

        //Characteristics of the workers
    public float tired;
    public int talent;
    public float motivation;

    public Worker() { }

        //Public methods of the class

    //TODO ToData()

        //Static Methods to create a Worker from a specific data.
    static Worker ReadData(WorkerData data) {
        return new Worker() {
            id = data.Id,
            name = data.Name,
            sprite = SpritePool.LoadSprite(data.SpriteId),
            color = new Color(data.R, data.G, data.B, data.A),
            tired = data.Tired,
            talent = data.Talent,
            motivation = data.Motivation
        };
    }
    public static Shop.IItem FromDataToShop(WorkerData data) {
        return new OfferingWorker() {
            worker = ReadData(data),
            price = data.Price
        };
    }
    public static Inventory.IItem FromDataToInventory(WorkerData data) {
        return new HiredWorker() {
            worker = ReadData(data),
            quantity = data.Quantity
        };
    }

        //Static Method to create an ExampleXML
    public static WorkerDataList ExampleData() {
        WorkerDataList dl = new WorkerDataList();

        WorkerData d1 = new WorkerData() {
            Id = "d1",
            Name = "Worker1",
            Price = 8000,
            Quantity = 1,
            SpriteId = SpritePool.GetId(SpritePool.RandomSprite()),
            Tired = 0,
            Talent = 50,
            Motivation = 0.4f,
            R = 1,
            G = 1,
            B = 1,
            A = 1
        };

        WorkerData d2 = new WorkerData() {
            Id = "d2",
            Name = "DevilWorker",
            Price = 666,
            Quantity = 6,
            SpriteId = SpritePool.GetId(SpritePool.RandomSprite()),
            Tired = 0.2f,
            Talent = 20,
            Motivation = 0.1f,
            R = 1,
            G = 0,
            B = 0,
            A = 1
        };

        dl.workers = new WorkerData[2];
        dl.workers[0] = d1;
        dl.workers[1] = d2;

        return dl;
    }

        //Structs to connect with the UI and the other classes
    public struct OfferingWorker : Shop.IItem {
        public Worker worker;
        public int price;

        //Selling Item Properties
        string Shop.IItem.Name   { get { return worker.name; } }
        string Shop.IItem.Code   { get { return worker.id; } }
        int    Shop.IItem.Price  { get { return price; } }
        Sprite Shop.IItem.Sprite { get { return worker.sprite; } }
        Color  Shop.IItem.Color  { get { return worker.color; } }

        Inventory.Type  Shop.IItem.Type { get { return Inventory.Type.Worker; } }
        Inventory.IItem Shop.IItem.InventoryItem {
            get {
                return new HiredWorker() {
                    worker = worker,
                    quantity = 0
                };
            }
        }
    }
    public struct HiredWorker : Inventory.IItem {
        public Worker worker;
        public int quantity;

        //Inventory Items Properties;
        string Inventory.IItem.Name     { get { return worker.name; } }
        string Inventory.IItem.Code     { get { return worker.id; } }
        Sprite Inventory.IItem.Sprite   { get { return worker.sprite; } }
        Color  Inventory.IItem.Color    { get { return worker.color; } }
        int    Inventory.IItem.Quantity { get { return quantity; } set { quantity = value; } }
    }

        //Classes to Save and Load data
    [System.Serializable] public class WorkerData {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string Name;
        [XmlAttribute] public int Price;
        [XmlAttribute] public int Quantity;
        [XmlAttribute] public string SpriteId;
        [XmlAttribute] public float Tired;
        [XmlAttribute] public int Talent;
        [XmlAttribute] public float Motivation;
        [XmlAttribute] public float R;
        [XmlAttribute] public float G;
        [XmlAttribute] public float B;
        [XmlAttribute] public float A;
    }
    [XmlRoot] public class WorkerDataList {
        [XmlArray, XmlArrayItem] public WorkerData[] workers;
        public WorkerDataList() { }
    }
}