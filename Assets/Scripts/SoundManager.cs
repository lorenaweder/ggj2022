using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour {

    [Header("Background Music")]
    public AudioClip MusicBackground;
    public AudioClip[] MusicCatAlive;
    public AudioClip[] MusicCatDeath;

    [Header("Sound Effects")]
    public AudioClip[] CatDamaged;
    public AudioClip[] CatDied;
    public AudioClip[] CatDiedMouseLaughing;
    public AudioClip[] MouseDied;

    [Header("Sources")]
    public AudioSource BackgroundMusicSource;
    public AudioSource EffectMusicSource;
    public AudioSource EffectSource;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            PlayMusic(BackgroundMusicSource, MusicBackground, true);
        }
    }

    public void PlayEffectMusic(string clipName, bool loop) {
        switch (clipName) {
            case "MusicCatAlive":
                PlayMusic(EffectMusicSource, MusicCatAlive[UnityEngine.Random.Range(0, MusicCatAlive.Length)], true);
                break;
            case "MusicCatDeath":
                PlayMusic(EffectMusicSource, MusicCatDeath[UnityEngine.Random.Range(0, MusicCatDeath.Length)], true);
                break;
             default:
                return;
        }
    }

    public void PlayMusic(AudioSource source, AudioClip clip, bool loop) {
        if (source.isPlaying) {
            source.Stop();
        }
        source.clip = clip;
        source.loop = loop;
        source.Play();
    }

    public void PlayEffect(string clipName) {
        switch (clipName) {
            // Cat
            case "CatDamaged":
                EffectSource.PlayOneShot(CatDamaged[UnityEngine.Random.Range(0, CatDamaged.Length)]);
                break;
            case "CatDied":
                EffectSource.PlayOneShot(CatDied[UnityEngine.Random.Range(0, CatDied.Length)]);
                break;
            // Mouse
            case "MouseDied":
                EffectSource.PlayOneShot(MouseDied[UnityEngine.Random.Range(0, MouseDied.Length)]);
                break;
            case "CatDiedMouseLaughing":
                EffectSource.PlayOneShot(CatDiedMouseLaughing[UnityEngine.Random.Range(0, CatDiedMouseLaughing.Length)]);
                break;
             default:
                return;
        }
    }

    public static SoundManager Instance { get { return instance; } }
    private static SoundManager instance;
}