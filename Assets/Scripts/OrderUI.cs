using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour {

    public Animator controller;

	// Use this for initialization
	void Start () {
        if (controller == null)
            controller = GetComponent<Animator>();

        controller.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeAnimation() {
        controller.enabled = true;

        controller.SetBool("Down", !controller.GetBool("Down"));
    }
}
