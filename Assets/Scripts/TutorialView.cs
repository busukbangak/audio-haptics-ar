
public class TutorialView : View
{

    public override void Show()
    {
        base.Show();
        DataManager.Instance.Log("tutorialStartTimestamp", System.DateTime.Now.ToString("o"));
    }

    public override void Hide()
    {
        base.Hide();
        DataManager.Instance.Log("tutorialEndTimestamp", System.DateTime.Now.ToString("o"));
    }
}
