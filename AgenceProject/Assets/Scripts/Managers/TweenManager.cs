using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TweenManager : MonoBehaviour
{
    public static TweenManager tweenManager;

    public float timerStar1;
    public float timerStar2;
    public float timerStar3;

    private float timerStartButton;

    public Tween[] tweens;

    public TweenListMenu[] menuTweens;

    void Awake()
    {
        if (tweenManager == null)
        {
            tweenManager = this;
        }            
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(string name)
    {
        Tween t = Array.Find(tweens, tween => tween.name == name);
        if (t == null)
        {
            return;
        }
        if (t.objectToTween.GetComponent<AnimationTween>() == null)
        {
            t.objectToTween.AddComponent<AnimationTween>();
        }
        t.objectToTween.GetComponent<AnimationTween>().enabled = true;
        t.objectToTween.GetComponent<AnimationTween>().StartAnim(t.objectToTween, t.objectif, t.timer);
    }

    public void PlayMenuTween(string name)
    {
        TweenListMenu t = Array.Find(menuTweens, tween => tween.name == name);
        if (t == null)
        {
            return;
        }
        foreach(string tween in t.tweens)
        {
            Play(tween);
        }
    }

    public void PlayAnimStar(int numberStars)
    {
        switch (numberStars)
        {
            case 1:
                Tween tw = Array.Find(tweens, tween => tween.name == "introStarOne");
                AudioManager.instance.Play("SFX_Scoring_One_Star", false);
                timerStartButton = timerStar1;
                StartCoroutine(StartPlay(tw, timerStar1));
                break;
            case 2:
                Tween tw1 = Array.Find(tweens, tween => tween.name == "introStarOne");
                Tween tw2 = Array.Find(tweens, tween => tween.name == "introStarTwo");
                AudioManager.instance.Play("SFX_Scoring_Two_Star", false);
                timerStartButton = timerStar2;
                StartCoroutine(StartPlay(tw1, timerStar1));
                StartCoroutine(StartPlay(tw2, timerStar2));
                break;
            case 3:
                Tween twA = Array.Find(tweens, tween => tween.name == "introStarOne");
                Tween twB = Array.Find(tweens, tween => tween.name == "introStarTwo");
                Tween twC= Array.Find(tweens, tween => tween.name == "introStarThree");
                AudioManager.instance.Play("SFX_Scoring_Three_Star", false);
                timerStartButton = timerStar3;
                StartCoroutine(StartPlay(twA, timerStar1));
                StartCoroutine(StartPlay(twB, timerStar2));
                StartCoroutine(StartPlay(twC, timerStar3));
                break;
            default:
                timerStartButton = 0f;
                break;
        }
        StartCoroutine("WaitForButton");
    }

    IEnumerator StartPlay(Tween tween, float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        tween.objectToTween.SetActive(true);
        VFXManager.instance.PlayOnPositon("Star_Splash", tween.objectToTween.GetComponent<RectTransform>().position);
    }

    IEnumerator WaitForButton()
    {
        yield return new WaitForSecondsRealtime(timerStartButton);
        UIManager.uiManager.DisplayButton();
    }
}
