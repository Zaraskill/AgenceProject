using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public WallType wallType;

    public void ActivateWall(PlayerController player)
    {

    }
}

public enum WallType
{
    Sticky,
    Bouncy,
    Normal
}
