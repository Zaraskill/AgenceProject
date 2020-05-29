﻿using System;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public LightEvent lightEvent;

    public ParticleFX[] VFXs;

    public static VFXManager instance;

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

        foreach (ParticleFX p in VFXs)
        {
            GameObject current = Instantiate(p.particle, transform);
            p.particle = current;
            p.systeme = current.GetComponent<ParticleSystem>();
        }

    }

    public void Play(string name)
    {
        ParticleFX p = Array.Find(VFXs, particle => particle.name == name);
        if (p == null)
            return;
        p.systeme.Play();
    }

    public void Stop(string name)
    {
        ParticleFX p = Array.Find(VFXs, particle => particle.name == name);
        if (p == null)
            return;
        p.systeme.Stop();
    }

    public void PlayOnPositon(string name, Vector3 pos)
    {
        ParticleFX p = Array.Find(VFXs, particle => particle.name == name);
        if (p == null)
            return;
        
        p.particle.transform.position = pos;
        p.systeme.Play();
    }

    public void PlayOnScreenPositon(string name, Vector3 pos)
    {
        ParticleFX p = Array.Find(VFXs, particle => particle.name == name);
        if (p == null)
            return;

        p.particle.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 9.8f)); ;
        p.systeme.Play();
    }


    public void Alerte()
    {
        lightEvent.isOnAlerte = true;
    }


}
