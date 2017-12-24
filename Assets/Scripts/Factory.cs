using System;
using System.Collections;
using System.Xml.Serialization;

using UnityEngine;

public class Factory : MonoBehaviour {

    public static string saveFileName = "factory.fundata";

    /*
     * Datos para guardar
     * Id Worker
     * Id Receta
     * lastTimePieceCreated
     * working
     */

    public Worker worker;
    public Piece pieceToCreate;
    
    [HideInInspector] public int secondsToCreate = 5;
    [HideInInspector] public bool working;
    public WorkerStationUI ui;

    TimeSpan lastTimePieceCreated;
    int deltaSeconds = 0;

    public float tirednessRate = 0.05f;

    void Start () {
        //CargarDatosDelPlayerPrefs;
        StartCoroutine(Creating());
    }

    public FactoryData Save() {
        FactoryData data = new FactoryData() {
            working = working,
            lastTimeCreated = lastTimePieceCreated.ToString()
        };

        if (worker != null)
            data.workerId = worker.id;
        if (pieceToCreate != null)
            data.recipeId = pieceToCreate.id;

        return data;
    }
    public void TrySave() {
        string filePath = Application.persistentDataPath + "/" + saveFileName;
        DataManager.XMLMarshalling(filePath, Save());
    }
    public void Load(FactoryData data) {
        if (data.workerId != null)
            worker = ((Worker.HiredWorker) Inventory.Instance.workers[data.workerId]).worker;

        if (data.recipeId != null)
            ui.UpdateRecipe(data.recipeId);

        lastTimePieceCreated = TimeSpan.Parse(data.lastTimeCreated);

        if (data.working) {
            secondsToCreate = pieceToCreate.secondsNeeded;
            working = true;
        }

        ui.UpdateGUI();
    }
    public void TryLoad() {
        string filePath = Application.persistentDataPath + "/" + saveFileName;

        if (System.IO.File.Exists(filePath)) {
            FactoryData data = DataManager.XMLUnmarshalling<FactoryData>(filePath);
            Load(data);
        }
    }

    public void Rest() {
        working = false;
        deltaSeconds = 0;
    }

    public void Work() {
        if (worker == null || pieceToCreate == null)
            return;

        if (CheckMaterialsEnough(pieceToCreate.materialsNeeded, 1) <= 0) {
            UIManager.Instance.ShowAlert("There are not enough little things.", UIManager.Instance.ToShopMaterials);
            return;
        }


        working = true;
        lastTimePieceCreated = DateTime.Now.TimeOfDay;
        secondsToCreate = pieceToCreate.secondsNeeded;
    }

    public int TimeLeft() {
        return secondsToCreate - deltaSeconds;
    }

    IEnumerator Creating() {
        while (true) {
            yield return new WaitForSeconds(1.0f);

            deltaSeconds = (int) (DateTime.Now.TimeOfDay - lastTimePieceCreated).TotalSeconds;

            if (working) {
                if (deltaSeconds >= secondsToCreate) {

                    //Check how many pieces 
                    int piecesToCreate = deltaSeconds / secondsToCreate;

                    piecesToCreate = CheckMaterialsEnough(pieceToCreate.materialsNeeded, piecesToCreate);

                    if (piecesToCreate > 0 && worker.tired < 1) {
                        CreatePiece(pieceToCreate, piecesToCreate);
                    } else {
                        Rest();
                    }

                    int secondsLeft = deltaSeconds % secondsToCreate;
                    lastTimePieceCreated = DateTime.Now.TimeOfDay - new TimeSpan(0, 0, secondsLeft);

                    if (CheckMaterialsEnough(pieceToCreate.materialsNeeded, 1) <= 0)
                        Rest();
                }
                
                //Update UI
                if (ui != null) {
                    ui.UpdateGUI();
                }

            }
            
        }
    }

    int CheckMaterialsEnough(Piece.Requirement[] requirements, int totalQuantity) {
        int minimunQuantity = totalQuantity;
        try {
            foreach (Piece.Requirement requirement in requirements) {
                int minimumMaterialQuantity = Inventory.Instance.materials[requirement.material.id].Quantity / requirement.quantity;

                if (minimumMaterialQuantity < minimunQuantity) {
                    minimunQuantity = minimumMaterialQuantity;
                }
            }
        } catch (Exception e) {
            minimunQuantity = 0;
        }

        return minimunQuantity;
    }

    void CreatePiece(Piece piece, int totalQuantity) {
        foreach (Piece.Requirement requirement in piece.materialsNeeded) {
            int quantityNecesary = totalQuantity * requirement.quantity;
            Inventory.Instance.Remove(requirement.material.id, quantityNecesary, Inventory.Type.Material);
        }

        Inventory.IItem item = piece.ToInventory();
        item.Quantity *= totalQuantity;

        Inventory.Instance.Add(item, Inventory.Type.Piece);
        worker.tired += tirednessRate;

        if (worker.tired >= 1)
            Rest();
    }

    void OnApplicationQuit() {
        TrySave();
    }

    [XmlRoot]
    public class FactoryData : DataManager.IData {
        [XmlAttribute] public string workerId;
        [XmlAttribute] public string recipeId;
        [XmlAttribute] public bool working;
        [XmlAttribute] public string lastTimeCreated;
        public FactoryData() { }
    }
}
