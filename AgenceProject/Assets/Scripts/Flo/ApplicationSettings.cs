using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{

    bool postProcess = true;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void PostProcessActive()
    {
        postProcess = !postProcess;
    }
}
