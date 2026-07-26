using UnityEngine;

public class DelayedRepeatAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public float delay = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(PlayAudio), delay, delay);
    }

    private void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}