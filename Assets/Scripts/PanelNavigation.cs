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
    void Update() {
        if (Input.touchCount > 0) {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos2D = touchPos;

            RaycastHit2D[] hits = Physics2D.RaycastAll(touchPos2D, Camera.main.transform.forward);
            //Debug.Log(hits.Length);

            foreach (RaycastHit2D hit in hits) {
                //Debug.Log("Object Hitted: " + hit.transform.position);
            }
        }

        //if (Input.GetMouseButton(0)) {
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit[] hits = Physics.RaycastAll(ray);

        //        foreach (RaycastHit hit in hits) {
        //            Debug.Log("Object Hitted: " + hit.collider.name);
        //        }
        //}
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
