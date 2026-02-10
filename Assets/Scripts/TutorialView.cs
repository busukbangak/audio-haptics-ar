
public class TutorialView : View
{

    public override void Show()
    {
        if (gameObject.activeSelf) return;
        base.Show();
        DataManager.Instance.Log("tutorialStartTimestamp", System.DateTime.Now.ToString("o"));
    }

    public override void Hide()
    {
        if (!gameObject.activeSelf) return;
        base.Hide();
        DataManager.Instance.Log("tutorialEndTimestamp", System.DateTime.Now.ToString("o"));
    }
}
