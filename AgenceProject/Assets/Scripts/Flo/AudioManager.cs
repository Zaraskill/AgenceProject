using UnityEngine.Audio;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    public AudioMixer Mixer;
    public AudioMixerGroup MusicMixer;
    public AudioMixerGroup SoundMixer;

    public float musicVolume;
    public float soundVolume;

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
            if (s.isMusic)
                s.source.outputAudioMixerGroup = MusicMixer;
            else
                s.source.outputAudioMixerGroup = SoundMixer;


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

    public void RandomPlay (string name, int min, int max)
    {
        name  += Random.Range(min, max);
        Debug.Log(name);
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
            Mixer.SetFloat("MusicVolume", -80f);
        else
            Mixer.SetFloat("MusicVolume", musicVolume);
    }

    public void CutSound(bool cut)
    {
        if (cut)
            Mixer.SetFloat("SoundVolume", -80f);
        else
            Mixer.SetFloat("SoundVolume", soundVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        Mixer.SetFloat("MusicVolume", musicVolume);
    }

    public void SetEffectsVolume(float value)
    {
        soundVolume = value;
        Mixer.SetFloat("SoundVolume", soundVolume);
    }
}
