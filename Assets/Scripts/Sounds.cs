using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [Header("Music")]
    public AudioClip MainThemeWithIntro;
    public AudioClip MainThemeLoop;

    public AudioClip BossThemeWithIntro;
    public AudioClip BossThemeLoop;

    public AudioClip CreditsTheme;

    public AudioClip DarkTheme;
    [Header("Single play effects")]
    public AudioClip Effort;
    public AudioClip FireLoop;
    public AudioClip FireIntenseLoop;
    public AudioClip Steps;
    public AudioClip StepsArmor;
    public AudioClip MonsterTalk;
    public AudioClip EffortDown;
    public AudioClip[] ClientTalks;
    public AudioClip[] ClientHappy;
    public AudioClip[] ClientAngry;

    public AudioClip[] Coins;
    public AudioClip[] GoodHits;
    public AudioClip[] ColdHits;
    public AudioClip[] HotHits;
    public AudioClip[] MissedHits;

    // Use two Audio Sources to queue the intro with the loop
    public AudioSource IntroMusicAudioSource;

    internal void PlayEndScreenMusic()
    {
        LoopMusicAudioSource.Stop();
        MusicSource.Stop();
        PlayMusic(CreditsTheme, true);
    }

    public AudioSource LoopMusicAudioSource;

    public AudioSource MusicSource;

    private static readonly System.Random _random = new System.Random();
    private int _clientNumber = 0;

    public void PlayEffort()
    {
        PlayEffect(Effort);
    }
    public void PlayEffortDown()
    {
        PlayEffect(EffortDown);
    }
    public void PlayGoodHit()
    {
        PlayRandomEffect(GoodHits);
    }
    public void PlayColdHit()
    {
        PlayRandomEffect(ColdHits);
    }
    public void PlayHotHit()
    {
        PlayRandomEffect(HotHits);
    }
    public void PlayMissedHit()
    {
        PlayRandomEffect(MissedHits);
    }
    public void PlayFireLoop()
    {
        Debug.Log("Normal");
        PlayMusic(FireLoop, true);
    }
    public void PlayFireIntenseLoop()
    {
        Debug.Log("Intense");
        PlayEffect(FireIntenseLoop);
    }

    //Client number is set to be used in PlayClientHappy()/PlayClientSad()
    public void PlayClientTalks()
    {
        _clientNumber = PlayRandomEffect(ClientTalks);
    }
    public void PlayClientHappy()
    {
        PlayEffect(ClientHappy, _clientNumber);
    }
    public void PlayClientAngry()
    {
        PlayEffect(ClientAngry, _clientNumber);
    }
    public void PlayCoins()
    {
        PlayRandomEffect(Coins);
    }
    public void PlaySteps()
    {
        PlayEffect(Steps);
    }

    public void StopEffect()
    {
        effectsSource.Stop();
    }
    public void PlayStepsArmor()
    {
        PlayEffect(StepsArmor);
    }
    public void PlayMonsterTalk()
    {
        PlayEffect(MonsterTalk);
    }

    public static SoundManager Instance { get { return instance; } }
    private static SoundManager instance;
    public AudioSource effectsSource;
    
    private void Awake()
    {
        LoopMusicAudioSource.loop = true;
        IntroMusicAudioSource.clip = MainThemeWithIntro;
        LoopMusicAudioSource.clip = MainThemeLoop;

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            instance.PlayMainTheme();
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true, bool isMainTheme = false)
    {
        if (MusicSource.isPlaying && !isMainTheme)
        {
            MusicSource.Stop();
        }
        MusicSource.clip = clip; 
        MusicSource.loop = loop;

        MusicSource.Play();
    }

    public void PlayMainTheme()
    {
        IntroMusicAudioSource.Stop();
        LoopMusicAudioSource.Stop();

        MusicSource.Stop();

        LoopMusicAudioSource.loop = true;
        IntroMusicAudioSource.clip = MainThemeWithIntro;
        LoopMusicAudioSource.clip = MainThemeLoop;

        // This accurately calculates the intro's duration
        double introDuration = (double)MainThemeWithIntro.samples / MainThemeWithIntro.frequency;
        // This plays the first clip and schedules the second to play immediately after it
        IntroMusicAudioSource.PlayScheduled(AudioSettings.dspTime + 0.1);
        LoopMusicAudioSource.PlayScheduled(AudioSettings.dspTime + 0.1 + introDuration);
    }

    public void StopMainTheme()
    {
        LoopMusicAudioSource.Stop();
    }

    public void StartBossTheme()
    {
        // This accurately calculates the intro's duration
        double introDuration = (double)BossThemeWithIntro.samples / BossThemeWithIntro.frequency;
        // This plays the first clip and schedules the second to play immediately after it
        IntroMusicAudioSource.clip = BossThemeWithIntro;
        LoopMusicAudioSource.clip = BossThemeLoop;

        IntroMusicAudioSource.PlayScheduled(AudioSettings.dspTime + 0.1);
        LoopMusicAudioSource.PlayScheduled(AudioSettings.dspTime + 0.1 + introDuration);
    }

    public void PlayEffect(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }

    public void PlayEffect(AudioClip[] clips, int index)
    {
        effectsSource.PlayOneShot(clips[index]);
    }

    private int PlayRandomEffect(AudioClip[] clips)
    {
        int rnd = _random.Next(clips.Length - 1);
        Debug.Log(rnd);
        PlayEffect(clips[rnd]);
        return rnd;
    }
}