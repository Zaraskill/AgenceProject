using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableWall : MonoBehaviour
{
    private Vector2 collidepos;
    private Vector2 otherVelocity;
    private GameObject parent;
    private Rigidbody2D parentRb;

    void Awake()
    {
        parent = transform.parent.gameObject;
        parentRb = parent.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = parent.transform.rotation;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            otherVelocity = other.GetContact(0).rigidbody.velocity;
            collidepos = other.GetContact(0).point;
            parentRb.AddForceAtPosition(-otherVelocity, collidepos,ForceMode2D.Impulse);
        }
    }
}