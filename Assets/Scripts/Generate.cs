using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject prefab;
    public Transform startPoint;

    public Transform parent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateObjectBtn() {
        GenerateObject();
    }

    public GameObject GenerateObject() {
        return ObjectPool.Instantiate(prefab, startPoint.position, Quaternion.identity, parent);
    }

    //public void GenerateObject() {
        
    //}
}
