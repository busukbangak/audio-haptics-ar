using Oculus.Interaction;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(InteractableUnityEventWrapper), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private AudioSource _audioSource;

    private InteractableUnityEventWrapper _interactableWrapper;

    private Vector3 _initialPosition;

    private Quaternion _initialRotation;

    private Rigidbody _rigidbody;

    public bool IsInitialized { get; private set; } = false;

    public void Initialize()
    {
        _audioSource = GetComponent<AudioSource>();
        _interactableWrapper = GetComponent<InteractableUnityEventWrapper>();
        _rigidbody = GetComponent<Rigidbody>();
        _interactableWrapper.WhenSelect.AddListener(() => _audioSource.Play());

        // Store initial transform
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;

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

    public void ResetPosition()
    {
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RespawnTrigger"))
        {
            ResetPosition();
        }
    }
}
