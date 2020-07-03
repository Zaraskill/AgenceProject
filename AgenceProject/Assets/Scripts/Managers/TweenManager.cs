using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TweenManager : MonoBehaviour
{
    public static TweenManager tweenManager;

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
                StartCoroutine(StartPlay(tw, 0f));
                AudioManager.instance.Play("SFX_Scoring_One_Star", false);
                break;
            case 2:
                Tween tw1 = Array.Find(tweens, tween => tween.name == "introStarOne");
                Tween tw2 = Array.Find(tweens, tween => tween.name == "introStarTwo");
                StartCoroutine(StartPlay(tw1, 0f));
                StartCoroutine(StartPlay(tw2, 0.25f));
                AudioManager.instance.Play("SFX_Scoring_Two_Star", false);
                break;
            case 3:
                Tween twA = Array.Find(tweens, tween => tween.name == "introStarOne");
                Tween twB = Array.Find(tweens, tween => tween.name == "introStarTwo");
                Tween twC= Array.Find(tweens, tween => tween.name == "introStarThree");
                StartCoroutine(StartPlay(twA , 0f));
                StartCoroutine(StartPlay(twB, 0.25f));
                StartCoroutine(StartPlay(twC, 0.5f));
                AudioManager.instance.Play("SFX_Scoring_Three_Star", false);
                break;
            default:
                break;
        }
    }

    IEnumerator StartPlay(Tween tween, float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        tween.objectToTween.SetActive(true);
        Play(tween.name);
    }
}
