using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTween : MonoBehaviour
{

    private GameObject objectTween;
    private Vector3 objectifScale;
    private Vector3 startScale;
    private float timer;
    private float timeAnim = 0f;
    private bool isInit = false;
    private bool isSpecialAnim = false;


    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            if (isSpecialAnim)
            {
                timeAnim += Time.deltaTime;
                objectTween.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, objectifScale, Mathf.PingPong(timeAnim, 1));
            }
            else
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
                    if (objectTween.GetComponent<Button>() != null)
                    {
                        objectTween.GetComponent<Button>().interactable = true;
                    }
                }
            }
        }
    }

    public void StartAnim(GameObject objectToTween, Vector3 scale, float timing, bool specialAnim)
    {
        if (objectToTween.GetComponent<Button>() != null && !specialAnim)
        {
            objectToTween.GetComponent<Button>().interactable = false;
        }
        startScale = objectToTween.GetComponent<RectTransform>().localScale;
        objectTween = objectToTween;
        objectifScale = scale;
        timer = timing;
        isInit = true;
        timeAnim = 0f;
        isSpecialAnim = specialAnim;
    }

    public void StopAnim()
    {
        isInit = false;
    }
}
