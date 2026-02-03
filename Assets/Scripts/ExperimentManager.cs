using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }

    public List<Condition> Conditions;
    public List<Cube> Cubes;

    public UnityEvent OnExperimentStart;
    public UnityEvent OnExperimentEnd;

    public UnityEvent<string> OnConditionStart;
    public UnityEvent<string> OnConditionEnd;

    public UnityEvent<int> OnTrialStart;
    public UnityEvent<int> OnTrialEnd;

    private List<Condition> _orderedConditions;
    private int _currentConditionIndex;
    private int _currentTrialIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartExperiment(int subjectID = -1)
    {
        // If subjectID is -1, use the one from StudyManager
        if (subjectID == -1)
        {
            subjectID = StudyManager.Instance.SubjectID;
        }

        GenerateConditionOrder(subjectID);

        _currentConditionIndex = 0;
        _currentTrialIndex = 0;

        Debug.Log("Experiment started");
        OnExperimentStart?.Invoke();

        StartCurrentTrial();
    }

    public void StopExperiment()
    {
        Debug.Log("Experiment stopped");
        OnExperimentEnd?.Invoke();
    }

    void GenerateConditionOrder(int subjectID)
    {
        _orderedConditions = new List<Condition>(Conditions);

        int offset = subjectID % Conditions.Count;

        for (int i = 0; i < offset; i++)
        {
            Condition first = _orderedConditions[0];
            _orderedConditions.RemoveAt(0);
            _orderedConditions.Add(first);
        }
    }

    void StartCurrentTrial()
    {
        Condition condition = _orderedConditions[_currentConditionIndex];

        Debug.Log($"Condition: {condition.Name}, Trial: {_currentTrialIndex}");

        // Invoke only on first trial of the condition
        if (_currentTrialIndex == 0) OnConditionStart?.Invoke(condition.Name);

        ConfigureCubes(condition);

        OnTrialStart?.Invoke(_currentTrialIndex);
    }

    public void EndTrial()
    {
        OnTrialEnd?.Invoke(_currentTrialIndex);

        _currentTrialIndex++;

        Condition condition = _orderedConditions[_currentConditionIndex];

        if (_currentTrialIndex >= condition.NumberOfTrials)
        {
            OnConditionEnd?.Invoke(condition.Name); // Current condition ended

            _currentTrialIndex = 0; // Reset for next condition
            _currentConditionIndex++; // Move to next condition

            if (_currentConditionIndex >= _orderedConditions.Count)
            {
                StopExperiment();
                return;
            }
        }

        StartCurrentTrial();
    }

    public void ConfigureCubes(Condition condition)
    {
        if (condition.CubeAudioClips.Length != Cubes.Count)
        {
            Debug.LogError($"Mismatch: Condition '{condition.Name}' has {condition.CubeAudioClips.Length} audio clips but there are {Cubes.Count} cubes. They should match!");
            return;
        }

        for (int i = 0; i < Cubes.Count; i++)
        {
            AudioClip clip = condition.CubeAudioClips[i];
            if (!Cubes[i].IsInitialized) Cubes[i].Initialize();
            Cubes[i].SetCollisionSound(clip);
        }
    }

}
