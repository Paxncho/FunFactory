using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;

public class DataManager : MonoBehaviour {

    [Serializable]
    public class DataItem {
        [XmlAttribute]
        public string Text;

        [XmlAttribute]
        public int Number;

        [XmlArray, XmlArrayItem]
        public List<int> Numeros = new List<int>();

        public DataItem() {

        }

        public DataItem(string text, int number) {
            Text = text;
            Number = number;
            Numeros.Add(1);
            Numeros.Add(3);
            Numeros.Add(7);
            Numeros.Add(9);
        }

        public override string ToString() {
            string s = "DataItem: \n";
            s += "Text: " + Text + "\n";
            s += "Number: " + Number + "\n";
            s += "Numeros: ";
            for (int i = 0; i < Numeros.Count; i++) {
                s += Numeros[i] + ", ";
            }

            return s;
        }
    }

    [XmlRoot]
    public class DataContainer {

        [XmlArray, XmlArrayItem]
        public List<DataItem> Items = new List<DataItem>();

        public DataContainer() {

        }

        public void Randomize(int elementNumber) {
            Items.Clear();

            for(int i = 0; i < elementNumber; i++) {
                DataItem item = new DataItem();
                item.Text = UnityEngine.Random.ColorHSV().ToString();
                item.Number = UnityEngine.Random.Range(0, 10);

                item.Numeros.Clear();

                for (int j = 0; j < item.Number; j++) {
                    item.Numeros.Add(UnityEngine.Random.Range(0, 100));
                }

                Items.Add(item);
            }
        }

        public override string ToString() {
            string s = "DataContainer: ";
            for (int i = 0; i < Items.Count; i++) {
                s += Items[i].ToString() + "\n";
            }

            return s;
        }
    }

    public DataItem item;

    public DataContainer items = new DataContainer();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void XMLMarshalling(string path, object item) {
        using (FileStream fs = new FileStream(path, FileMode.Create)) {
            XmlSerializer xml = new XmlSerializer(item.GetType());
            xml.Serialize(fs, item);
        }
    }

    public static T XMLUnmarshalling<T>(string path) {
        using (FileStream fs = new FileStream(path, FileMode.Open)) {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            return (T)xml.Deserialize(fs);
        }
    }


    public void ToJson() {
        string json = JsonUtility.ToJson(item);

        using (StreamWriter sw = new StreamWriter("Assets/Data/file.json")) {
            sw.Write(json);
            Debug.Log("Json writed: " + json);
        }
    }

    public void FromJson() {
        using (StreamReader sr = new StreamReader("Assets/Data/file.json")) {
            string json = sr.ReadToEnd();
            item = JsonUtility.FromJson<DataItem>(json);

            Debug.Log("Json loaded: " + json);
        }
    }

    public void Randomize() {
        items.Randomize(5);
        Debug.Log("RANDOM");
    }

    public void ToXML() {
        XMLMarshalling("Assets/Data/file.xml", items);
        Debug.Log("XML writed:");
    }

    public void FromXML() {
        items = XMLUnmarshalling<DataContainer>("Assets/Data/file.xml");
        Debug.Log("XML readed:");
        Debug.Log(items.ToString());
    }

    
}
