using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstMove : MonoBehaviour
{

    public float forceToAdd;
    public BoxCollider2D colliderStick;
    public BoxCollider2D colliderStatic;
    public BoxCollider2D triggerColl;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitIntroPlayer()
    {
        PlayerController.playerState = PlayerState.moving;
        GameManager.gameManager.Shoot();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(forceToAdd, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StaticWall" || collision.gameObject.tag == "StickyWall")
        {
            colliderStick.enabled = false;
            colliderStatic.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StaticWall" || collision.gameObject.tag == "StickyWall")
        {
            triggerColl.enabled = false;
            colliderStick.enabled = true;
            colliderStatic.enabled = true;
        }
    }
}
