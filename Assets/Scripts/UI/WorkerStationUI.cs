using UnityEngine;
using UnityEngine.UI;

public class WorkerStationUI : MonoBehaviour {

    public Factory Factory;
    public GameObject Stats;

    public WorkerStatsUI[] WorkerStats = new WorkerStatsUI[3];
    public Image WorkTable;
    public Image Worker;

    public UIManager.TimeBar time;
    public RecipeUI Recipe;

    void Start() {
        WorkerStats[0].bar.interactable = false;
        WorkerStats[1].bar.interactable = false;
        WorkerStats[2].bar.interactable = false;

        if (Factory != null) {
            UpdateGUI();
            Factory.ui = this;
        }

        Recipes.Instance.actualUI = this;
    }

    public void UpdateGUI() {
        if (Factory == null) {
            Stats.SetActive(false);
            return;
        } else if (Factory.worker == null) {
            Stats.SetActive(false);
            UpdateTime();
            return;
        } else {
            Stats.SetActive(true);
        }

        WorkerStats[0].titleText.text = WorkerStats[0].title;
        WorkerStats[1].titleText.text = WorkerStats[1].title;
        WorkerStats[2].titleText.text = WorkerStats[2].title;

        WorkerStats[0].bar.value = Factory.worker.talent;
        WorkerStats[1].bar.value = Factory.worker.motivation;
        WorkerStats[2].bar.value = Factory.worker.tired;

        WorkerStats[0].percentage.text = Factory.worker.talent.ToString();
        WorkerStats[1].percentage.text = (Factory.worker.motivation * 100f) + "%";
        WorkerStats[2].percentage.text = (Factory.worker.tired * 100f) + "%";

        Recipe.UpdateUI();
        UpdateTime();
    }

    public void UpdateTime() {
        int timeLeft = Factory.TimeLeft();

        if (Factory.working) {
            float percentage = 1.0f - (timeLeft * 1.0f / Factory.secondsToCreate);

            time.bar.value = percentage;
            time.timeLeftText.text = timeLeft + "s Left";
        } else {
            time.timeLeftText.text = "Not Working";

        }
    }

    public void UpdateRecipe(string code) {
        Factory.pieceToCreate = Recipes.Instance.pieces[code];
        Recipe.item = Factory.pieceToCreate.ToInventory();
        Factory.Rest();
        Factory.Work();
        UpdateGUI();
    }

    public void UpdateWorker(string code) {
        Factory.worker = ((Worker.HiredWorker)Inventory.Instance.workers[code]).worker;
        Factory.Work();
        UpdateGUI();
    }

    public void UpdateFactory(Factory f) {
        Factory.ui = null;
        f.ui = this;
        Factory = f;
        UpdateGUI();
    }

    [System.Serializable] public struct WorkerStatsUI {
        public Slider bar;
        public Text titleText;
        public Text percentage;
        public string title;
    }
}
