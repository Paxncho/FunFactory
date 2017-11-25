using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviourSingleton<MiniGameManager> {

    public int totalScore = 0;
    public Generate generator;


    GameObject[] toys;
    string[] actualOrder;

	// Use this for initialization
	void Start () {
        //toys = GameObject.FindGameObjectsWithTag("Toy");
        toys = new GameObject[0];
        actualOrder = new string[1];
        actualOrder[0] = Color.white.ToString();
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

                    foreach(GameObject go in selected) {
                        Toy t = go.GetComponent<Toy>();

                        t.Deselected();
                        //if (CheckToy(t) && selected.Count > 2) {
                        if (selected.Count > 2) {
                            tempScore += t.score;
                            ObjectPool.Kill(go);
                        }
                    }

                    Inventory.Instance.Money += tempScore;
                    UIManager.Instance.UpdateMoney();
                    break;
            }
        }
	}

    bool CheckToy(Toy t) {
        bool[] check = new bool[actualOrder.Length];
        string[] toyData = t.GetToyData();

        //Check the Toy with the Order
        for (int i = 0; i < check.Length; i++) {
            for (int j = 0; j < toyData.Length; j++) {
                if (actualOrder[i] == toyData[j]) {
                    check[i] = true;
                    break;
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

    public void GenerateRandomToy() {
        GameObject go = generator.GenerateObject();
    }
}
