
public class ExperimentView : View
{
    public TMPro.TextMeshProUGUI RoundTitleText;

    public void UpdateRoundTitleText()
    {
        RoundTitleText.text = "Runde " + (ExperimentManager.Instance.GetCurrentTrialRoundSummedUp() + 1);
    }
}
