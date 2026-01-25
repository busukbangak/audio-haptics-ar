using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class StudyStep
{
    public string Name;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;
}


public class StudyManager : MonoBehaviour
{
    public static StudyManager Instance { get; private set; }

    public int SubjectID = 0;
    public int CurrentStepIndex = 0;
    public List<StudyStep> Steps;

    private StudyStep _currentStep => Steps[CurrentStepIndex];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        EnterStep(CurrentStepIndex);
        Debug.Log($"Study started with step: {_currentStep.Name}");
    }

    public void GoToStep(int index)
    {
        ExitCurrentStep();
        CurrentStepIndex = index;
        EnterStep(CurrentStepIndex);
    }

    public void NextStep()
    {
        ExitCurrentStep();

        CurrentStepIndex++;

        if (CurrentStepIndex >= Steps.Count)
        {
            Debug.Log("Study finished");
            return;
        }

        EnterStep(CurrentStepIndex);
    }

    public void PreviousStep()
    {
        if (CurrentStepIndex <= 0)
        {
            Debug.Log("Already at first step");
            return;
        }

        ExitCurrentStep();

        CurrentStepIndex--;

        EnterStep(CurrentStepIndex);
    }

    void EnterStep(int index)
    {
        Debug.Log($"Enter Step: {Steps[index].Name}");
        Steps[index].OnEnter?.Invoke();
    }

    void ExitCurrentStep()
    {
        Debug.Log($"Exit Step: {_currentStep.Name}");
        _currentStep.OnExit?.Invoke();
    }
}
