using System.Linq;
using TMPro;
using UnityEngine.UI;

public class PreparationView : View
{
    public TextMeshProUGUI SubjectID;

    public TextMeshProUGUI Age;

    public ToggleGroup GenderToggleGroup;

    public ToggleGroup MusicExperienceToggleGroup;

    public ToggleGroup XRExperienceToggleGroup;

    public Toggle GeneralDataStepContinueButton;

    public Toggle GeneralQuestionsStepContinueButton;

    public void Start()
    {
        GenderToggleGroup.SetAllTogglesOff();
        MusicExperienceToggleGroup.SetAllTogglesOff();
        XRExperienceToggleGroup.SetAllTogglesOff();
    }

    private void Update()
    {
        ValidateGeneralDataStepContinueButtons();
    }

    private void ValidateGeneralDataStepContinueButtons()
    {
        bool isIdValid = int.TryParse(SubjectID.text, out int id) && id != 0;
        bool isAgeValid = int.TryParse(Age.text, out int age) && age != 0;
        bool isGenderSelected = GenderToggleGroup.ActiveToggles().Any();
        bool isMusicExpSelected = MusicExperienceToggleGroup.ActiveToggles().Any();
        bool isXRExpSelected = XRExperienceToggleGroup.ActiveToggles().Any();

        GeneralDataStepContinueButton.interactable = isIdValid && isAgeValid && isGenderSelected;
        GeneralQuestionsStepContinueButton.interactable = isMusicExpSelected && isXRExpSelected;
    }

    public void OnConsentStepContinueButtonPressed()
    {
        DataManager.Instance.Log("isConsentGiven", "true");
        DataManager.Instance.Log("givenConsentTimestamp", System.DateTime.Now.ToString("o"));
    }

    public void OnGeneralDataStepContinueButtonPressed()
    {
        DataManager.Instance.Log("subjectId", SubjectID.text);
        DataManager.Instance.Log("age", Age.text);
        DataManager.Instance.Log("gender", GenderToggleGroup.ActiveToggles().FirstOrDefault().name);
        DataManager.Instance.Log("generalDataStepTimestamp", System.DateTime.Now.ToString("o"));

        StudyManager.Instance.SubjectID = int.Parse(SubjectID.text);
    }

    public void OnGeneralQuestionsStepContinueButtonPressed()
    {
        DataManager.Instance.Log("musicExperience", MusicExperienceToggleGroup.ActiveToggles().FirstOrDefault().name);
        DataManager.Instance.Log("xrExperience", XRExperienceToggleGroup.ActiveToggles().FirstOrDefault().name);
        DataManager.Instance.Log("generalQuestionsStepTimestamp", System.DateTime.Now.ToString("o"));
        DataManager.Instance.ExportData();
    }

    public void IncreaseSubjectId()
    {
        SubjectID.text = (int.Parse(SubjectID.text) + 1).ToString();
    }
    public void DecreaseSubjectId()
    {
        int currentId = int.Parse(SubjectID.text);
        if (currentId > 0) SubjectID.text = (currentId - 1).ToString();
    }

    public void IncreaseAge()
    {
        Age.text = (int.Parse(Age.text) + 1).ToString();
    }

    public void DecreaseAge()
    {
        int currentAge = int.Parse(Age.text);
        if (currentAge > 0) Age.text = (currentAge - 1).ToString();
    }
}
