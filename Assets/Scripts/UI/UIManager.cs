using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager> {

    public Text moneyText;
    public Text repText;

    public ShopUI shopUI;
    public ShopPanels shopPanel;
    public InventoryUI inventoryUI;
    public InventoryPanels inventoryPanel;

    public WorkerUI workerStation;

    public void UpdateGUI() {
        moneyText.text = Inventory.Instance.Money.ToString();
    }

        //Inventory Methods
    public void ToInventoryMaterials() {
        inventoryPanel.layout.SetActive(true);
        inventoryPanel.title.text = inventoryPanel.materialsTitle;
        inventoryPanel.workers.SetActive(false);
        inventoryPanel.items.SetActive(true);
        inventoryPanel.buyMaterial.SetActive(true);
        inventoryPanel.hireWorker.SetActive(false);
        Inventory.Instance.UpdateGUI(Inventory.Type.Material);
    }
    public void ToInventoryPieces() {
        inventoryPanel.layout.SetActive(true);
        inventoryPanel.title.text = inventoryPanel.piecesTitle;
        inventoryPanel.workers.SetActive(false);
        inventoryPanel.items.SetActive(true);
        inventoryPanel.buyMaterial.SetActive(false);
        inventoryPanel.hireWorker.SetActive(false);
        Inventory.Instance.UpdateGUI(Inventory.Type.Piece);
    }
    public void ToInventoryWorkers() {
        inventoryPanel.layout.SetActive(true);
        inventoryPanel.title.text = inventoryPanel.workersTitle;
        inventoryPanel.workers.SetActive(true);
        inventoryPanel.items.SetActive(false);
        inventoryPanel.buyMaterial.SetActive(false);
        inventoryPanel.hireWorker.SetActive(true);
    }
    public void BackFromInventory() {
        inventoryPanel.layout.SetActive(false);
    }

        //Shop Methods
    public void ToShopMaterials() {
        shopPanel.layout.SetActive(true);
        shopPanel.title.text = shopPanel.materialsTitle;
        shopPanel.materials.SetActive(true);
        shopPanel.workers.SetActive(false);
    }
    public void ToShopWorkers() {
        shopPanel.layout.SetActive(true);
        shopPanel.title.text = shopPanel.workersTitle;
        shopPanel.materials.SetActive(false);
        shopPanel.workers.SetActive(true);
    }
    public void BackFromShop() {
        shopPanel.layout.SetActive(false);
    }

        //Structs to organize the Inspector and have a better management of the UI

    [System.Serializable] public struct WorkerUI {
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
    [System.Serializable] public struct ShopUI {
        public Transform materialsParent;
        public GameObject materialsPrefab;

        public Transform workersParent;
        public GameObject workersPrefab;
    }
    [System.Serializable] public struct ShopPanels {
        public GameObject layout;
        public Text title;
        public string materialsTitle;
        public string workersTitle;
        public GameObject materials;
        public GameObject workers;
    }
    [System.Serializable] public struct InventoryUI {
        public Transform itemsParents;
        public GameObject itemsPrefab;

        public Transform workersParent;
        public GameObject workersPrefab;
    }
    [System.Serializable] public struct InventoryPanels {
        public GameObject layout;
        public Text title;
        public string piecesTitle;
        public string materialsTitle;
        public string workersTitle;
        public GameObject items;
        public GameObject buyMaterial;
        public GameObject workers;
        public GameObject hireWorker;
    }
}
