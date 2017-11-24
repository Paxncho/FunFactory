using System;
using System.Xml.Serialization;

using UnityEngine;

//Base Class
public class GameMaterial {

    public string id;
    public string name;
    public Sprite sprite;
    public Color color;

    public GameMaterial() { }

        //Public methods of the class

    public Data ToData() {
        Data data = new Data();

        data.Id = id;
        data.Name = name;
        data.SpriteId = SpritePool.GetId(sprite);
        data.R = color.r;
        data.G = color.g;
        data.B = color.b;
        data.A = color.a;

        return data;
    }

        //Static methods to create a GameMaterial from a specific data.

    static GameMaterial ReadData(Data data) {
        return new GameMaterial() {
            id = data.Id,
            name = data.Name,
            sprite = SpritePool.LoadSprite(data.SpriteId),
            color = new Color(data.R, data.G, data.B, data.A)
        };
    }

    public static Shop.IItem FromDataToShop (Data data) {
        return new SellingItem() {
            material = ReadData(data),
            price = data.Price
        };
    }

    public static Inventory.IItem FromDataToInventory (Data data) {
        return new InventoryItem() {
            material = ReadData(data),
            quantity = data.Quantity
        };
    }

    public static DataList ExampleData() {
        DataList dl = new DataList();

        Data d1 = new Data() {
            Id = "d1",
            Name = "Name1",
            Price = 900,
            Quantity = 4,
            SpriteId = SpritePool.GetId(SpritePool.RandomSprite()),
            R = 1,
            G = 0,
            B = 0,
            A = 1
        };

        Data d2 = new Data() {
            Id = "d2",
            Name = "Name2",
            Price = 666,
            Quantity = 6,
            SpriteId = SpritePool.GetId(SpritePool.RandomSprite()),
            R = 0,
            G = 0,
            B = 1,
            A = 1
        };

        dl.materials = new Data[2];
        dl.materials[0] = d1;
        dl.materials[1] = d2;

        return dl;
    }

        //Structs to connect with the UI and the other classes

    public struct SellingItem : Shop.IItem {
        public GameMaterial material;
        public int price;

        //Selling Item Properties
        string  Shop.IItem.Name   { get { return material.name; } }
        string  Shop.IItem.Code   { get { return material.id; } }
        int     Shop.IItem.Price  { get { return price; } }
        Sprite  Shop.IItem.Sprite { get { return material.sprite; } }
        Color   Shop.IItem.Color  { get { return material.color; } }

        Inventory.Type  Shop.IItem.Type { get { return Inventory.Type.Material; } }
        Inventory.IItem Shop.IItem.InventoryItem {
            get {
                InventoryItem item = new InventoryItem();
                item.material = material;
                item.quantity = 0;
                return item;
            }
        }
    }

    public struct InventoryItem : Inventory.IItem {
        public GameMaterial material;
        public int quantity;

        //Inventory Items Properties;
        string Inventory.IItem.Name     { get { return material.name; } }
        string Inventory.IItem.Code     { get { return material.id; } }
        Sprite Inventory.IItem.Sprite   { get { return material.sprite; } }
        Color  Inventory.IItem.Color    { get { return material.color; } }
        int    Inventory.IItem.Quantity { get { return quantity; } set { quantity = value; } }

    }

        //Classes to Save and Load data

    [Serializable]
    public class Data {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string Name;
        [XmlAttribute] public int Price;
        [XmlAttribute] public int Quantity;
        [XmlAttribute] public string SpriteId;
        [XmlAttribute] public float R;
        [XmlAttribute] public float G;
        [XmlAttribute] public float B;
        [XmlAttribute] public float A;
        public Data() { }
    }

    [XmlRoot]
    public class DataList {
        [XmlArray, XmlArrayItem] public Data[] materials;
        public DataList() { }
    }
}
