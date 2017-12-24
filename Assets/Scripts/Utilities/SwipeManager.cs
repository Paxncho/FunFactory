using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection {
    None = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4
}

public class SwipeManager : MonoBehaviourSingleton<SwipeManager> {

    public static SwipeDirection Direction;

    public Vector2 distanceRequieredToSwipe = new Vector2(200, 400);
    public Rect rectWhereToSwipe;

    Vector2 initialPos;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {

        Direction = SwipeDirection.None;

        if (Input.touchCount > 0) {
            CheckSwipe(Input.GetTouch(0));
        }
    }

    void CheckSwipe(Touch touch) {
        switch (touch.phase) {
            case TouchPhase.Began:
                initialPos = touch.position;
                break;

            case TouchPhase.Ended:
                Vector2 deltaPos = touch.position - initialPos;

                if (!rectWhereToSwipe.Contains(touch.position) || !rectWhereToSwipe.Contains(initialPos))
                    break;

                if (Mathf.Abs(deltaPos.x) > distanceRequieredToSwipe.x) {
                    if (deltaPos.x > 0)
                        Direction = SwipeDirection.Right;
                    else
                        Direction = SwipeDirection.Left;
                }

                if (Mathf.Abs(deltaPos.y) > distanceRequieredToSwipe.y) {
                    if (deltaPos.y > 0)
                        Direction = SwipeDirection.Up;
                    else
                        Direction = SwipeDirection.Down;
                }
                break;
        }
    }

    public static bool IsSwiping(SwipeDirection dir) {
        return Direction == dir;
    }
}
