
public class ExperimentView : View
{
    public TMPro.TextMeshProUGUI RoundTitleText;

    public void UpdateRoundTitleText()
    {
        RoundTitleText.text = "Runde " + (ExperimentManager.Instance.GetCurrentTrialRoundSummedUp() + 1);
    }

    public void OnCubeOrderConfirmed()
    {
        DataManager.Instance.Log("cubeOrderConfirmedTimestamp", System.DateTime.Now.ToString("o"));
    }
}
