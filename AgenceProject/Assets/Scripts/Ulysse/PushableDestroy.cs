﻿using UnityEngine;

public class PushableDestroy : MonoBehaviour
{
    public bool isDestroying;
    private Material mat;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !isDestroying)
        {
            StartCoroutine(VFXManager.instance.DestroyingDissolve(gameObject, mat, BrickData.timerPushableDestroy));
            isDestroying = true;
        }

        if (other.relativeVelocity.magnitude > 1f && other.relativeVelocity.magnitude <= 5f)
            AudioManager.instance.RandomPlayVolume("SFX_Brick_Metal_Impact_", 1, 6, 0.1f);
        else if (other.relativeVelocity.magnitude > 5f && other.relativeVelocity.magnitude <= 8f)
            AudioManager.instance.RandomPlayVolume("SFX_Brick_Metal_Impact_", 1, 6, 0.2f);
        else if (other.relativeVelocity.magnitude > 8f)
            AudioManager.instance.RandomPlayVolume("SFX_Brick_Metal_Impact_", 1, 6, 0.3f);
        
    }
}