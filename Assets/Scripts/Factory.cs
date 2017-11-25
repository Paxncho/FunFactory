using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

    public Worker worker;
    public Piece pieceToCreate;
    
    [HideInInspector] public int secondsToCreate = 5;
    [HideInInspector] public bool working;
    public WorkerStationUI ui;

    TimeSpan lastTimePieceCreated;
    int deltaSeconds = 0;

    void Start () {
        //CargarDatosDelPlayerPrefs;
        StartCoroutine(Creating());
    }

    public void Rest() {
        working = false;
        deltaSeconds = 0;
    }

    public void Work() {
        if (worker == null || pieceToCreate == null)
            return;

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

            deltaSeconds = (DateTime.Now.TimeOfDay - lastTimePieceCreated).Seconds;

            if (working) {
                if (deltaSeconds >= secondsToCreate) {

                    //Check how many pieces 
                    int piecesToCreate = deltaSeconds / secondsToCreate;

                    piecesToCreate = CheckMaterialsEnough(pieceToCreate.materialsNeeded, piecesToCreate);

                    if (piecesToCreate > 0) {
                        CreatePiece(pieceToCreate, piecesToCreate);
                    } else {
                        Rest();
                    }

                    int secondsLeft = deltaSeconds % secondsToCreate;
                    lastTimePieceCreated = DateTime.Now.TimeOfDay - new TimeSpan(0, 0, secondsLeft);
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
    }
}
