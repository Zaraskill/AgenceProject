using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    public AudioMixerGroup mixer;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = mixer;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    /*
    void start()
    {
        Play("music");
    }
    */
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public void CutMusic(bool cut)
    {
        if (cut)
        {
            Sound s = Array.Find(sounds, sound => sound.name == "music");
            if (s == null)
                return;
            s.source.volume = 0f;
        }
        else
        {
            Sound s = Array.Find(sounds, sound => sound.name == "music");
            if (s == null)
                return;
            s.source.volume = 1f;
        }
    }

    public void CutSound(bool cut)
    {
        if (cut)
        {
            foreach (Sound s in sounds)
            {
                if(s.name != "music")
                {
                    s.source.volume = 0f;
                }
            }
        }
        else
        {
            foreach (Sound s in sounds)
            {
                if (s.name != "music")
                {
                    s.source.volume = 1f;
                }
            }
        }
    }

}
