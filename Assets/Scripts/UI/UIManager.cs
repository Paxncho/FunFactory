using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager> {

    public Text moneyText;
    public Text repText;

    public WorkerUI workerStation;

    public ShopUI shopUI;
    
    public void UpdateGUI() {

    }

        //Structs to organize the Inspector and have a better managment of the UI

    [System.Serializable]
    public struct WorkerUI {
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

    [System.Serializable]
    public struct ShopUI {
        public Transform materialsParent;
        public GameObject materialsPrefab;

        public Transform workersParent;
        public GameObject workersPrefab;
    }
}
