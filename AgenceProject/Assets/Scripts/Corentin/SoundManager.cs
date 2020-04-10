using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager soundManager;

    //public AudioClip clioType;

    void Awake()
    {
        if (soundManager != null)
        {
            Debug.LogError("Too many instances!");
        }
        else
        {
            soundManager = this;
            DontDestroyOnLoad(soundManager);
        }
    }

    private void MakeSound(AudioClip originalClip)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position);
    }

    //public void MakeTypeSound()
    //{
    //    MakeSound(typeSound);
    //}
}
