using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class Recipes : MonoBehaviourSingleton<Recipes> {


        //Attributes of the Recipe Manager

    //Dictionary to store the recipes
    public Dictionary<string, Piece> recipes = new Dictionary<string, Piece>();

    //XML Which will be used to load the recipes
    public string RecipesPath = "Data/recipes";

    public WorkerStationUI actualUI;


        //Private methods for internal (or common) operations

    void Start() {
        Load();
    }

    //Load All the Recipes
    void Load() {
        TextAsset xml = Resources.Load<TextAsset>(RecipesPath);
        Piece.PieceDataList pdl = DataManager.XMLUnmarshallingFromText<Piece.PieceDataList>(xml.text);
        Piece[] dataPieces = new Piece[pdl.pieces.Length];

        for (int i = 0; i < dataPieces.Length; i++) {
            dataPieces[i] = Piece.ReadData(pdl.pieces[i]);
            recipes.Add(dataPieces[i].id, dataPieces[i]);
        }
    }

    //Clear The UI
    void DeActiveGUI() {
        Transform items = UIManager.Instance.recipesUI.parent;
        for (int i = 0; i < items.childCount; i++) {
            items.GetChild(i).gameObject.SetActive(false);
        }
    }

    //Put a new item in the UI
    void UpdateGUI(Piece p, GameObject prefab, Transform parent) {
        //Instantiate the gui item
        GameObject go = ObjectPool.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, parent);

        //Fix the scale error
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.transform.localScale = Vector3.one;

        //Load the data in the UI
        RecipeUI itemUI = go.GetComponent<RecipeUI>();
        itemUI.item = p.ToInventory();
        itemUI.UpdateUI();
        itemUI.ResetListeners();
        itemUI.AddListener<string>(actualUI.UpdateRecipe, p.id);
        itemUI.AddListener(UIManager.Instance.BackFromRecipes);

        print("Updating");
    }


        //Public methods to connect with other classes

    public Piece GetRandomPiece() {

        if (recipes.Count <= 0)
            return null;

        List<Piece> values = System.Linq.Enumerable.ToList(recipes.Values);
        int size = values.Count;

        return values[Random.Range(0, size)];

    }

    public void GiveThemParts() {
        foreach (KeyValuePair<string, Piece> entry in recipes) {
            Inventory.IItem item = entry.Value.ToInventory();
            item.Quantity = 1;
            Inventory.Instance.Add(item, Inventory.Type.Piece);
        }
    }

    //Update all the UI
    public void UpdateGUI() {
        //Clear First
        DeActiveGUI();

        //Add the recipes to the UI
        GameObject prefab = UIManager.Instance.recipesUI.prefab;
        Transform parent = UIManager.Instance.recipesUI.parent;

        foreach(string code in recipes.Keys) {
            Piece p = recipes[code];
            UpdateGUI(p, prefab, parent);
        }
    }
}
