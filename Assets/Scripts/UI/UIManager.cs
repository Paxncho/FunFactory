using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviourSingleton<UIManager> {

    public Text moneyText;
    public Text repText;

    public ShopUI shopUI;
    public ShopPanels shopPanel;
    public InventoryUI inventoryUI;
    public InventoryPanels inventoryPanel;
    public RecipesUI recipesUI;
    public AlertUI alertUI;
    public ExitUI exitUI;
    public ToyGenerationUI generateToy;

    void Start() {
        exitUI.exitButton.onClick.AddListener(Application.Quit);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToExit();
    }

    public void UpdateMoney() {
        moneyText.text = Inventory.Instance.Money.ToString();
    }

        //Recipes Methods
    public void ToRecipes() {
        recipesUI.layout.SetActive(true);
        recipesUI.titleText.text = recipesUI.title;
        Recipes.Instance.UpdateGUI();
    }
    public void BackFromRecipes() {
        recipesUI.layout.SetActive(false);
    }

        //Alert Methods
    public void ShowAlert(string errorMessage) {
        ShowAlert(errorMessage, BackFromAlert);
    }
    public void ShowAlert(string errorMessage, UnityAction call) {
        alertUI.layout.SetActive(true);
        alertUI.descriptionText.text = errorMessage;

        alertUI.takeMeThere.onClick.RemoveAllListeners();
        alertUI.takeMeThere.onClick.AddListener(delegate { call(); });
        alertUI.takeMeThere.onClick.AddListener(BackFromAlert);
    }
    public void BackFromAlert() {
        alertUI.layout.SetActive(false);
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
        inventoryPanel.navigator.MoveToScreen(1);
    }
    public void ToInventoryPieces() {
        inventoryPanel.layout.SetActive(true);
        inventoryPanel.title.text = inventoryPanel.piecesTitle;
        inventoryPanel.workers.SetActive(false);
        inventoryPanel.items.SetActive(true);
        inventoryPanel.buyMaterial.SetActive(false);
        inventoryPanel.hireWorker.SetActive(false);
        Inventory.Instance.UpdateGUI(Inventory.Type.Piece);
        inventoryPanel.navigator.MoveToScreen(0);
    }
    public void ToInventoryWorkers() {
        inventoryPanel.layout.SetActive(true);
        inventoryPanel.title.text = inventoryPanel.workersTitle;
        inventoryPanel.workers.SetActive(true);
        inventoryPanel.items.SetActive(false);
        inventoryPanel.buyMaterial.SetActive(false);
        inventoryPanel.hireWorker.SetActive(true);
        inventoryPanel.navigator.MoveToScreen(2);
    }
    public void BackFromInventory() {
        inventoryPanel.layout.SetActive(false);
        inventoryPanel.navigator.ReactiveMain();
    }

        //Shop Methods
    public void ToShopMaterials() {
        shopPanel.layout.SetActive(true);
        shopPanel.title.text = shopPanel.materialsTitle;
        shopPanel.materials.SetActive(true);
        shopPanel.workers.SetActive(false);
        shopPanel.navigator.MoveToScreen(0);
    }
    public void ToShopWorkers() {
        shopPanel.layout.SetActive(true);
        shopPanel.title.text = shopPanel.workersTitle;
        shopPanel.materials.SetActive(false);
        shopPanel.workers.SetActive(true);
        shopPanel.navigator.MoveToScreen(1);
    }
    public void BackFromShop() {
        shopPanel.layout.SetActive(false);
        shopPanel.navigator.ReactiveMain();
    }

        //Exit Methods
    public void ToExit() {
        exitUI.layout.SetActive(true);
    }
    public void BackFromExit() {
        exitUI.layout.SetActive(false);
    }

        //Structs to organize the Inspector and have a better management of the UI

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
        public PanelNavigation navigator;
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
        public PanelNavigation navigator;
    }
    [System.Serializable] public struct RecipesUI {
        public GameObject layout;
        public Text titleText;
        public string title;

        public Transform parent;
        public GameObject prefab;
    }
    [System.Serializable] public struct TimeBar {
        public Slider bar;
        public Text timeLeftText;
    }
    [System.Serializable] public struct ToyGenerationUI {
        public GameObject basePrefab;
        public GameObject piecePrefab;
        public Transform parent;
        public Transform generationPoint;
    }
    [System.Serializable] public struct AlertUI {
        public GameObject layout;
        public Text descriptionText;
        public Button takeMeThere;
    }
    [System.Serializable] public struct ExitUI {
        public GameObject layout;
        public Button exitButton;
    }
}
