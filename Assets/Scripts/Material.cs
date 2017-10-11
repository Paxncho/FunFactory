using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

using UnityEngine;

public class Material {

    public string Id;
    public string Descripcion;
    public int Costo;
    public string MaterialNecesario;
    public Color color;

    public Material() {

    }

    public DataMaterial ToData() {
        DataMaterial data = new DataMaterial();

        data.Id = Id;
        data.Descripcion = Descripcion;
        data.Costo = Costo;
        data.MaterialNecesario = MaterialNecesario;
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
        color = new Color(data.R, data.G, data.B, data.A);
    }

    [Serializable]
    public class DataMaterial {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string Descripcion;
        [XmlAttribute] public int Costo;
        [XmlAttribute] public string MaterialNecesario;
        [XmlAttribute] public float R;
        [XmlAttribute] public float G;
        [XmlAttribute] public float B;
        [XmlAttribute] public float A;

        public DataMaterial() {

        }
    }
}
