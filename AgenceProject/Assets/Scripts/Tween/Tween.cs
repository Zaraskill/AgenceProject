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

    public bool isSpecialAnim;
}

[System.Serializable]
public class TweenListMenu
{
    public string name;

    public string[] tweens;
}