using Oculus.Interaction;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(InteractableUnityEventWrapper))]
public class Cube : MonoBehaviour
{
    private AudioSource _audioSource;

    private InteractableUnityEventWrapper _interactableWrapper;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _interactableWrapper = GetComponent<InteractableUnityEventWrapper>();
        _interactableWrapper.WhenSelect.AddListener(() => _audioSource.Play());
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
