using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

using UnityEngine;
using UnityEngine.UI;

public class GameMaterial : IInventoryItem {

    public string Id;
    public string Descripcion;
    public int Costo;
    public string MaterialNecesario;
    public Sprite sprite;
    public Color color;

    int quantity = 0;

    public GameMaterial() {
        
    }

    //Inventory Item properties
    int IInventoryItem.Quantity  { get { return quantity; }    set { quantity = value; } }
    int IInventoryItem.Price     { get { return Costo; }       set { Costo = value; } }
    string IInventoryItem.Code   { get { return Id; }          set { Id = value; } }
    string IInventoryItem.Name   { get { return Descripcion; } set { Descripcion = value; } }
    Sprite IInventoryItem.Sprite { get { return sprite; } }
    Color IInventoryItem.Color   { get { return color; } }

    public DataMaterial ToData() {
        DataMaterial data = new DataMaterial();

        data.Id = Id;
        data.Descripcion = Descripcion;
        data.Costo = Costo;
        data.MaterialNecesario = MaterialNecesario;
        data.SpritePath = sprite.name;
        data.R = color.r;
        data.G = color.g;
        data.B = color.b;
        data.A = color.a;

        return data;
    }

    public void FromData(DataMaterial data) {
        Id = data.Id;
        Descripcion = data.Descripcion;
        Costo = data.Costo;
        MaterialNecesario = data.MaterialNecesario;
        sprite = TexturePool.LoadTexture(data.SpritePath);
        Color color = new Color(data.R, data.G, data.B, data.A);
    }

    [Serializable]
    public class DataMaterial {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string Descripcion;
        [XmlAttribute] public int Costo;
        [XmlAttribute] public string MaterialNecesario;
        [XmlAttribute] public string SpritePath;
        [XmlAttribute] public float R;
        [XmlAttribute] public float G;
        [XmlAttribute] public float B;
        [XmlAttribute] public float A;

        public DataMaterial() {

        }
    }
}
