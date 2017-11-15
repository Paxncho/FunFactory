using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour {

    public Color selectedColor = Color.gray;
    public int score = 100;

    [HideInInspector] public bool selected;
    
    SpriteRenderer m_renderer;
    Color originalColor;

	// Use this for initialization
	void Start () {
        m_renderer = GetComponent<SpriteRenderer>();

        originalColor = m_renderer.color;
        MiniGameManager.Instance.AddToy(gameObject);
        Debug.Log("This is Start");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseEnter() {
        Debug.Log("Selected");
        m_renderer.color = selectedColor;

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
            sr.color = selectedColor;
        }

        selected = true;
    }

    public void Deselected() {
        m_renderer.color = originalColor;

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
            sr.color = originalColor;
        }

        selected = false;
    }

    public string[] GetToyData() {
        List<string> dataList = new List<string>();
        foreach (Piece p in GetComponentsInChildren<Piece>()) {
            foreach (string s in p.GetData()) {
                dataList.Add(s);
            }
        }

        return dataList.ToArray();
    }
}
