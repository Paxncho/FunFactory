using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WorkerUI {
    //WorkerData
    public Transform Parent;
    public Text[] stats;
    public Text[] statsPercentages;
    public Slider[] statsBar;

    public Image WorkTable;
    public Image Worker;

    public Image baseMaterial;
    public Image finalMaterial;

    public Text timeLeft;
}

public class UIManager : MonoBehaviour {

    public Text moneyText;
    public Text repText;

    public WorkerUI workerStation;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateGUI() {

    }
}
