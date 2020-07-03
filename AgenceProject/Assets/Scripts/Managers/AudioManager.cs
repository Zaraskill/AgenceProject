using UnityEngine.Audio;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [Header("Sound List")]
    public Sound[] sounds;
    public string currentMusic;

    public static AudioManager instance;
    public AudioMixer Mixer;
    public AudioMixerGroup MusicMixer;
    public AudioMixerGroup SoundMixer;

    [Header("Volume")]
    public float musicVolume;
    public float soundVolume;

    [Header("Pitch Range")]
    public float minPitch;
    public float maxPitch;

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

    public void IntiAudio(int sceneId)
    {
        UIManager.uiManager.RefreshToggleMusic();
        UIManager.uiManager.RefreshToggleSound();
        CutMusic(PlayerData.instance.parameter[2]);
        CutSound(PlayerData.instance.parameter[3]);

        PlayLevelMusic(sceneId);
    }

    /* Music List :
     * 
     * Music_Main_Theme
     * Music_The_Experiment
     * Music_The_Laboratory
     * Music_Lab
     * 
     */
    void PlayLevelMusic(int id)
    {
        if (id == 0)
            PlayMusic("Music_Main_Theme");
        else if (id >= 1 && id < 9)
            PlayMusic("Music_The_Experiment");
        else if (id >= 9 && id < 17)
            PlayMusic("Music_The_Laboratory");
        else
            PlayMusic("Music_Lab");
    }

    public void Play (string name, bool pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        else if (pitch)
            s.source.pitch = Random.Range(minPitch, maxPitch);

        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        else if (currentMusic == name)
            return;
        else if (currentMusic != null)
            Stop(currentMusic);

        currentMusic = name;

        s.source.Play();
    }

    public void RandomPlay (string name, bool pitch, int min, int max)
    {
        name  += Random.Range(min, max);
        //Debug.Log(name);
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        else if (pitch)
            s.source.pitch = Random.Range(minPitch, maxPitch);

        s.source.Play();
    }
    
    public void RandomPlayVolume(string name, bool pitch, int min, int max, float volume)
    {
        name += Random.Range(min, max);
        //Debug.Log(name);
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        else if (pitch)
            s.source.pitch = Random.Range(minPitch, maxPitch);

        s.source.volume = volume;
        s.source.Play();
    }

    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public void ManagerVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        s.volume = volume;
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
