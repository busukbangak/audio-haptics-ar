
using System.Linq;
using UnityEngine.UI;

public class QuestionnaireView : View
{
    public ToggleGroup PerceivedWeightToggleGroup;

    public ToggleGroup RealismToggleGroup;

    public ToggleGroup DifferencesToggleGroup;

    public ToggleGroup CertaintyToggleGroup;

    public ToggleGroup IntuitionToggleGroup;

    public Toggle PerceivedWeightContinueButton;

    public Toggle RealismContinueButton;

    public Toggle DifferencesContinueButton;

    public Toggle CertaintyContinueButton;

    public Toggle IntuitionContinueButton;

    public void Awake()
    {
        ResetToggleGroups();
    }

    public void Update()
    {
        ValidateQuestionnaireContinueButtons();
    }

    public void ResetToggleGroups()
    {
        PerceivedWeightToggleGroup.SetAllTogglesOff();
        RealismToggleGroup.SetAllTogglesOff();
        DifferencesToggleGroup.SetAllTogglesOff();
        CertaintyToggleGroup.SetAllTogglesOff();
        IntuitionToggleGroup.SetAllTogglesOff();
    }

    private void ValidateQuestionnaireContinueButtons()
    {
        bool isPerceivedWeightSelected = PerceivedWeightToggleGroup.ActiveToggles().Any();
        bool isRealismSelected = RealismToggleGroup.ActiveToggles().Any();
        bool isDifferencesSelected = DifferencesToggleGroup.ActiveToggles().Any();
        bool isCertaintySelected = CertaintyToggleGroup.ActiveToggles().Any();
        bool isIntuitionSelected = IntuitionToggleGroup.ActiveToggles().Any();

        PerceivedWeightContinueButton.interactable = isPerceivedWeightSelected;
        RealismContinueButton.interactable = isRealismSelected;
        DifferencesContinueButton.interactable = isDifferencesSelected;
        CertaintyContinueButton.interactable = isCertaintySelected;
        IntuitionContinueButton.interactable = isIntuitionSelected;
    }

    public void OnPerceivedWeightContinueButtonPressed()
    {
        string conditionName = ExperimentManager.Instance.GetCurrentConditionName();
        int trialIndex = ExperimentManager.Instance.GetCurrentTrialIndex();
        string logKey = $"perceivedWeight_{conditionName}_{trialIndex}";
        string value = PerceivedWeightToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }

    public void OnRealismContinueButtonPressed()
    {
        string conditionName = ExperimentManager.Instance.GetCurrentConditionName();
        int trialIndex = ExperimentManager.Instance.GetCurrentTrialIndex();
        string logKey = $"realism_{conditionName}_{trialIndex}";
        string value = RealismToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }

    public void OnDifferencesContinueButtonPressed()
    {
        string conditionName = ExperimentManager.Instance.GetCurrentConditionName();
        int trialIndex = ExperimentManager.Instance.GetCurrentTrialIndex();
        string logKey = $"differences_{conditionName}_{trialIndex}";
        string value = DifferencesToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }

    public void OnCertaintyContinueButtonPressed()
    {
        string conditionName = ExperimentManager.Instance.GetCurrentConditionName();
        int trialIndex = ExperimentManager.Instance.GetCurrentTrialIndex();
        string logKey = $"certainty_{conditionName}_{trialIndex}";
        string value = CertaintyToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }

    public void OnIntuitionContinueButtonPressed()
    {
        string conditionName = ExperimentManager.Instance.GetCurrentConditionName();
        int trialIndex = ExperimentManager.Instance.GetCurrentTrialIndex();
        string logKey = $"intuition_{conditionName}_{trialIndex}";
        string value = IntuitionToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }
}
