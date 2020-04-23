using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{

    public float health = 3f;
    public float highHealthJump = 7f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.relativeVelocity);
        Debug.Log(collision.relativeVelocity.magnitude);
        if ( (collision.gameObject.tag == "Ennemy" && collision.relativeVelocity.magnitude >= highHealthJump) || (collision.gameObject.tag != "Ennemy" && collision.relativeVelocity.magnitude >= highHealthJump) )
        {
            Destroy(gameObject);
        }
        else if ( collision.gameObject.tag == "DestructibleWall" || (collision.gameObject.tag == "BouncyWall" && collision.gameObject.GetComponent<BouncingWall>().IsMovable()) )
        {
            if (collision.relativeVelocity.magnitude >= health)
            {
                Destroy(gameObject);
            }
        }
    }

}
