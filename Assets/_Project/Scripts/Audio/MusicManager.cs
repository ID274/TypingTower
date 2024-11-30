using PatternLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioClip[] musicClips;
    private AudioSource audioSource;

    [Header("Play when scene loaded")]
    [SerializeField] private bool playOnLoad = true;
    [SerializeField] private int indexToPlayOnLoad = 0;
    [SerializeField] private bool loop = true; // flag to indicate if the music should loop

    private bool isFading; // flag to indicate if a fade operation is in progress
    [SerializeField] private float fadeDuration = 1.0f;


    private void FixedUpdate()
    {
        audioSource.pitch = 0.5f + Time.timeScale / 2;
    }

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (playOnLoad)
        {
            PlayMusic(indexToPlayOnLoad, true);
        }
    }

    public void PlayMusic(int index, bool loop)
    {
        audioSource.clip = musicClips[index];
        audioSource.Play();

        if (loop)
        {
            audioSource.loop = true;
        }
        else
        {
            audioSource.loop = false;
        }
    }

    public void PlayMusicFadeOut(int index)
    {
        StartCoroutine(FadeOutAndPlay(index));
    }

    public IEnumerator FadeOutCurrentTrack()
    {
        float startVolume = audioSource.volume; // store the initial volume
        // gradually reduce the volume to zero
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null; // wait for the next frame
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeOutAndPlay(int index)
    {
        float startVolume = audioSource.volume; // store the initial volume
        // gradually reduce the volume to zero
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null; // wait for the next frame
        }

        PlayMusic(index, false);

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null; // wait for the next frame
        }
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.UnPause();
    }

    public void Stop()
    {
        audioSource.Stop();
    }


}
