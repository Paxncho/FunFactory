using UnityEngine.UI;

public class InventoryWorkerUI : InventoryItemUI{
    public int MaximumTalent;

    public Slider Stat1;
    public Text Stat1Text;
    public string Stat1Title;
    public Slider Stat2;
    public Text Stat2Text;
    public string Stat2Title;
    public Slider Stat3;
    public Text Stat3Text;
    public string Stat3Title;

    void Start() {
        Stat1.interactable = false;
        Stat2.interactable = false;
        Stat3.interactable = false;

        Stat1.maxValue = MaximumTalent;
    }

    public override void UpdateUI() {
        Stat1.maxValue = MaximumTalent;

        base.UpdateUI();

        Worker worker = ((Worker.HiredWorker)item).worker;

        Stat1.value = worker.talent;
        Stat2.value = worker.motivation;
        Stat3.value = worker.tired;

        Stat1Text.text = Stat1Title + ": " + worker.talent;
        Stat2Text.text = Stat2Title + ": " + (worker.motivation * 100f) + "%";
        Stat3Text.text = Stat3Title + ": " + (worker.tired * 100f) + "%";
    }
}
