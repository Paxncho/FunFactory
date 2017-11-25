using System;
using UnityEngine;
using System.Collections;

public class Ensambler : MonoBehaviour {

    public Piece[] pieces;

    public int secondsToCreate = 5;
    public bool working;
    public bool PrintInConsole = true;

    TimeSpan lastTimeToyCreated;
    int deltaSeconds = 0;

    // Use this for initialization
    void Start() {
        //Load();
        StartCoroutine(Creating());
    }

    public void Rest() {
        working = false;
        deltaSeconds = 0;
    }

    public void Work() {
        if (pieces.Length == 0) 
            return;

        working = true;
        lastTimeToyCreated = DateTime.Now.TimeOfDay;
    }

    public int TimeLeft() {
        return secondsToCreate - deltaSeconds;
    }

    public void GetRandomPieces() {
        int count = Inventory.Instance.pieces.Count;

        if (count > 3)
            count = 3;

        pieces = new Piece[count];

        int i = 0;
        foreach (string key in Inventory.Instance.pieces.Keys) {
            pieces[i] = ((Piece.InventoryPiece) Inventory.Instance.pieces[key]).piece;

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

            deltaSeconds = (DateTime.Now.TimeOfDay - lastTimeToyCreated).Seconds;

            if (working) {

                if (PrintInConsole)
                    Debug.Log("EnsamblerWorking - " + TimeLeft() + "s left.");

                if (deltaSeconds >= secondsToCreate) {

                    //Check how many toys
                    int toysToCreate = deltaSeconds / secondsToCreate;

                    toysToCreate = CheckPiecesEnough(pieces, toysToCreate);
                    
                    if (toysToCreate > 0) {
                        CreateToy(pieces, toysToCreate);
                    } else {
                        Rest();
                    }

                    int secondsLeft = deltaSeconds % secondsToCreate;
                    lastTimeToyCreated = DateTime.Now.TimeOfDay - new TimeSpan(0, 0, secondsLeft);
                }
            }

            //Update UI
            //if (ui != null) {
            //    ui.UpdateGUI();
            //}
        }
    }

    int CheckPiecesEnough(Piece[] pieces, int totalQuantity) {
        int minimunQuantity = totalQuantity;

        try {
            foreach(Piece p in pieces) {
                int minimumPieceQuantity = Inventory.Instance.pieces[p.id].Quantity;

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

        //Code to generate a Toy
        GameObject prefab = UIManager.Instance.generateToy.basePrefab;
        Transform parent = UIManager.Instance.generateToy.parent;
        Transform startPoint = UIManager.Instance.generateToy.generationPoint;

        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        sr.sprite = pieces[0].sprite;
        sr.color = pieces[0].color;

        GameObject toy = ObjectPool.Instantiate(prefab, startPoint.position, prefab.transform.rotation, parent);

        GameObject piecePrefab = UIManager.Instance.generateToy.piecePrefab;
        for (int i = 1; i < pieces.Length; i++) {
            SpriteRenderer psr = piecePrefab.GetComponent<SpriteRenderer>();
            psr.sprite = pieces[i].sprite;
            psr.color = pieces[i].color;

            GameObject piece = ObjectPool.Instantiate(piecePrefab, toy.transform.position, piecePrefab.transform.rotation, toy.transform);

            //Add the Joint to make them together;
            toy.AddComponent<FixedJoint2D>().connectedBody = piece.GetComponent<Rigidbody2D>();
        }

        if (PrintInConsole)
            Debug.Log("Toy Created");

    }
    
}
