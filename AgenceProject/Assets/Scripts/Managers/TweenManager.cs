using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TweenManager : MonoBehaviour
{

    public Tween[] tweens;

    public TweenListMenu[] menuTweens;

    private bool canDoNext = true;

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
        switch (t.movement)
        {
            case "move":
                LeanTween.moveLocal(t.objectToTween, t.objectif, t.timer);
                break;
            case "alpha":
                LeanTween.alpha(t.objectToTween, t.alpha, t.timer);
                break;
            case "rotate":
                LeanTween.rotateLocal(t.objectToTween, t.objectif, t.timer);
                break;
            case "scale":
                LeanTween.scale(t.objectToTween, t.objectif, t.timer);
                break;
            default:
                break;
        }
    }

    public void PlayMenuTween(string name)
    {
        TweenListMenu t = Array.Find(menuTweens, tween => tween.name == name);
        if (t == null)
        {
            return;
        }
        while (!canDoNext)
        {

        }
        canDoNext = false;
        foreach(string tween in t.tweens)
        {
            Play(tween);
        }
        canDoNext = true;
    }
}
