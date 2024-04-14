using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
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

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
 
}
