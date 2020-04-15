using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWall : Wall
{
    public override void ActivateWall(PlayerController player)
    {
        player.rb.bodyType = RigidbodyType2D.Kinematic;
        player.playerState = PlayerState.idle;
    }
}
