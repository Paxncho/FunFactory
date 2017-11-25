using System.Collections;
using System.Collections.Generic;

using System.Xml.Serialization;

using UnityEngine;


public class Materials : MonoBehaviour {

    public int MaterialSize;

    public GameMaterial[] materials;
    public GameObject UIPrefab;
    public GameObject UIContenedor;

    List<GameObject> Contenedores = new List<GameObject>();

	// Use this for initialization
	void Start () {
        MaterialSize = 10;
        Randomize();
        Save();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Randomize() {
        materials = new GameMaterial[MaterialSize];

        for (int i = 0; i < MaterialSize; i++) {
            materials[i] = new GameMaterial();

            materials[i].id = "ID00" + i;
            materials[i].name = "LOREM IPSUM " + Random.Range(0, 1000);
            //materials[i].Costo = Random.Range(0, 1000);
            materials[i].color = Random.ColorHSV();
            materials[i].sprite = SpritePool.RandomSprite();
        }

        //ToUI();
    }

    public void Save() {
        Data mdl = new Data();

        mdl.materials = new GameMaterial.MaterialData[materials.Length];

        for (int i = 0; i < materials.Length; i++) {
            //mdl.materials[i] = materials[i].ToData();
        }

        DataManager.XMLMarshalling("Assets/Data/materialData.xml", mdl);
    }

    public void Load() {
        Data mdl = DataManager.XMLUnmarshalling<Data>("Assets/Data/materialData.xml");

        materials = new GameMaterial[mdl.materials.Length];

        for (int i = 0; i < mdl.materials.Length; i++) {
            materials[i] = new GameMaterial();
            //materials[i].FromData(mdl.materials[i]);
        }

        ToUI();
    }

    public void ClearUI() {
        for(int i = 0; i < Contenedores.Count; i++) {
            Destroy(Contenedores[i].gameObject);
        }

        Contenedores.Clear();
    }

    public void ToUI() {
        int j, k;

        for (int i = 0; i < materials.Length; i++) {
            j = i % 3;
            k = i / 3;

            if (i >= Contenedores.Count) {
                GameObject go = Instantiate(UIPrefab, Vector3.zero, Quaternion.identity, UIContenedor.transform);
                RectTransform gort = go.GetComponent<RectTransform>();

                gort.anchoredPosition = new Vector2(-355 + 355 * j, 400 - 355 * k);

                Contenedores.Add(go);
            }

            ShopItemUI mui = Contenedores[i].GetComponent<ShopItemUI>();
            //mui.material = materials[i];
            mui.UpdateUI();
        }
    }

    [XmlRoot]
    public class Data {

        [XmlArray, XmlArrayItem]
        public GameMaterial.MaterialData[] materials;

        public Data() { }
    }
}
