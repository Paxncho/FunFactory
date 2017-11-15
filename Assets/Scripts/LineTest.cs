using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour {

    public LineRenderer line;
    public int virtualLengthOfLine;
    public int lengthOfLine;

    int pointCounter;

	// Use this for initialization
	void Start () {
        pointCounter = line.positionCount;

        DebugPositions();
        //SetNewPositions();
        DebugPositions();
    }

    // Update is called once per frame
    void Update() {
        if (Input.touchCount > 0) {
            SetTouchPoint(Input.GetTouch(0));
        }
    }

    public void DebugPositions() {
        int length = line.positionCount;
        Debug.Log("LINE POSITIONS");

        for (int i = 0; i < length; i++ ){
            Debug.Log(line.GetPosition(i).ToString());
        }

        Debug.Log("~ ~ ~ ~ ~ O ~ ~ ~ ~ ~");
    }

    void SetNewPositions() {
        var points = new Vector3[virtualLengthOfLine];
        var t = Time.time;
        for (int i = 0; i < virtualLengthOfLine; i++) {
            points[i] = new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f);
        }
        line.SetPositions(points);
        line.positionCount = lengthOfLine;
    }

    void SetTouchPoint(Touch touch) {
        bool posModified = false;
        Vector2 touchpos = Vector2.zero;

        switch (touch.phase) {
            case TouchPhase.Began:
                pointCounter++;
                touchpos = touch.position;
                posModified = true;
                break;
            case TouchPhase.Moved:
                touchpos = touch.position;
                posModified = true;
                break;
            case TouchPhase.Ended:

                break;
        }

        line.positionCount = pointCounter;

        Vector3 pos = Camera.main.ScreenToWorldPoint(touchpos);

        pos.z = -1;

        if (pointCounter == 1) {
            line.SetPosition(pointCounter - 1, pos);
            pointCounter++;
            line.positionCount = pointCounter;
        }

        if (posModified)
            line.SetPosition(pointCounter - 1, pos);
    }

    void ModifyPoint(int index, Vector3 pos) {
        line.SetPosition(index, pos);
    }

    public void AddPoint(Vector3 pos) {

    }
}
