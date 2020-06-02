using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class RigManager : MonoBehaviour
{
    public GameObject[] Rigs;
    int rigsCount;

    void Start()
    {
        rigsCount = Rigs.Length;
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
            }
            else
            {
                Rigs[i].SetActive(true);
            }
        }
    }
}
