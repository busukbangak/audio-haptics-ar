
using UnityEngine;

[CreateAssetMenu(menuName = "Experiment/Condition")]
public class Condition : ScriptableObject
{
    public string Name;
    public int NumberOfTrials;
    public AudioClip[] CubeAudioClips;
}