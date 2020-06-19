using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TweenManager : MonoBehaviour
{

    public Tween[] tweens;

    public TweenListMenu[] menuTweens;

    public float speedStar1;
    public float speedStar2;
    public float speedStar3;

    public static TweenManager tweenManager;

    // Start is called before the first frame update
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
                tw.timer = speedStar1;
                Play(tw.name);
                break;
            case 2:
                Tween tw1 = Array.Find(tweens, tween => tween.name == "introStarOne");
                tw1.timer = speedStar2;
                Tween tw2 = Array.Find(tweens, tween => tween.name == "introStarTwo");
                tw2.timer = speedStar2;
                Play(tw1.name);
                Play(tw2.name);
                break;
            case 3:
                Tween twA = Array.Find(tweens, tween => tween.name == "introStarOne");
                twA.timer = speedStar3;
                Tween twB = Array.Find(tweens, tween => tween.name == "introStarTwo");
                twB.timer = speedStar3;
                Tween twC= Array.Find(tweens, tween => tween.name == "introStarThree");
                twC.timer = speedStar3;
                Play(twA.name);
                Play(twB.name);
                Play(twC.name);
                break;
            default:
                break;
        }
    }
}
