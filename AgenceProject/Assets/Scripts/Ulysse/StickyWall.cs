using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWall : Wall
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.otherRigidbody.bodyType = RigidbodyType2D.Kinematic;
            other.gameObject.GetComponent<PlayerController>().playerState = PlayerState.idle;
            Debug.Log("collide with : "+other.gameObject.tag);
        }
    }
    //public void ActivateWall(PlayerController player)
    //{
    //    player.rb.bodyType = RigidbodyType2D.Kinematic;
    //    player.playerState = PlayerState.idle;
    //}
    
}
