using Oculus.Interaction;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(InteractableUnityEventWrapper))]
public class Cube : MonoBehaviour
{
    private AudioSource _audioSource;

    private InteractableUnityEventWrapper _interactableWrapper;

    public bool IsInitialized { get; private set; } = false;

    public void Initialize()
    {
        _audioSource = GetComponent<AudioSource>();
        _interactableWrapper = GetComponent<InteractableUnityEventWrapper>();
        _interactableWrapper.WhenSelect.AddListener(() => _audioSource.Play());
        IsInitialized = true;
    }

    public void SetCollisionSound(AudioClip clip)
    {
        _audioSource.clip = clip;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _audioSource.Play();
    }
}
