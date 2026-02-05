
using System.Linq;
using UnityEngine.UI;

public class PostQuestionnaireView : View
{
    public ToggleGroup MappingToggleGroup;

    public ToggleGroup UsefulnessToggleGroup;

    public ToggleGroup PreferenceToggleGroup;

    public Toggle MappingContinueButton;

    public Toggle UsefulnessContinueButton;

    public Toggle PreferenceContinueButton;

    public void Awake()
    {
        ResetToggleGroups();
    }

    public void Update()
    {
        ValidatePostQuestionnaireContinueButtons();
    }

    public void ResetToggleGroups()
    {
        MappingToggleGroup.SetAllTogglesOff();
        UsefulnessToggleGroup.SetAllTogglesOff();
        PreferenceToggleGroup.SetAllTogglesOff();
    }

    private void ValidatePostQuestionnaireContinueButtons()
    {
        bool isMappingSelected = MappingToggleGroup.ActiveToggles().Any();
        bool isUsefulnessSelected = UsefulnessToggleGroup.ActiveToggles().Any();
        bool isPreferenceSelected = PreferenceToggleGroup.ActiveToggles().Any();
        MappingContinueButton.interactable = isMappingSelected;
        UsefulnessContinueButton.interactable = isUsefulnessSelected;
        PreferenceContinueButton.interactable = isPreferenceSelected;
    }

    public void OnMappingContinueButtonPressed()
    {
        string logKey = "mapping";
        string value = MappingToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }

    public void OnUsefulnessContinueButtonPressed()
    {
        string logKey = "usefulness";
        string value = UsefulnessToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }

    public void OnPreferenceContinueButtonPressed()
    {
        string logKey = "preference";
        string value = PreferenceToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }
}
