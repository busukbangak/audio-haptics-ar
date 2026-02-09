
using UnityEngine;

[CreateAssetMenu(menuName = "Experiment/Condition")]
public class Condition : ScriptableObject
{
    public string Name;
    public string DisplayName;
    public int NumberOfTrials;
    public AudioClip[] CubeAudioClips;
}