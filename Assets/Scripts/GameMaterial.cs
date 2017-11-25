using System.Xml.Serialization;

using UnityEngine;

//Base Class
public class GameMaterial {
        
        //Attributes of the class
    public string id;
    public string name;
    public Sprite sprite;
    public Color color;

    public GameMaterial() { }

        //Static methods to create a GameMaterial from a specific data.
    static GameMaterial ReadData(MaterialData data) {
        return new GameMaterial() {
            id = data.Id,
            name = data.Name,
            sprite = SpritePool.LoadSprite(data.SpriteId),
            color = new Color(data.R, data.G, data.B, data.A)
        };
    }
    public static Shop.IItem FromDataToShop (MaterialData data) {
        return new SellingItem() {
            material = ReadData(data),
            price = data.Price
        };
    }
    public static Inventory.IItem FromDataToInventory (MaterialData data) {
        return new InventoryItem() {
            material = ReadData(data),
            quantity = data.Quantity
        };
    }

        //Static Method to create an ExampleXML
    public static MaterialDataList ExampleData() {
        MaterialDataList dl = new MaterialDataList();

        MaterialData d1 = new MaterialData() {
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

        MaterialData d2 = new MaterialData() {
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

        dl.materials = new MaterialData[2];
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
                return new InventoryItem {
                    material = material,
                    quantity = 0
                };
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

        DataManager.IData Inventory.IItem.Data {
            get {
                return new MaterialData() {
                    Id = material.id,
                    Name = material.name,
                    Price = -1,
                    Quantity = quantity,
                    SpriteId = SpritePool.GetId(material.sprite),
                    R = material.color.r,
                    G = material.color.g,
                    B = material.color.b,
                    A = material.color.a
                };
            }
        }
    }

        //Classes to Save and Load data
    [System.Serializable] public class MaterialData : DataManager.IData {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string Name;
        [XmlAttribute] public int Price;
        [XmlAttribute] public int Quantity;
        [XmlAttribute] public string SpriteId;
        [XmlAttribute] public float R;
        [XmlAttribute] public float G;
        [XmlAttribute] public float B;
        [XmlAttribute] public float A;
        public MaterialData() { }
    }
    [XmlRoot] public class MaterialDataList {
        [XmlArray, XmlArrayItem] public MaterialData[] materials;
        public MaterialDataList() { }
    }
}
