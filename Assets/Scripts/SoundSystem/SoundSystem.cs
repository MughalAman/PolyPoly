using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        s.source.Play();
    }

}
