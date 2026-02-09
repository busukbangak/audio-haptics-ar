
using System.Linq;
using UnityEngine.UI;

public class PostQuestionnaireView : View
{
    public ToggleGroup MappingPitchToggleGroup;

    public ToggleGroup MappingLoudnessToggleGroup;

    public ToggleGroup MappingTimbreToggleGroup;

    public ToggleGroup UsefulnessPitchToggleGroup;

    public ToggleGroup UsefulnessLoudnessToggleGroup;

    public ToggleGroup UsefulnessTimbreToggleGroup;

    public ToggleGroup PreferenceToggleGroup;

    public Toggle MappingContinueButton;

    public Toggle UsefulnessContinueButton;

    public Toggle PreferenceContinueButton;

    public TMPro.TextMeshProUGUI MappingConditionHintText;

    public TMPro.TextMeshProUGUI UsefulnessConditionHintText;

    public TMPro.TextMeshProUGUI PreferenceConditionHintText;

    public void Awake()
    {
        ResetToggleGroups();
    }

    public void Update()
    {
        ValidatePostQuestionnaireContinueButtons();
    }

    public void UpdateConditionHintText()
    {
        string conditionRangeText = ExperimentManager.Instance.GetConditionRangeText();

        MappingConditionHintText.text = conditionRangeText;
        UsefulnessConditionHintText.text = conditionRangeText;
        PreferenceConditionHintText.text = conditionRangeText;
    }

    public void ResetToggleGroups()
    {
        MappingPitchToggleGroup.SetAllTogglesOff();
        MappingLoudnessToggleGroup.SetAllTogglesOff();
        MappingTimbreToggleGroup.SetAllTogglesOff();
        UsefulnessPitchToggleGroup.SetAllTogglesOff();
        UsefulnessLoudnessToggleGroup.SetAllTogglesOff();
        UsefulnessTimbreToggleGroup.SetAllTogglesOff();
        PreferenceToggleGroup.SetAllTogglesOff();
    }

    private void ValidatePostQuestionnaireContinueButtons()
    {
        bool isMappingPitchSelected = MappingPitchToggleGroup.ActiveToggles().Any();
        bool isMappingLoudnessSelected = MappingLoudnessToggleGroup.ActiveToggles().Any();
        bool isMappingTimbreSelected = MappingTimbreToggleGroup.ActiveToggles().Any();
        bool isUsefulnessPitchSelected = UsefulnessPitchToggleGroup.ActiveToggles().Any();
        bool isUsefulnessLoudnessSelected = UsefulnessLoudnessToggleGroup.ActiveToggles().Any();
        bool isUsefulnessTimbreSelected = UsefulnessTimbreToggleGroup.ActiveToggles().Any();
        bool isPreferenceSelected = PreferenceToggleGroup.ActiveToggles().Any();
        MappingContinueButton.interactable = isMappingPitchSelected && isMappingLoudnessSelected && isMappingTimbreSelected;
        UsefulnessContinueButton.interactable = isUsefulnessPitchSelected && isUsefulnessLoudnessSelected && isUsefulnessTimbreSelected;
        PreferenceContinueButton.interactable = isPreferenceSelected;
    }

    public void OnMappingContinueButtonPressed()
    {
        string logKey = "mapping_pitch";
        string value = MappingPitchToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);

        logKey = "mapping_loudness";
        value = MappingLoudnessToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);

        logKey = "mapping_timbre";
        value = MappingTimbreToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }

    public void OnUsefulnessContinueButtonPressed()
    {
        string logKey = "usefulness_pitch";
        string value = UsefulnessPitchToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);

        logKey = "usefulness_loudness";
        value = UsefulnessLoudnessToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);

        logKey = "usefulness_timbre";
        value = UsefulnessTimbreToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }

    public void OnPreferenceContinueButtonPressed()
    {
        string logKey = "preference";
        string value = PreferenceToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }
}
