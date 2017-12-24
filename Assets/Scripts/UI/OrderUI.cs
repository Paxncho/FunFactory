using System;

using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {

    public Animator controller;

    public Image sprite1;
    public Image color;
    public Image sprite2;

    public Text count;
    public Text time;

    int secondsPerOrder = 120;
    TimeSpan startTime;

	// Use this for initialization
	void Start () {
        if (controller == null)
            controller = GetComponent<Animator>();

        controller.enabled = false;

        UpdateCount();

        ResetTime();
	}

    void Update() {
        UpdateTime();
    }

    public void UpdateCount() {
        string countString = MiniGameManager.Instance.Count + " / " + MiniGameManager.Instance.MaxCount;
        count.text = countString;
    }

    public void UpdateTime() {
        double deltaSeconds = (DateTime.Now.TimeOfDay - startTime).TotalSeconds;

        if (secondsPerOrder <= deltaSeconds) {

            int secondsLeft = (int) deltaSeconds % secondsPerOrder;
            startTime = DateTime.Now.TimeOfDay - new TimeSpan(0, 0, secondsLeft);

            MiniGameManager.Instance.GenerateOrder();
        }

        time.text = (secondsPerOrder - (int) deltaSeconds) + "";
    }

    public void ChangeAnimation() {
        controller.enabled = true;

        controller.SetBool("Down", !controller.GetBool("Down"));
    }

    public void ResetTime() {
        startTime = DateTime.Now.TimeOfDay;
        UpdateTime();
    }

    public void UpdateOrder() {

        Piece[] order = MiniGameManager.Instance.actualOrder;

        if (order[0] != null) {
            sprite1.sprite = order[0].sprite;
            sprite1.color = Color.white;
        } else {
            sprite1.color = new Color(0, 0, 0, 0);
        }

        if (order[1] != null) {
            sprite2.sprite = order[1].sprite;
            sprite2.color = Color.white;
        } else {
            sprite2.color = new Color(0, 0, 0, 0);
        }

        if (order[2] != null) {
            color.color = order[2].color;
        } else {
            color.color = new Color(0, 0, 0, 0);
        }
    }
}
