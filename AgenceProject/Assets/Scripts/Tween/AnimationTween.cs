using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTween : MonoBehaviour
{

    private GameObject objectTween;
    private Vector3 objectifScale;
    private Vector3 startScale;
    private float timer;
    private float timeAnim = 0f;
    private bool isInit = false;


    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            if (timeAnim <= timer)
            {
                timeAnim += Time.unscaledDeltaTime;
                float fractionOfTravel = timeAnim / timer;

                objectTween.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, objectifScale, fractionOfTravel);
            }
            else
            {
                isInit = false;
                timeAnim = 0f;
                this.enabled = false;
            }
        }
    }

    public void StartAnim(GameObject objectToTween, Vector3 scale, float timing) 
    {
        objectTween = objectToTween;
        objectifScale = scale;
        timer = timing;
        isInit = true;
    }
}
