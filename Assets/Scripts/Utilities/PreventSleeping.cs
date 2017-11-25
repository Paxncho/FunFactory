using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventSleeping : MonoBehaviour {

    void Awake() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
