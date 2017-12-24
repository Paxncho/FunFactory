using System;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Ensambler : MonoBehaviour {

    public static string saveFileName = "ensambler.fundata";

    /*
     * Datos para guardar
     * [] idPieces
     * working
     * lastTimeCreated
     */

    public List<Piece> pieces = new List<Piece>();
    //public Piece[] pieces;

    public int secondsToCreate = 5;
    public bool working;
    public bool PrintInConsole = true;

    TimeSpan lastTimeToyCreated;
    double deltaSeconds = 0;


    public EnsamblerUI ui;

    void Start() {
        StartCoroutine(Creating());
    }

    public EnsamblerData Save() {

        EnsamblerData data = new EnsamblerData();
        List<string> ids = new List<string>();
        for (int i = 0; i < pieces.Count; i++) {
            ids.Add(pieces[i].id);
        }

        data.piecesId = ids.ToArray();
        data.lastTimeToyCreated = lastTimeToyCreated.ToString();
        data.working = working;

        return data;
    }
    public void TrySave() {
        string filePath = Application.persistentDataPath + "/" + saveFileName;
        DataManager.XMLMarshalling(filePath, Save());

    }
    public void Load(EnsamblerData data) {
        pieces.Clear();
        
        for (int i = 0; i < data.piecesId.Length; i++) {
            pieces.Add(((Piece.InventoryPiece)Inventory.Instance.pieces[data.piecesId[i]]).piece);
        }

        lastTimeToyCreated = TimeSpan.Parse(data.lastTimeToyCreated);

        if (data.working) {
            secondsToCreate = pieces.Count * 3;
            working = true;
        }

        ui.UpdateGUI();
            
    }
    public void TryLoad() {
        string filePath = Application.persistentDataPath + "/" + saveFileName;

        if (System.IO.File.Exists(filePath)) {
            EnsamblerData data = DataManager.XMLUnmarshalling<EnsamblerData>(filePath);
            Load(data);
        }
    }

    public void Rest() {
        working = false;
        deltaSeconds = 0;
    }

    public void Work() {
        if (pieces.Count == 0) {
            return;
        }

        if (CheckPiecesEnough(pieces.ToArray(), 1) <= 0) {
            UIManager.Instance.ShowAlert("There are not enough parts to glue.", UIManager.Instance.ToInventoryPieces);
            return;
        }

        secondsToCreate = pieces.Count * 3;
        working = true;
        lastTimeToyCreated = DateTime.Now.TimeOfDay;
    }

    public int TimeLeft() {
        return secondsToCreate - (int) deltaSeconds;
    }

    public void GetRandomPieces() {
        int count = Inventory.Instance.pieces.Count;

        if (count > 3)
            count = 3;

        pieces.Clear();

        int i = 0;
        foreach (string key in Inventory.Instance.pieces.Keys) {
            pieces.Add(((Piece.InventoryPiece) Inventory.Instance.pieces[key]).piece);

            i++;

            if (i == 3) {
                break;
            }
        }
        Rest();
        Work();
    }

    IEnumerator Creating() {
        while (true) {
            yield return new WaitForSeconds(1.0f);

            deltaSeconds = (DateTime.Now.TimeOfDay - lastTimeToyCreated).TotalSeconds;

            if (working) {

                if (PrintInConsole)
                    Debug.Log("EnsamblerWorking - " + TimeLeft() + "s left.");

                if (deltaSeconds >= secondsToCreate) {

                    //Check how many toys
                    int toysToCreate = (int) deltaSeconds / secondsToCreate;

                    toysToCreate = CheckPiecesEnough(pieces.ToArray(), toysToCreate);
                    
                    if (toysToCreate > 0) {
                        CreateToy(pieces.ToArray(), toysToCreate);
                    } else {
                        Rest();
                        pieces.Clear();
                    }

                    int secondsLeft = (int) deltaSeconds % secondsToCreate;
                    lastTimeToyCreated = DateTime.Now.TimeOfDay - new TimeSpan(0, 0, secondsLeft);

                    if (CheckPiecesEnough(pieces.ToArray(), 1) <= 0)
                        Rest();
                }

                //Update UI
                if (ui != null) {
                    ui.UpdateGUI();
                }
            }
        }
    }

    int CheckPiecesEnough(Piece[] pieces, int totalQuantity) {
        int minimunQuantity = totalQuantity;

        try {
            foreach(Piece p in pieces) {
                int minimumPieceQuantity = Inventory.Instance.pieces[p.id].Quantity;

                if (minimumPieceQuantity < minimunQuantity)
                    minimunQuantity = minimumPieceQuantity;

                if (PrintInConsole)
                    Debug.Log(p + ", " + minimumPieceQuantity);
            }
        }
        catch (Exception e) {
            minimunQuantity = 0;
        }

        if (PrintInConsole)
            Debug.Log("Checking Pieces");

        return minimunQuantity;
    }

    void CreateToy(Piece[] pieces, int totalQuantity) {
        foreach (Piece p in pieces) {
            Inventory.Instance.Remove(p.id, totalQuantity, Inventory.Type.Piece);
        }

        Toy.ToyData data = new Toy.ToyData() {
            pieces = pieces
        };

        Inventory.Instance.Add(data);

        MiniGameManager.Instance.GenerateToy(data);
    }

    void OnApplicationQuit() {
        TrySave();
    }

    [XmlRoot] public class EnsamblerData : DataManager.IData {
        [XmlArray, XmlArrayItem] public string[] piecesId;
        [XmlAttribute] public bool working;
        [XmlAttribute] public string lastTimeToyCreated;
        public EnsamblerData() { }
    }

}
