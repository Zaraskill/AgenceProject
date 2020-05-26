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
        //CreateArrays();
        //SetComponentArray();
    }
    void Update()
    {
        SwitchActive((int)PlayerController.playerState);
    }

    void SwitchActive(int activeRig)
    {
        for (int i = 0; i < rigsCount; i++)
        {
            if (i != activeRig)
            {
                Rigs[i].SetActive(false);
                //spriteR[i].enabled = false;
                //spriteSkin[i].enabled = false;
            }
            else
            {
                Rigs[i].SetActive(true);
                //spriteR[activeRig].enabled = true;
                //spriteSkin[activeRig].enabled = true;
            }
        }
    }

    private void CreateArrays()
    {
        spriteR = new SpriteRenderer[rigsCount];
        spriteSkin = new SpriteSkin[rigsCount];
    }

    void SetComponentArray()
    {
        for (int i = 0; i < rigsCount; i++)
        {
            spriteR[i] = Rigs[i].GetComponent<SpriteRenderer>();
            spriteSkin[i] = Rigs[i].GetComponent<SpriteSkin>();
        }
    }
}
