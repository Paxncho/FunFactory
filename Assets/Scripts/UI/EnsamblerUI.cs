using UnityEngine;
using System.Collections;

public class EnsamblerUI : MonoBehaviour {

    public Ensambler ensambler;

    public UIManager.TimeBar time;
    public RecipeUI recipe;

    void Start() {

        if (ensambler != null) {
            UpdateGUI();
            ensambler.ui = this;
        }
    }

    public void UpdateGUI() {
        UpdateTime();

    }

    public void UpdateTime() {
        int timeLeft = ensambler.TimeLeft();

        if (ensambler.working) {
            float percentage = 1.0f - (timeLeft * 1.0f / ensambler.secondsToCreate);

            time.bar.value = percentage;
            time.timeLeftText.text = timeLeft + "s Left";
        } else {
            time.timeLeftText.text = "Not Working";
        }
    }
}
