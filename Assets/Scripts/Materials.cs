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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Randomize() {
        materials = new GameMaterial[MaterialSize];

        for (int i = 0; i < MaterialSize; i++) {
            materials[i] = new GameMaterial();

            materials[i].Id = "ID00" + i;
            materials[i].Descripcion = "LOREM IPSUM " + Random.Range(0, 1000);
            materials[i].Costo = Random.Range(0, 1000);
            materials[i].MaterialNecesario = "S0, " + Random.Range(0, 1000);
            materials[i].color = Random.ColorHSV();
        }

        ToUI();
    }

    public void Save() {
        MaterialDataList mdl = new MaterialDataList();
        mdl.materials = this.materials;

        DataManager.XMLMarshalling("Assets/Data/materialData.xml", mdl);
    }

    public void Load() {
        MaterialDataList mdl = DataManager.XMLUnmarshalling<MaterialDataList>("Assets/Data/materialData.xml");
        materials = mdl.materials;

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

            MaterialUI mui = Contenedores[i].GetComponent<MaterialUI>();
            mui.material = materials[i];
            mui.UpdateUI();
        }
    }

    [XmlRoot]
    public class MaterialDataList {

        [XmlArray, XmlArrayItem]
        public GameMaterial[] materials;

        public MaterialDataList() {

        }
    }
}
