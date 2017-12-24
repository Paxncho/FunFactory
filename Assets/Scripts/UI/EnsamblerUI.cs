using UnityEngine;
using UnityEngine.UI;

public class EnsamblerUI : MonoBehaviour {
    public Ensambler ensambler;
    public Transform toyPreview;
    public GameObject toyPreviewPrefab;

    public UIManager.TimeBar time;
    public PiecesToEnsambleUI toEnsamble;

    void Start() {

        if (ensambler != null) {
            UpdateGUI();
            ensambler.ui = this;
        }
    }

    public void UpdateGUI() {
        UpdateTime();
        UpdatePieces();
    }

    public void UpdateTime() {
        int timeLeft = ensambler.TimeLeft();

        if (ensambler.working) {
            float percentage = 1.0f - (timeLeft * 1.0f / ensambler.secondsToCreate);

            time.bar.value = percentage;
            time.timeLeftText.text = timeLeft + "s Left";
        } else {
            time.bar.value = 0;
            time.timeLeftText.text = "Not Working";
            UpdatePieces();
        }
    }

    public void UpdatePieces() {
        ClearPieces();

        foreach (Piece p in ensambler.pieces) {
            //Create the piece in the bottom bar
            GameObject go = ObjectPool.Instantiate(toEnsamble.prefab, toEnsamble.prefab.transform.position, toEnsamble.prefab.transform.rotation, toEnsamble.parent);

            RectTransform rect = go.GetComponent<RectTransform>();
            rect.transform.localScale = Vector3.one;

            InventoryItemUI itemUI = go.GetComponent<InventoryItemUI>();
            itemUI.ResetListeners();
            itemUI.AddListener<string>(RemovePiece, p.id);
            itemUI.AddListener(ensambler.Rest);
            itemUI.AddListener(UpdateTime);

            itemUI.item = p.ToInventory();
            itemUI.UpdateUI();

            //Create the piece in the preview
            GameObject goP = ObjectPool.Instantiate(toyPreviewPrefab, toyPreviewPrefab.transform.position, toyPreviewPrefab.transform.rotation, toyPreview);

            RectTransform goTransform = goP.GetComponent<RectTransform>();
            //goTransform.localScale = Vector3.one;
            goTransform.anchorMin = Vector2.zero;
            goTransform.anchorMax = Vector2.one;
            goTransform.pivot = new Vector2(0.5f, 0.5f);
            goTransform.localPosition = Vector3.zero;

            goP.GetComponent<Image>().sprite = p.sprite;
            goP.GetComponent<Image>().color = p.color;
            goP.GetComponent<Image>().preserveAspect = true;
        }

        GameObject goAdd = ObjectPool.Instantiate(toEnsamble.addPiecePrefab, toEnsamble.addPiecePrefab.transform.position, toEnsamble.addPiecePrefab.transform.rotation, toEnsamble.parent);
        RectTransform rectAdd = goAdd.GetComponent<RectTransform>();
        rectAdd.transform.localScale = Vector3.one;

        goAdd.GetComponent<InventoryItemUI>().ResetListeners();
        goAdd.GetComponent<InventoryItemUI>().AddListener(AddPiece);

        print("Pieces updated");
    }

    public void ClearPieces() {
        for (int i = 0; i < toEnsamble.parent.childCount; i++) {
            toEnsamble.parent.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < toyPreview.childCount; i++) {
            toyPreview.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void RemovePiece(string code) {
        ensambler.pieces.Remove(((Piece.InventoryPiece)Inventory.Instance.pieces[code]).piece);
        UpdatePieces();
    }

    public void AddPiece(string code) {
        Transform piecesParent = UIManager.Instance.inventoryUI.itemsParents;

        for (int i = 0; i < piecesParent.childCount; i++) {
            InventoryItemUI iiui = piecesParent.GetChild(i).GetComponent<InventoryItemUI>();

            if (iiui != null) {
                iiui.ResetListeners();
            }
        }

        UIManager.Instance.BackFromInventory();

        ensambler.pieces.Add(((Piece.InventoryPiece)Inventory.Instance.pieces[code]).piece);
        UpdatePieces();
    }

    public void AddPiece() {
        UIManager.Instance.ToInventoryPieces();
        Transform piecesParent = UIManager.Instance.inventoryUI.itemsParents;

        for (int i = 0; i < piecesParent.childCount; i++) {
            InventoryItemUI iiui = piecesParent.GetChild(i).GetComponent<InventoryItemUI>();

            if (iiui != null) {
                iiui.AddListener<string>(AddPiece, iiui.item.Code);
                iiui.AddListener(Work);
            }
        }
    }

    public void Work() {
        ensambler.Rest();
        ensambler.Work();
    }

    [System.Serializable] public struct PiecesToEnsambleUI {
        public GameObject prefab;
        public Transform  parent;
        public GameObject addPiecePrefab;
    }
    
}
