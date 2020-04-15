using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWall : Wall
{
    public void ActivateWall(PlayerController player)
    {
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        player.playerState = PlayerState.idle;
    }
}
