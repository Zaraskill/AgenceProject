using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAnimation : MonoBehaviour
{
    public GameObject[] Rigs;
    private int indexAnim;
    int rigsCount;
    private Animator animator;

    void Start()
    {
        rigsCount = Rigs.Length;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        SwitchActive(indexAnim);
        SwitchAnim();
        animator.SetInteger("indexAnim",indexAnim);
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

    void SwitchAnim()
    {
        if (Input.GetKeyDown(KeyCode.A))
            indexAnim = 0;
        else if (Input.GetKeyDown(KeyCode.Z))
            indexAnim = 1;
        else if (Input.GetKeyDown(KeyCode.E))
            indexAnim = 2;
    }

    public void OnClickSwitchAnim(int indexAnim)
    {
        this.indexAnim = indexAnim;
    }
}
