using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Cube : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
