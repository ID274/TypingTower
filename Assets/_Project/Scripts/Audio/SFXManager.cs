using System.Collections;
using System.Collections.Generic;
using PatternLibrary;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : Singleton<SFXManager>
{
    [SerializeField] private AudioClip[] sfxClips;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(int index, bool randomPitch)
    {
        if (randomPitch)
        {
            audioSource.pitch = Random.Range(0.9f, 1.2f);
        }
        else
        {
            audioSource.pitch = 1;
        }

        audioSource.PlayOneShot(sfxClips[index]);
    }

    public void PlaySFX(int index, bool randomPitch, bool fadeOut)
    {
        if (randomPitch)
        {
            audioSource.pitch = Random.Range(0.9f, 1.2f);
        }
        else
        {
            audioSource.pitch = 1;
        }

        audioSource.clip = sfxClips[index];
        audioSource.volume = 1; // Ensure volume starts at max
        audioSource.Play();

        if (fadeOut)
        {
            StartCoroutine(FadeOutSFX(audioSource, 5f)); // 2 seconds fade-out duration
        }
    }

    private IEnumerator FadeOutSFX(AudioSource source, float fadeDuration)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * (Time.deltaTime / fadeDuration);
            yield return null; // Wait for the next frame
        }

        source.Stop();
        source.volume = startVolume; // Reset volume for the next use
    }

    public void StopAllSFX()
    {
        audioSource.enabled = false;
        audioSource.enabled = true;
    }
}
