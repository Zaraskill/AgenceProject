using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tween 
{
    public string name;

    public GameObject objectToTween;

    public Vector3 objectif;

    public float timer;
}

[System.Serializable]
public class TweenListMenu
{
    public string name;

    public string[] tweens;
}