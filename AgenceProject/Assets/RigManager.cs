using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class RigManager : MonoBehaviour
{
    public GameObject[] Rigs;
    private SpriteRenderer[] spriteR;
    private SpriteSkin[] spriteSkin;
    int rigsCount;

    void Start()
    {
        rigsCount = Rigs.Length;
        CreateArrays();
        SetComponentList();
    }

    private void CreateArrays()
    {
        spriteR = new SpriteRenderer[rigsCount];
        spriteSkin = new SpriteSkin[rigsCount];
    }

    void LateUpdate()
    {
        SwitchActive((int)PlayerController.playerState);
    }

    void SwitchActive(int activeRig)
    {
        for (int i = 0; i < rigsCount; i++)
        {
            if (i != activeRig)
            {
                spriteR[i].enabled = false;
                spriteSkin[i].enabled = false;
            }
            else
            {
                spriteR[activeRig].enabled = true;
                spriteSkin[activeRig].enabled = true;
            }
        }
        Debug.Log(activeRig);
    }

    void SetComponentList()
    {
        for (int i = 0; i < rigsCount; i++)
        {
            Debug.Log("set list " + i);
            Debug.Log("rig list " + Rigs[i].gameObject);
            spriteR[i] = Rigs[i].GetComponent<SpriteRenderer>();
            spriteSkin[i] = Rigs[i].GetComponent<SpriteSkin>();
        }
    }
}
