using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelNavigation : MonoBehaviour {

    public RectTransform[] panels;
    public int panelWidth;

	// Use this for initialization
	void Start () {
        MoveToPanel(1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveToPanel(int panel) {
        for (int i = 0; i < panels.Length; i++) {
            Debug.Log(panels[i].localPosition);
            Vector3 pos = panels[i].localPosition;
            pos.x = panelWidth * (i - panel);
            panels[i].localPosition = pos;
        }
    }
}
