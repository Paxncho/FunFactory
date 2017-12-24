using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelNavigation : MonoBehaviour {

    public RectTransform[] panels;
    public int panelWidth;

    public Transform[] worldScreens;
    public int worldWidth;

    public UINavigationButtons buttons;
    public bool main;
    public PanelNavigation mainPanel;

    int actualScreen;

	// Use this for initialization
	void Start () {
        if (main)
            MoveToScreen(1);
	}

    // Update is called once per frame
    void Update() {
        switch (SwipeManager.Direction) {
            case SwipeDirection.Right:

                if (actualScreen > 0)
                    MoveToScreen(actualScreen - 1);

                break;
            case SwipeDirection.Left:

                if (actualScreen < panels.Length - 1)
                    MoveToScreen(actualScreen + 1);

                break;
        }
    }

    public void MoveToScreen(int panel) {
        if (!main)
            DeactiveMain();

        for (int i = 0; i < panels.Length; i++) {
            if (panels[i] != null) {
                //Debug.Log(panels[i].localPosition);
                Vector3 pos = panels[i].localPosition;
                pos.x = panelWidth * (i - panel);
                panels[i].localPosition = pos;
            }
        }

        for (int i = 0; i < worldScreens.Length; i++) {
            if (worldScreens[i] != null) {
                Vector3 pos = worldScreens[i].localPosition;
                pos.x = worldWidth * (i - panel);
                worldScreens[i].localPosition = pos;
            }
        }

        buttons.UpdateButtons(panel);

        actualScreen = panel;
    }

    public void DeactiveMain() {
        if (!main)
            mainPanel.gameObject.SetActive(false);
    }

    public void ReactiveMain() {
        if (!main)
            mainPanel.gameObject.SetActive(true);
    }
}
