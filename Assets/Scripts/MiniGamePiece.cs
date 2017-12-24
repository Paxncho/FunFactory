using System;
using UnityEngine;

public class MiniGamePiece : MonoBehaviour {
    public Piece item;

    // Use this for initialization
    void Start() {

        //Destroy the previous colliders in the gameObject
        foreach (Collider2D c in GetComponents<Collider2D>()) {
            Destroy(c);
        }

        //Assign the new Sprite
        //if (data.idTexture != "") {
        //    Sprite t = SpritePool.LoadSprite(data.idTexture);
        //    GetComponent<SpriteRenderer>().sprite = t;
        //    Debug.Log("LOADED " + t);
        //}

        //Create them a Collider
        //gameObject.AddComponent<CapsuleCollider2D>();
    }

    public Piece GetData() {
        return item;
    }
}

[Serializable]
public class DataPiece {
    [SerializeField] public UnityEngine.Color color;
    [SerializeField] public string idTexture;
    public DataPiece() { }
}
