using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundName
    {
        Walk,
        Jump,
        Land,
        BoostJump,
        Catch,
        Pickup,
        UIClick,
        GetOut
    }

    [SerializeField] private Sound[] sounds;

    public static SoundManager instance;
    
    // private void Start()
    // {
    //     Play(SoundName.Ambient);
    // }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Play(SoundName name)
    {
        Sound sound = GetSound(name);
        if (sound.audioSource == null)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }
        sound.audioSource.Play();
    }

    public void Stop(SoundName name)
    {
        Sound sound = GetSound(name);
        if (sound.audioSource == null)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }
        sound.audioSource.Stop();
    }

    private Sound GetSound(SoundName name)
    {
        return Array.Find(sounds, s => s.soundName == name);
    }
}
