using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWall : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().UpdatePlayerState(PlayerState.idle);
            Physics2D.IgnoreLayerCollision(8,9);
            Debug.Log("collide with : " + other.gameObject.tag);
        }
    }
}
