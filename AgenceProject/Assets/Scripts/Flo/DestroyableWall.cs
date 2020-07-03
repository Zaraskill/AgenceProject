using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    private ParticleSystem destructFX;
    private float health;
    private float highHealthJump;

    void Start()
    {
        health = GameData.glassHealth;
        highHealthJump = GameData.glassHighHealthJump;
        destructFX = GameData.destructFX;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Instantiate(destructFX, transform.position, Quaternion.identity);
            AudioManager.instance.RandomPlay("SFX_Brick_Break_Glass_", true, 1, 8);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ( (collision.gameObject.tag == "Ennemy" && collision.relativeVelocity.magnitude >= highHealthJump) || (collision.gameObject.tag != "Ennemy" && collision.relativeVelocity.magnitude >= highHealthJump) )
        {
            Destroy(gameObject);
            Instantiate(destructFX, transform.position, Quaternion.identity);
            AudioManager.instance.RandomPlay("SFX_Brick_Break_Glass_", true, 1, 8);
        }
        else if ( collision.gameObject.tag == "DestructibleWall" || collision.gameObject.tag == "PushableWall" )
        {
            if (collision.relativeVelocity.magnitude >= health)
            {
                Destroy(gameObject);
                Instantiate(destructFX, transform.position, Quaternion.identity);
                AudioManager.instance.RandomPlay("SFX_Brick_Break_Glass_", true, 1, 8);
            }
        }
    }
}