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

    public void StopAllSFX()
    {
        audioSource.enabled = false;
        audioSource.enabled = true;
    }
}
