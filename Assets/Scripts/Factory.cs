using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

    public int SecondsToCreate;

    [HideInInspector] public bool working;

    TimeSpan lastTimePieceCreated;

	// Use this for initialization
	void Start () {

        //CargarDatosDelPlayerPrefs;

        StartCoroutine(Creating());
    }
	
	// Update is called once per frame
	void Update () { }

    public void Rest() {
        working = false;
    }

    public void Work() {
        working = true;
        lastTimePieceCreated = DateTime.Now.TimeOfDay;
    }

    IEnumerator Creating() {
        while (true) {
            yield return new WaitForSeconds(1.0f);

            int deltaSeconds = (DateTime.Now.TimeOfDay - lastTimePieceCreated).Seconds;

            if (working) {
                if (deltaSeconds >= SecondsToCreate) {

                    int piecesToCreate = deltaSeconds / SecondsToCreate;
                    
                    //Add pieces to Inventory

                    int secondsLeft = deltaSeconds % SecondsToCreate;
                    lastTimePieceCreated = DateTime.Now.TimeOfDay - new TimeSpan(0, 0, secondsLeft);
                }
                //update GUI
            }
            
        }
    }
}
