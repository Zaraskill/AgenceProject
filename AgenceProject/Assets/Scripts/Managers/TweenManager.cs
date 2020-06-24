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
                tw.objectToTween.SetActive(true);
                Play(tw.name);
                AudioManager.instance.Play("SFX_Scoring_One_Star");
                break;
            case 2:
                Tween tw1 = Array.Find(tweens, tween => tween.name == "introStarOne");
                Tween tw2 = Array.Find(tweens, tween => tween.name == "introStarTwo");
                tw1.objectToTween.SetActive(true);
                tw2.objectToTween.SetActive(true);
                Play(tw1.name);
                Play(tw2.name);
                AudioManager.instance.Play("SFX_Scoring_Two_Star");
                break;
            case 3:
                Tween twA = Array.Find(tweens, tween => tween.name == "introStarOne");
                Tween twB = Array.Find(tweens, tween => tween.name == "introStarTwo");
                Tween twC= Array.Find(tweens, tween => tween.name == "introStarThree");
                twA.objectToTween.SetActive(true);
                twB.objectToTween.SetActive(true);
                twC.objectToTween.SetActive(true);
                Play(twA.name);
                Play(twB.name);
                Play(twC.name);
                AudioManager.instance.Play("SFX_Scoring_Three_Star");
                break;
            default:
                break;
        }
    }
}
