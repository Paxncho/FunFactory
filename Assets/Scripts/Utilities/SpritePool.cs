using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;

public class SpritePool : MonoBehaviourSingleton<SpritePool> {

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

    Sprite RandomTexture() {
        int total = pool.Count;
        int toGet = Random.Range(0, total);

        int i = 0;

        foreach (Sprite sprite in pool.Values) {
            if (i == toGet) {
                return sprite;
            }
            i++;
        }

        return null;
    }

    string FindId(Sprite sprite) {
        foreach (string path in pool.Keys) {
            Sprite s = pool[path];

            if (s == sprite)
                return path;
        }

        return "";
    }

    public static Sprite LoadSprite(string key) {
        return Instance.pool[key];
    }

    public static Sprite RandomSprite() {
        return Instance.RandomTexture();
    }

    public static string GetId(Sprite sprite) {
        return Instance.FindId(sprite);
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
[System.Serializable] public class TextureData {
    [XmlAttribute] public string id;
    [XmlAttribute] public string path;
    [XmlAttribute] public string name;
    [XmlAttribute] public string description;
    public TextureData() { }
}
[XmlRoot] public class TextureDataList {
    [XmlArray, XmlArrayItem] public TextureData[] textures;
    public TextureDataList() { }
}