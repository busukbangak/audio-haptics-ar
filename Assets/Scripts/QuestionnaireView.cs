
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireView : View
{
    public ToggleGroup PerceivedWeightToggleGroup;

    public ToggleGroup RealismToggleGroup;

    public ToggleGroup DifferencesToggleGroup;

    public ToggleGroup ConfidenceToggleGroup;

    public ToggleGroup IntuitionToggleGroup;

    public Toggle PerceivedWeightContinueButton;

    public Toggle RealismContinueButton;

    public Toggle DifferencesContinueButton;

    public Toggle ConfidenceContinueButton;

    public Toggle IntuitionContinueButton;

    private Dictionary<ToggleGroup, Toggle> _lastSelectedToggles = new Dictionary<ToggleGroup, Toggle>();


    public void Awake()
    {
        ResetToggleGroups();
    }

    public void Update()
    {
        ValidateQuestionnaireContinueButtons();
    }

    private void ResetToggleGroup(ToggleGroup toggleGroup)
    {
        foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false;
        }
    }

    public void ResetToggleGroups()
    {
        ResetToggleGroup(PerceivedWeightToggleGroup);
        ResetToggleGroup(RealismToggleGroup);
        ResetToggleGroup(DifferencesToggleGroup);
        ResetToggleGroup(ConfidenceToggleGroup);
        ResetToggleGroup(IntuitionToggleGroup);

        _lastSelectedToggles.Clear();
    }

    private void ValidateQuestionnaireContinueButtons()
    {
        bool isPerceivedWeightSelected = PerceivedWeightToggleGroup.ActiveToggles().Any();
        bool isRealismSelected = RealismToggleGroup.ActiveToggles().Any();
        bool isDifferencesSelected = DifferencesToggleGroup.ActiveToggles().Any();
        bool isConfidenceSelected = ConfidenceToggleGroup.ActiveToggles().Any();
        bool isIntuitionSelected = IntuitionToggleGroup.ActiveToggles().Any();

        PerceivedWeightContinueButton.interactable = isPerceivedWeightSelected;
        RealismContinueButton.interactable = isRealismSelected;
        DifferencesContinueButton.interactable = isDifferencesSelected;
        ConfidenceContinueButton.interactable = isConfidenceSelected;
        IntuitionContinueButton.interactable = isIntuitionSelected;
    }

    public void OnPerceivedWeightContinueButtonPressed()
    {
        string conditionStringIndex = ExperimentManager.Instance.GetCurrentConditionStringIndex();
        string trialStringIndex = ExperimentManager.Instance.GetCurrentTrialStringIndex();
        string logKey = $"perceivedWeight_{conditionStringIndex}_{trialStringIndex}";

        Toggle selectedToggle = PerceivedWeightToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn);
        DataManager.Instance.Log(logKey, selectedToggle.name);
        _lastSelectedToggles[PerceivedWeightToggleGroup] = selectedToggle;
        selectedToggle.isOn = false;
    }

    public void OnRealismContinueButtonPressed()
    {
        string conditionStringIndex = ExperimentManager.Instance.GetCurrentConditionStringIndex();
        string trialStringIndex = ExperimentManager.Instance.GetCurrentTrialStringIndex();
        string logKey = $"realism_{conditionStringIndex}_{trialStringIndex}";

        Toggle selectedToggle = RealismToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn);
        DataManager.Instance.Log(logKey, selectedToggle.name);
        _lastSelectedToggles[RealismToggleGroup] = selectedToggle;
        selectedToggle.isOn = false;
    }

    public void OnDifferencesContinueButtonPressed()
    {
        string conditionStringIndex = ExperimentManager.Instance.GetCurrentConditionStringIndex();
        string trialStringIndex = ExperimentManager.Instance.GetCurrentTrialStringIndex();
        string logKey = $"differences_{conditionStringIndex}_{trialStringIndex}";

        Toggle selectedToggle = DifferencesToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn);
        DataManager.Instance.Log(logKey, selectedToggle.name);
        _lastSelectedToggles[DifferencesToggleGroup] = selectedToggle;
        selectedToggle.isOn = false;
    }

    public void OnConfidenceContinueButtonPressed()
    {
        string conditionStringIndex = ExperimentManager.Instance.GetCurrentConditionStringIndex();
        string trialStringIndex = ExperimentManager.Instance.GetCurrentTrialStringIndex();
        string logKey = $"confidence_{conditionStringIndex}_{trialStringIndex}";

        Toggle selectedToggle = ConfidenceToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn);
        DataManager.Instance.Log(logKey, selectedToggle.name);
        _lastSelectedToggles[ConfidenceToggleGroup] = selectedToggle;
        selectedToggle.isOn = false;
    }

    public void OnIntuitionContinueButtonPressed()
    {
        string conditionStringIndex = ExperimentManager.Instance.GetCurrentConditionStringIndex();
        string trialStringIndex = ExperimentManager.Instance.GetCurrentTrialStringIndex();
        string logKey = $"intuition_{conditionStringIndex}_{trialStringIndex}";

        Toggle selectedToggle = IntuitionToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn);
        DataManager.Instance.Log(logKey, selectedToggle.name);
        _lastSelectedToggles[IntuitionToggleGroup] = selectedToggle;
        selectedToggle.isOn = false;
    }

    public void RestoreLastSelection(ToggleGroup toggleGroup)
    {
        if (_lastSelectedToggles.ContainsKey(toggleGroup))
        {
            _lastSelectedToggles[toggleGroup].isOn = true;
        }
    }

    public void OnBackToPerceivedWeight() => RestoreLastSelection(PerceivedWeightToggleGroup);
    public void OnBackToRealism() => RestoreLastSelection(RealismToggleGroup);
    public void OnBackToDifferences() => RestoreLastSelection(DifferencesToggleGroup);
    public void OnBackToConfidence() => RestoreLastSelection(ConfidenceToggleGroup);
    public void OnBackToIntuition() => RestoreLastSelection(IntuitionToggleGroup);
}
