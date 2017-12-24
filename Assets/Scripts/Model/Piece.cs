using System.Collections.Generic;
using System.Xml.Serialization;

using UnityEngine;

//Base Class
public class Piece {

        //Attributes of the class
    public string id;
    public string name;
    public bool onlyColor;
    public Sprite sprite;
    public Color color;

    public Requirement[] materialsNeeded;
    public int quantityToCreate;
    public int secondsNeeded;

    public Piece() { }

    public Inventory.IItem ToInventory() {
        return new InventoryPiece() {
            piece = this,
            quantity = quantityToCreate
        };
    }

        //Static parse Methods to change from string to Requirement
    static string RequirementsToString(Requirement[] requirements) {
        string line = "";

        for (int i = 0; i < requirements.Length; i++) {
            line += requirements[i].material.id + " " + requirements[i].quantity + ", ";
        }

        return line;
    }
    static Requirement[] StringToRequirements(string line) {
        List<Requirement> list = new List<Requirement>();

        while (true) {
            int commaIndex = line.IndexOf(',');

            if (commaIndex == -1) {
                break;
            }

            int spaceIndex = line.IndexOf(' ');
            string materialCode = line.Substring(0, spaceIndex).Trim();
            string quantityS = line.Substring(spaceIndex, commaIndex - spaceIndex).Trim();
            int quantity = int.Parse(quantityS);
            line = line.Substring(commaIndex + 1).Trim();

            list.Add(new Requirement() {
                material = ((GameMaterial.SellingItem)Shop.Instance.materials[materialCode]).material,
                quantity = quantity
            });
        }

        return list.ToArray();
    }

        //Static Methods to create a Worker from a specific data.
    public static Piece ReadData(PieceData data) {
        return new Piece() {
            id = data.Id,
            name = data.Name,
            onlyColor = data.IsOnlyColor,
            sprite = SpritePool.LoadSprite(data.SpriteId),
            color = new Color(data.R, data.G, data.G, data.A),
            materialsNeeded = StringToRequirements(data.Requirements),
            secondsNeeded = data.SecondsNeeded,
            quantityToCreate = data.QuantityToCreate
        };
    }
    public static Inventory.IItem FromDataToInventory (PieceData data) {
        return new InventoryPiece() {
            piece = ReadData(data),
            quantity = data.Quantity
        };
    }

        //Static method to get an XML Example of the data
    public static PieceDataList ExampleData() {
        PieceDataList pdl = new PieceDataList();
        pdl.pieces = new PieceData[2];
        pdl.pieces[0] = new PieceData() {
            Id = "P01",
            Name = "Piece 01",
            IsOnlyColor = true,
            Quantity = 0,
            SecondsNeeded = 40,
            Requirements = "d5 10, d3 1,",
            QuantityToCreate = 1,
            SpriteId = SpritePool.GetId(SpritePool.RandomSprite()),
            R = 1,
            G = 1,
            B = 1,
            A = 1
        };

        pdl.pieces[1] = new PieceData() {
            Id = "P02",
            Name = "Piece 02",
            IsOnlyColor = false,
            Quantity = 0,
            SecondsNeeded = 20,
            Requirements = "d4 1, d3 1,",
            QuantityToCreate = 5,
            SpriteId = SpritePool.GetId(SpritePool.RandomSprite()),
            R = 1,
            G = 1,
            B = 1,
            A = 1
        };

        return pdl;
    }

        //Structs to connect with the UI and the other classes
    public struct InventoryPiece : Inventory.IItem {
        public Piece piece;
        public int quantity;

        string Inventory.IItem.Name     { get { return piece.name; } }
        string Inventory.IItem.Code     { get { return piece.id; } }
        Sprite Inventory.IItem.Sprite   { get { return piece.sprite; } }
        Color  Inventory.IItem.Color    { get { return piece.color; } }
        int    Inventory.IItem.Quantity { get { return quantity; } set { quantity = value; } }

        DataManager.IData Inventory.IItem.Data {
            get {
                return new PieceData() {
                    Id = piece.id,
                    Name = piece.name,
                    IsOnlyColor = piece.onlyColor,
                    Quantity = quantity,
                    Requirements = RequirementsToString(piece.materialsNeeded),
                    QuantityToCreate = piece.quantityToCreate,
                    SecondsNeeded = piece.secondsNeeded,
                    SpriteId = SpritePool.GetId(piece.sprite),
                    R = piece.color.r,
                    G = piece.color.g,
                    B = piece.color.b,
                    A = piece.color.a
                };
            }
        }
    }

        //Struct for the requirements
    public struct Requirement {
        public GameMaterial material;
        public int quantity;
    }

        //Classes to Save and Load data
    [System.Serializable] public class PieceData : DataManager.IData {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string Name;
        [XmlAttribute] public bool IsOnlyColor;
        [XmlAttribute] public int Quantity;
        [XmlAttribute] public int SecondsNeeded;
        [XmlAttribute] public string Requirements;
        [XmlAttribute] public int QuantityToCreate;
        [XmlAttribute] public string SpriteId;
        [XmlAttribute] public float R;
        [XmlAttribute] public float G;
        [XmlAttribute] public float B;
        [XmlAttribute] public float A;
        public PieceData() { }
    }
    [XmlRoot] public class PieceDataList {
        [XmlArray, XmlArrayItem] public PieceData[] pieces;
        public PieceDataList() { }
    }
}
