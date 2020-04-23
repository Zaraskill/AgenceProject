using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{

    public float health = 3f;
    public float highHealthJump = 7f;

    //public ParticleSystem DestructFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            //Instantiate(DestructFX, transform.position, Quaternion.identity);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ( (collision.gameObject.tag == "Ennemy" && collision.relativeVelocity.magnitude >= highHealthJump) || (collision.gameObject.tag != "Ennemy" && collision.relativeVelocity.magnitude >= highHealthJump) )
        {
            Destroy(gameObject);
            //Instantiate(DestructFX, transform.position, Quaternion.identity);
        }
        else if ( collision.gameObject.tag == "DestructibleWall" || collision.gameObject.tag == "PushableWall" )
        {
            if (collision.relativeVelocity.magnitude >= health)
            {
                Destroy(gameObject);
                //Instantiate(DestructFX, transform.position, Quaternion.identity);
            }
        }
    }

}
