using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound[] music;
    public Sound[] sounds;

    [SerializeField] private Slider musicVol, sfxVol;

    //public static AudioManager instance; // Uncomment if working with multiple scenes

    // Start is called before the first frame update
    void Awake()
    {
        // Uncomment the section below if working with multiple scenes
        /*
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destory(gameObject);
            return;
        }

        DontDestoryOnLoad(gameObject);
        */
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume * sfxVol.value;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;

            m.source.volume = m.volume * musicVol.value;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = s.volume * sfxVol.value;
        s.source.Play();
    }
    public void PlayMusic(string name)
    {
        Sound m = Array.Find(music, sound => sound.name == name);
        m.source.volume = m.volume * musicVol.value;
        m.source.Play();
    }

    public void UpdateVolume()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * sfxVol.value;
        }

        foreach (Sound m in music)
        {
            m.source.volume = m.volume * musicVol.value;
        }
    }
    
}
