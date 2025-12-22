using UnityEngine;

public class CollisionSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();
    }
}
