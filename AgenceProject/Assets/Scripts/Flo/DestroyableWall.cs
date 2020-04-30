using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{

    private float health;
    private float highHealthJump;

    void Awake()
    {
        health = transform.parent.gameObject.GetComponent<GlassValues>().health;
        highHealthJump = transform.parent.gameObject.GetComponent<GlassValues>().highHealthJump;
    }

    public ParticleSystem DestructFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Instantiate(DestructFX, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("glass_broken");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ( (collision.gameObject.tag == "Ennemy" && collision.relativeVelocity.magnitude >= highHealthJump) || (collision.gameObject.tag != "Ennemy" && collision.relativeVelocity.magnitude >= highHealthJump) )
        {
            Destroy(gameObject);
            Instantiate(DestructFX, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("glass_broken");
        }
        else if ( collision.gameObject.tag == "DestructibleWall" || collision.gameObject.tag == "PushableWall" )
        {
            if (collision.relativeVelocity.magnitude >= health)
            {
                Destroy(gameObject);
                Instantiate(DestructFX, transform.position, Quaternion.identity);
                FindObjectOfType<AudioManager>().Play("glass_broken");
            }
        }
    }

}
