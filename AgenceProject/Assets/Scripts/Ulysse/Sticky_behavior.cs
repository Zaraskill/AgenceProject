using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBehavior : PlayerController
{
    public PlayerController playerController;

    public bool throwAllowed;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "StickyWall" && playerController.playerState == PlayerState.moving && !throwAllowed)
        {
            playerController.UpdatePlayerState(PlayerState.idle);
            Debug.Log("collide with : " + other.gameObject.tag);
            throwAllowed = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "StickyWall" && playerController.playerState == PlayerState.moving && throwAllowed)
        {
            Debug.Log("exit with : " + other.gameObject.tag);
            throwAllowed = false;
        }

    }
}
