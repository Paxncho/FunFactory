using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour {

    public Color selectedColor = Color.gray;
    public int score = 200;

    public Piece item;
    public ToyData data;

    [HideInInspector] public bool selected;
    
    SpriteRenderer m_renderer;
    Color originalColor;

	// Use this for initialization
	void Start () {
        m_renderer = GetComponent<SpriteRenderer>();

        originalColor = m_renderer.color;
        MiniGameManager.Instance.AddToy(gameObject);

        //Collider Thing
        foreach (Collider2D c in GetComponents<Collider2D>()) {
            Destroy(c);
        }

        gameObject.AddComponent<CapsuleCollider2D>();
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

    public Piece[] GetToyData() {
        return data.pieces;
    }

    public class ToyData {
        public Piece[] pieces;
        public ToyData() { }

        public TData ToData() {
            string[] ids = new string[pieces.Length];
            for (int i = 0; i < ids.Length; i++) {
                ids[i] = pieces[i].id;
            }
            return new TData() {
                idPieces = ids
            };
        }
    }

    public class TData : DataManager.IData {
        public string[] idPieces;
        public TData() { }
    }

}
