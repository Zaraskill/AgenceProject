using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{

    [Header("Parameter")]
    public bool postProcess;

    [Header("Object")]
    public GameObject Volume;

    

    private void Awake()
    {
        Application.targetFrameRate = 60;
        postProcess = PlayerData.instance.parameter[0];
        PostProcessActive(postProcess);
        LightProperty();
    }

    public void PostProcessActive(bool active)
    {
        postProcess = active;
        PlayerData.instance.parameter[0] = active;
        Volume.SetActive(postProcess);
    }

    public void LightProperty()
    {
        Transform LightCoponent = gameObject.transform.GetChild(0);
        foreach (GameObject light in LightCoponent)
        {
            light.GetComponent<Light>().intensity = 1f;
        }
    }


}
