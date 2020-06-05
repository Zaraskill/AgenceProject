using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEvent : MonoBehaviour
{
    public bool isOnAlerte;
    bool AcSwitch;

    public GameObject alarm1;
    public GameObject alarm2;
    [Range(10f, 450f)]
    public float speedRotate = 100f;
    public Animator anim;

    void Start()
    {
        AcSwitch = true;
    }    

    void Update()
    {
        if (isOnAlerte)
        {
            if (AcSwitch)
            {
                alarm1.SetActive(true);
                alarm2.SetActive(true);
                anim.SetBool("isOnAlerte", true);
                AcSwitch = false;
            }
            alarm1.transform.Rotate(0, 0, -speedRotate * Time.deltaTime);
            alarm2.transform.Rotate(0, 0, -speedRotate * Time.deltaTime);

        }
    }

    public void StopAlerte()
    {
        alarm1.SetActive(false);
        alarm2.SetActive(false);
        anim.SetBool("isOnAlerte", false);
        AcSwitch = true;
    }
}
