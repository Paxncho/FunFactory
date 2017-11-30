using UnityEngine;
using UnityEngine.UI;

public class WorkerStationUI : MonoBehaviour {

    public Factory factory;
    public GameObject stats;

    public WorkerStatsUI[] WorkerStats = new WorkerStatsUI[3];
    public Image WorkTable;
    public Image Worker;

    public UIManager.TimeBar time;
    public RecipeUI recipe;

    void Start() {
        WorkerStats[0].bar.interactable = false;
        WorkerStats[1].bar.interactable = false;
        WorkerStats[2].bar.interactable = false;

        if (factory != null) {
            UpdateGUI();
            factory.ui = this;
        }

        Recipes.Instance.actualUI = this;
    }

    public void UpdateGUI() {
        if (factory == null) {
            stats.SetActive(false);
            return;
        } else if (factory.worker == null) {
            stats.SetActive(false);
            UpdateTime();
            return;
        } else {
            stats.SetActive(true);
        }

        WorkerStats[0].titleText.text = WorkerStats[0].title;
        WorkerStats[1].titleText.text = WorkerStats[1].title;
        WorkerStats[2].titleText.text = WorkerStats[2].title;

        WorkerStats[0].bar.value = factory.worker.talent;
        WorkerStats[1].bar.value = factory.worker.motivation;
        WorkerStats[2].bar.value = factory.worker.tired;

        WorkerStats[0].percentage.text = factory.worker.talent.ToString();
        WorkerStats[1].percentage.text = (factory.worker.motivation * 100f) + "%";
        WorkerStats[2].percentage.text = (factory.worker.tired * 100f) + "%";

        Worker.sprite = factory.worker.sprite;  

        recipe.UpdateUI();
        UpdateTime();
    }

    public void UpdateTime() {
        int timeLeft = factory.TimeLeft();

        if (factory.working) {
            float percentage = 1.0f - (timeLeft * 1.0f / factory.secondsToCreate);

            time.bar.value = percentage;
            time.timeLeftText.text = timeLeft + "s Left";
        } else {
            time.timeLeftText.text = "Not Working";

        }
    }

    public void UpdateRecipe(string code) {
        factory.pieceToCreate = Recipes.Instance.pieces[code];
        recipe.item = factory.pieceToCreate.ToInventory();
        factory.Rest();
        factory.Work();
        UpdateGUI();
    }

    public void UpdateWorker(string code) {
        factory.worker = ((Worker.HiredWorker)Inventory.Instance.workers[code]).worker;
        factory.Work();
        UpdateGUI();
    }

    public void UpdateFactory(Factory f) {
        factory.ui = null;
        f.ui = this;
        factory = f;
        UpdateGUI();
    }

    [System.Serializable] public struct WorkerStatsUI {
        public Slider bar;
        public Text titleText;
        public Text percentage;
        public string title;
    }
}
