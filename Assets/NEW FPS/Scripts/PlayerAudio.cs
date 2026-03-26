using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioSource audioSource;

    private float stepTimer;
    private float stepInterval = 0.5f;

    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayStepSound()
    {
        stepTimer += Time.deltaTime;
        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f;
            if (stepSounds.Length > 0 && audioSource != null)
            {
                audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)], 0.5f);
            }
        }
    }

    public void PlayJumpSound()
    {
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound, 0.7f);
        }
    }

    public void PlayLandSound()
    {
        if (landSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(landSound, 0.8f);
        }
    }
}