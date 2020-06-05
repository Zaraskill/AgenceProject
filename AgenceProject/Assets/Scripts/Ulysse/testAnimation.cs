using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class testAnimation : MonoBehaviour
{
    public GameObject[] Rigs;
    public Image[] ColorButtons;
    int buttonIndex;
    int rigsCount;
    private Animator animator;

    void Start()
    {
        rigsCount = Rigs.Length;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetInteger("indexAnim",buttonIndex);
    }

    //void SwitchActive(int activeRig)
    //{
    //    for (int i = 0; i < rigsCount; i++)
    //    {
    //        if (i != activeRig)
    //        {
    //            Rigs[i].SetActive(false);
    //        }
    //        else
    //        {
    //            Rigs[i].SetActive(true);
    //        }
    //    }
    //}

    void SwitchActive()
    {
        for (int i = 0; i < rigsCount; i++)
        {
            if (i == buttonIndex)
            {
                Rigs[i].SetActive(!Rigs[i].activeSelf);
                SwitchButtonColor(i);
            }
        }
    }

    void SwitchAnim()
    {
        if (buttonIndex == 0)
        {
            Rigs[1].SetActive(false);
            SwitchButtonColor(1);
        }
        else if (buttonIndex == 1)
        {
            Rigs[0].SetActive(false);
            SwitchButtonColor(0);
        }
    }

    void SwitchButtonColor(int i)
    {
        if (Rigs[i].gameObject.activeSelf)
            ColorButtons[i].color = Color.green;
        else
            ColorButtons[i].color = Color.white;

    }

    public void OnClickSwitchAnim(int buttonIndex)
    {
        this.buttonIndex = buttonIndex;
        SwitchActive();
        SwitchAnim();
    }
}
