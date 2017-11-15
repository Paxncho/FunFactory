using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;

public class TexturePool : MonoBehaviourSingleton<TexturePool> {

    public string Path = "Assets/Data/images.xml";

    Dictionary<string, Sprite> pool;

	void Start () {
        pool = new Dictionary<string, Sprite>();
        Debug.Log("LOADING");
        Load();
    }

    void Load() {

        TextureDataList data = DataManager.XMLUnmarshalling<TextureDataList>(Path);
        foreach (TextureData tdata in data.textures) {
            string id = tdata.id;
            Sprite texture = Resources.Load<Sprite>(tdata.path);

            pool.Add(id, texture);
            Debug.Log(texture + " LOADED");
        }
    }

    public static Sprite LoadTexture(string key) {
        return Instance.pool[key];
    }

    public void XMLExample() {
        TextureData td1 = new TextureData();
        td1.id = "001";
        td1.path = "Blegh";
        td1.name = "Ble";
        td1.description = "This is a Bleg";

        TextureData td2 = new TextureData();
        td2.id = "td02";
        td2.path = "Dani";
        td2.name = "Daniela";
        td2.description = "This is a Dani";

        TextureDataList tdl = new TextureDataList();
        tdl.textures = new TextureData[2];
        tdl.textures[0] = td1;
        tdl.textures[1] = td2;

        DataManager.XMLMarshalling(Path, tdl);
    }
}


//Auxiliar classes to read the data of the Textures
public class TextureData {
    [XmlAttribute] public string id;
    [XmlAttribute] public string path;
    [XmlAttribute] public string name;
    [XmlAttribute] public string description;
    public TextureData() { }
}

[XmlRoot]
public class TextureDataList {
    [XmlArray, XmlArrayItem] public TextureData[] textures;
    public TextureDataList() { }
}