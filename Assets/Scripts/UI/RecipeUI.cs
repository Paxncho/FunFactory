using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUI : InventoryItemUI {

    public GameObject prefab;
    public Transform parent;

    void ClearMaterials() {
        for (int i = 0; i < parent.childCount; i++) {
            parent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public override void UpdateUI() {
        ClearMaterials();
        base.UpdateUI();

        if (item == null)
            return;

        Piece.InventoryPiece piece = (Piece.InventoryPiece) item;

        for (int i = 0; i < piece.piece.materialsNeeded.Length; i++) {
            GameObject go = ObjectPool.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, parent);

            RectTransform rect = go.GetComponent<RectTransform>();
            rect.transform.localScale = Vector3.one;

            InventoryItemUI itemUI = go.GetComponent<InventoryItemUI>();
            GameMaterial.InventoryItem gmItem = new GameMaterial.InventoryItem() {
                material = piece.piece.materialsNeeded[i].material,
                quantity = piece.piece.materialsNeeded[i].quantity
            };

            itemUI.item = gmItem;
            itemUI.UpdateUI();
        }
    }
}
