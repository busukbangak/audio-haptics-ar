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

        LogConditionOrder();
    }

    public void LogConditionOrder()
    {
        List<string> conditionNames = new List<string>();

        foreach (var condition in _orderedConditions)
        {
            conditionNames.Add(condition.Name);
        }

        DataManager.Instance.Log("conditionOrder", string.Join(">", conditionNames));
    }

    void StartCurrentTrial()
    {
        Condition condition = _orderedConditions[_currentConditionIndex];

        Debug.Log($"Condition: {condition.Name}, Trial: {_currentTrialIndex}");

        // Invoke only on first trial of the condition
        if (_currentTrialIndex == 0) OnConditionStart?.Invoke(condition.Name);

        ConfigureCubes(condition);

        OnTrialStart?.Invoke(_currentTrialIndex);

        DataManager.Instance.Log($"trialStartTimestamp_{GetCurrentConditionStringIndex()}_{GetCurrentTrialStringIndex()}", System.DateTime.Now.ToString("o"));

        // Log unsorted positions (initial state after shuffle)
        LogCubesUnsortedPositions();
    }

    public void EndTrial()
    {
        OnTrialEnd?.Invoke(_currentTrialIndex);
        DataManager.Instance.Log($"trialEndTimestamp_{GetCurrentConditionStringIndex()}_{GetCurrentTrialStringIndex()}", System.DateTime.Now.ToString("o"));


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

        // Shuffle the audio clips before assigning
        AudioClip[] shuffledClips = ShuffleArray(condition.CubeAudioClips);

        for (int i = 0; i < Cubes.Count; i++)
        {
            AudioClip clip = shuffledClips[i];
            if (!Cubes[i].IsInitialized) Cubes[i].Initialize();
            Cubes[i].ResetPosition();
            Cubes[i].SetCollisionSound(clip);
        }
    }

    public void LogCubesUnsortedPositions()
    {
        // Sort cubes by their x-position (left to right) to get their initial spatial order
        var cubesByPosition = Cubes.OrderBy(cube => cube.transform.position.x).ToList();

        for (int i = 0; i < cubesByPosition.Count; i++)
        {
            string clipName = cubesByPosition[i].GetCurrentAudioClipName();

            string logKey = $"cubesUnsortedPositions_{GetCurrentConditionStringIndex()}_{GetCurrentTrialStringIndex()}_{i + 1}";
            string logValue = clipName;

            DataManager.Instance.Log(logKey, logValue);
        }
    }

    public void LogCubesSortedPositions()
    {
        // Sort cubes by their x-position (left to right)
        var sortedCubes = Cubes.OrderBy(cube => cube.transform.position.x).ToList();

        for (int i = 0; i < sortedCubes.Count; i++)
        {
            string clipName = sortedCubes[i].GetCurrentAudioClipName();

            string logKey = $"cubesSortedPositions_{GetCurrentConditionStringIndex()}_{GetCurrentTrialStringIndex()}_{i + 1}";
            string logValue = clipName;

            DataManager.Instance.Log(logKey, logValue);
        }
    }

    public void LogCubesSortedTimestamp()
    {
        DataManager.Instance.Log($"cubesSortedTimestamp_{GetCurrentConditionStringIndex()}_{GetCurrentTrialStringIndex()}", System.DateTime.Now.ToString("o"));
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

    public string GetCurrentConditionStringIndex()
    {
        return "C" + (_currentConditionIndex + 1);
    }

    public string GetCurrentTrialStringIndex()
    {
        return "T" + (_currentTrialIndex + 1);
    }

    public string GetConditionRangeText()
    {
        int roundCounter = 1;
        List<string> conditionRanges = new List<string>();

        foreach (var condition in _orderedConditions)
        {
            int startRound = roundCounter;
            int endRound = roundCounter + condition.NumberOfTrials - 1;

            string rangeText;
            if (condition.NumberOfTrials == 1)
            {
                rangeText = $"Runde {startRound} {condition.DisplayName}";
            }
            else
            {
                rangeText = $"Runde {startRound}-{endRound} {condition.DisplayName}";
            }

            conditionRanges.Add(rangeText);
            roundCounter += condition.NumberOfTrials;
        }

        return string.Join(" > ", conditionRanges);
    }

    public T[] ShuffleArray<T>(T[] array)
    {
        System.Random random = new System.Random();
        return array.OrderBy(x => random.Next()).ToArray();
    }

}
