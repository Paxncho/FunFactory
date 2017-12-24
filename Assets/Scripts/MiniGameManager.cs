using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MiniGameManager : MonoBehaviourSingleton<MiniGameManager> {

    GameObject[] toys;
    public Piece[] actualOrder;

    public OrderUI UI;

    [HideInInspector] public int Count = 0;
    public int MaxCount = 10;

    TimeSpan lastTimeToyCreated;
    float creatingToyDelay = 0.5f;

    // Use this for initialization
    void Start () {
        //toys = GameObject.FindGameObjectsWithTag("Toy");
        toys = new GameObject[0];
        actualOrder = new Piece[3];
        GenerateOrder();
        lastTimeToyCreated = DateTime.Now.TimeOfDay;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
            switch (Input.GetTouch(0).phase) {
                case TouchPhase.Ended:
                    int tempScore = 0;

                    List<GameObject> selected = new List<GameObject>();

                    foreach (GameObject go in toys) {
                        //Debug.Log(go);
                        Toy t = go.GetComponent<Toy>();

                        if (t.selected) {
                            selected.Add(go);
                        }
                    }

                    foreach (GameObject go in selected) {
                        Toy t = go.GetComponent<Toy>();

                        t.Deselected();
                        if (selected.Count > 2) {
                            //if (selected.Count > 2) {
                            Count++;
                            tempScore += t.score;
                            ObjectPool.Kill(go);
                            Inventory.Instance.Remove(go.GetComponent<Toy>().data);
                        }
                    }

                    if (Count >= MaxCount) {
                        UI.ResetTime();
                        GenerateOrder();
                    }

                    UI.UpdateCount();
                    Inventory.Instance.Money += tempScore;
                    UIManager.Instance.UpdateMoney();
                    break;
            }
        }
	}

    //Generate only one meanwhile
    public void GenerateOrder() {

        for (int i = 0; i < actualOrder.Length; i++) {
            actualOrder[i] = null;
        }

        int color = UnityEngine.Random.Range(0, 2);

        if (color == 0) {
            Piece p = Inventory.Instance.GetRandomPiece();

            if (p == null) {
                p = Recipes.Instance.GetRandomPiece();
            }

            actualOrder[2] = p;
        } else {
            Piece p = Inventory.Instance.GetRandomPiece();

            if (p == null)
                p = Recipes.Instance.GetRandomPiece();

            actualOrder[0] = p;
        }

        UI.UpdateOrder();
    }

    bool CheckToy(Toy t) {
        bool[] check = new bool[actualOrder.Length];
        Piece[] toyData = t.GetToyData();

        //Check the Toy with the Order
        for (int i = 0; i < check.Length; i++) {
            for (int j = 0; j < toyData.Length; j++) {
                if (actualOrder[i] == null)
                    break;

                if (i != 2) { //Check Sprites
                    if (actualOrder[i].sprite == toyData[j].sprite) {
                        check[i] = true;
                        break;
                    }
                } else {
                    if (actualOrder[i].color == toyData[j].color) {
                        check[i] = true;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < check.Length; i++) {
            if (!check[i])
                return false;
        }

        return true;
    }

    public void AddToy(GameObject toy) {
        GameObject[] temp = (GameObject[]) toys.Clone();
        toys = new GameObject[temp.Length + 1];

        //Debug.Log(temp.Length + ", " + toys.Length);

        System.Array.Copy(temp, toys, temp.Length);
        toys[temp.Length] = toy;
    }

    public void GenerateToy(Toy.ToyData data) {
        double deltaSeconds = (DateTime.Now.TimeOfDay - lastTimeToyCreated).TotalSeconds;

        if (deltaSeconds < creatingToyDelay) {
            StartCoroutine(ToyStack(data, (float)(creatingToyDelay + .1f - deltaSeconds)));
            return;
        }

        //Code to generate a Toy
        GameObject prefab = UIManager.Instance.generateToy.basePrefab;
        Transform parent = UIManager.Instance.generateToy.parent;
        Transform startPoint = UIManager.Instance.generateToy.generationPoint;

        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        sr.sprite = data.pieces[0].sprite;
        sr.color = data.pieces[0].color;

        GameObject toy = ObjectPool.Instantiate(prefab, startPoint.position, prefab.transform.rotation, parent);

        toy.GetComponent<Toy>().item = data.pieces[0];
        toy.GetComponent<Toy>().data = data;

        GameObject piecePrefab = UIManager.Instance.generateToy.piecePrefab;
        for (int i = 1; i < data.pieces.Length; i++) {
            SpriteRenderer psr = piecePrefab.GetComponent<SpriteRenderer>();
            psr.sprite = data.pieces[i].sprite;
            psr.color = data.pieces[i].color;

            GameObject piece = ObjectPool.Instantiate(piecePrefab, toy.transform.position, piecePrefab.transform.rotation, toy.transform);

            piece.GetComponent<MiniGamePiece>().item = data.pieces[i];

            //Add the Joint to make them together;
            //toy.AddComponent<FixedJoint2D>().connectedBody = piece.GetComponent<Rigidbody2D>();
        }

        lastTimeToyCreated = DateTime.Now.TimeOfDay;
    }

    IEnumerator ToyStack(Toy.ToyData data, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        GenerateToy(data);
    }
}
