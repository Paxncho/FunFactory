using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINavigationButtons : MonoBehaviour {

    public GameObject[] buttons;
    //public Sprite activeBtnSprite;
    //public Sprite deactiveBtnSprite;

    public Vector2 activeSize;
    public Vector2 inactiveSize;
    public Vector2 offset;

    public Text[] textButtons;
    public Color activeTextColor;
    public Color deactiveTextColor;

    public void UpdateButtons(int panel) {
        switch (panel) {
            case 0:
                buttons[1].GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                buttons[1].GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                buttons[1].GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                buttons[1].GetComponent<RectTransform>().anchoredPosition = -(inactiveSize + 2 * offset);
                break;
            case 1:
                buttons[1].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
                buttons[1].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                buttons[1].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                buttons[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                break;
            case 2:
                buttons[1].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                buttons[1].GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                buttons[1].GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                buttons[1].GetComponent<RectTransform>().anchoredPosition = inactiveSize + 2 * offset;
                break;
        }

        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i] != null) {
                if (i == panel) {
                    //buttons[i].GetComponent<Image>().sprite = activeBtnSprite;
                    buttons[i].GetComponent<RectTransform>().sizeDelta = activeSize;
                } else {
                    //buttons[i].GetComponent<Image>().sprite = deactiveBtnSprite;
                    buttons[i].GetComponent<RectTransform>().sizeDelta = inactiveSize;
                }
            }
        }


        for (int i = 0; i < textButtons.Length; i++) {
            if (textButtons[i] != null) {
                if (i == panel)
                    textButtons[i].color = activeTextColor;
                else
                    textButtons[i].color = deactiveTextColor;
            }
        }
    }
}
