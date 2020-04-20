using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky_behavior : MonoBehaviour
{
    public PlayerController playerController;

    public bool throwAllowed;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "StickyWall" && !throwAllowed)
        {
            playerController.UpdatePlayerState(PlayerState.idle);
            Debug.Log("collide with : " + other.gameObject.tag);
            throwAllowed = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "StickyWall" && throwAllowed)
        {
            Debug.Log("exit with : " + other.gameObject.tag);
            throwAllowed = false;
        }

    }
}
