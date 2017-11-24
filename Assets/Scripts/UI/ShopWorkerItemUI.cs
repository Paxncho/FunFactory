using UnityEngine.UI;

public class ShopWorkerItemUI : ShopItemUI {

    public int MaximumTalent;

    public Slider Stat1;
    public Text Stat1Text;
    public string Stat1Title;
    public Slider Stat2;
    public Text Stat2Text;
    public string Stat2Title;

    void Start() {
        Stat1.interactable = false;
        Stat2.interactable = false;

        Stat1.maxValue = MaximumTalent;
    }

    public override void UpdateUI() {
        Stat1.maxValue = MaximumTalent;
        base.UpdateUI();

        Worker worker = ((Worker.OfferingWorker)item).worker;

        Stat1.value = worker.talent;
        Stat2.value = worker.motivation;

        Stat1Text.text = Stat1Title + ": " + worker.talent;
        Stat2Text.text = Stat2Title + ": " + (worker.motivation * 100f) + "%";
    }

    public override void Buy() {
        Shop.Instance.HireWorker(item.Code);
    }
}
