using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MaterialUI : MonoBehaviour {

    public GameMaterial material;

    public Text Id;
    public Text Description;
    public Text Costo;
    public Text MaterialesNecesarios;

    public Image color;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateUI() {
        Id.text = "ID: " + material.Id;
        Description.text = "Descripcion: " + material.Descripcion;
        Costo.text = "Costo: " + material.Costo;
        MaterialesNecesarios.text = "Mariales Necesarios: \n" + material.MaterialNecesario;

        color.color = material.color;
    }
}
