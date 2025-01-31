using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] Sound[] musicSounds, sfxSounds;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one AudioManager");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("AudioManager Created.");
    }

    private void Start()
    {
        PlayMusic("Theme");
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, s => s.name == name);
        if (sound == null)
        {
            Debug.Log("Sound not found!");
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);
        if(sound == null)
        {
            Debug.Log("Sound not found!");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    public void PlayClickSound()
    {
        PlaySFX("Click");
    }

    public void MasterVolume(float volume)
    {
        volume = -80 + 80 * volume;
        audioMixer.SetFloat("masterVol", volume);
    }
    public void MusicVolume(float volume)
    {
        volume = -80 + 80 * volume;
        audioMixer.SetFloat("musicVol", volume);
    }
    public void SFXVolume(float volume)
    {
        volume = -80 + 80 * volume;
        audioMixer.SetFloat("sfxVol", volume);
    }
}
