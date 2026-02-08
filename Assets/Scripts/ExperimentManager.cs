using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

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

    public void StartExperiment() => StartExperiment(StudyManager.Instance.SubjectID);

    public void StartExperiment(int subjectID)
    {
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

        DataManager.Instance.Log($"trialStartTimestamp_{GetCurrentConditionName()}_{GetCurrentTrialIndex()}", System.DateTime.Now.ToString("o"));
    }

    public void EndTrial()
    {
        OnTrialEnd?.Invoke(_currentTrialIndex);

        _currentTrialIndex++;

        Condition condition = _orderedConditions[_currentConditionIndex];

        if (_currentTrialIndex >= condition.NumberOfTrials)
        {
            OnConditionEnd?.Invoke(condition.Name); // Current condition ended
            DataManager.Instance.Log($"trialEndTimestamp_{GetCurrentConditionName()}_{GetCurrentTrialIndex()}", System.DateTime.Now.ToString("o"));

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

        // Shuffle the audio clips before assigning
        AudioClip[] shuffledClips = ShuffleArray(condition.CubeAudioClips);

        // Prepare volume levels if this is the Loudness condition
        float[] volumes = null;
        if (condition.Name == "Loudness")
        {
            volumes = ShuffleArray(new float[] { 1f, 0.7f, 0.5f, 0.35f, 0.2f });
        }

        for (int i = 0; i < Cubes.Count; i++)
        {
            AudioClip clip = shuffledClips[i];
            if (!Cubes[i].IsInitialized) Cubes[i].Initialize();
            Cubes[i].ResetPosition();
            Cubes[i].SetCollisionSound(clip);

            // Set volume for Loudness condition
            if (volumes != null)
            {
                Cubes[i].SetVolume(volumes[i]);
            }
        }
    }

    public int GetCurrentTrialRoundSummedUp()
    {
        // Sum trials from completed conditions + current trial
        int round = 0;
        for (int i = 0; i < _currentConditionIndex; i++)
        {
            round += _orderedConditions[i].NumberOfTrials;
        }
        return round + _currentTrialIndex;
    }

    public string GetCurrentConditionName()
    {
        return _orderedConditions[_currentConditionIndex].Name;
    }

    public int GetCurrentTrialIndex()
    {
        return _currentTrialIndex;
    }

    public void LogCubesSortedTimestamp()
    {
        DataManager.Instance.Log($"cubesSortedTimestamp_{GetCurrentConditionName()}_{GetCurrentTrialIndex()}", System.DateTime.Now.ToString("o"));
    }

    public T[] ShuffleArray<T>(T[] array)
    {
        System.Random random = new System.Random();
        return array.OrderBy(x => random.Next()).ToArray();
    }

}
