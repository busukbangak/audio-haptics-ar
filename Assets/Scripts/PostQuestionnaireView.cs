
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PostQuestionnaireView : View
{
    // Mapping Toggle Groups
    public Transform MappingToggleGroupsContainer;
    public ToggleGroup MappingPitchToggleGroup;
    public ToggleGroup MappingLoudnessToggleGroup;
    public ToggleGroup MappingTimbreToggleGroup;

    // Usefulness Toggle Groups
    public Transform UsefulnessToggleGroupsContainer;
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

    private Dictionary<string, ToggleGroup> _mappingToggleGroups;
    private Dictionary<string, ToggleGroup> _usefulnessToggleGroups;
    private List<string> _orderedConditionNames;

    public void Awake()
    {
        _mappingToggleGroups = new Dictionary<string, ToggleGroup>
        {
            { "Pitch", MappingPitchToggleGroup },
            { "Loudness", MappingLoudnessToggleGroup },
            { "Timbre", MappingTimbreToggleGroup }
        };

        _usefulnessToggleGroups = new Dictionary<string, ToggleGroup>
        {
            { "Pitch", UsefulnessPitchToggleGroup },
            { "Loudness", UsefulnessLoudnessToggleGroup },
            { "Timbre", UsefulnessTimbreToggleGroup }
        };
    }

    public void Update()
    {
        ValidatePostQuestionnaireContinueButtons();
    }

    public void InitializePostQuestionnaire()
    {
        UpdateConditionOrder();
        UpdateConditionHintText();
        UpdateToggleGroupOrder();
        ResetToggleGroups();
    }

    public void UpdateConditionOrder()
    {
        var orderedConditions = ExperimentManager.Instance.GetOrderedConditions();
        _orderedConditionNames = orderedConditions.Select(c => c.Name).ToList();
    }

    public void UpdateConditionHintText()
    {
        string conditionRangeText = ExperimentManager.Instance.GetConditionRangeText();

        MappingConditionHintText.text = conditionRangeText;
        UsefulnessConditionHintText.text = conditionRangeText;
        PreferenceConditionHintText.text = conditionRangeText;
    }

    public void UpdateToggleGroupOrder()
    {
        var orderedMappingGroups = GetOrderedMappingToggleGroups();
        for (int i = 0; i < orderedMappingGroups.Count; i++)
        {
            orderedMappingGroups[i].transform.SetSiblingIndex(i);
        }

        var orderedUsefulnessGroups = GetOrderedUsefulnessToggleGroups();
        for (int i = 0; i < orderedUsefulnessGroups.Count; i++)
        {
            orderedUsefulnessGroups[i].transform.SetSiblingIndex(i);
        }
    }

    private List<ToggleGroup> GetOrderedMappingToggleGroups()
    {
        List<ToggleGroup> orderedGroups = new List<ToggleGroup>();
        foreach (string conditionName in _orderedConditionNames)
        {
            if (_mappingToggleGroups.ContainsKey(conditionName))
            {
                orderedGroups.Add(_mappingToggleGroups[conditionName]);
            }
        }
        return orderedGroups;
    }

    private List<ToggleGroup> GetOrderedUsefulnessToggleGroups()
    {
        List<ToggleGroup> orderedGroups = new List<ToggleGroup>();
        foreach (string conditionName in _orderedConditionNames)
        {
            if (_usefulnessToggleGroups.ContainsKey(conditionName))
            {
                orderedGroups.Add(_usefulnessToggleGroups[conditionName]);
            }
        }
        return orderedGroups;
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
        var orderedGroups = GetOrderedMappingToggleGroups();

        for (int i = 0; i < _orderedConditionNames.Count && i < orderedGroups.Count; i++)
        {
            string logKey = $"mapping_C{i + 1}";
            string value = orderedGroups[i].GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;

            DataManager.Instance.Log(logKey, value);

        }
    }

    public void OnUsefulnessContinueButtonPressed()
    {
        var orderedGroups = GetOrderedUsefulnessToggleGroups();

        for (int i = 0; i < _orderedConditionNames.Count && i < orderedGroups.Count; i++)
        {
            string logKey = $"usefulness_C{i + 1}";
            string value = orderedGroups[i].GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;

            DataManager.Instance.Log(logKey, value);
        }
    }

    public void OnPreferenceContinueButtonPressed()
    {
        string logKey = "preference";
        string value = PreferenceToggleGroup.GetComponentsInChildren<Toggle>().FirstOrDefault(t => t.isOn).name;
        DataManager.Instance.Log(logKey, value);
    }
}
