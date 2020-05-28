using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableWall : MonoBehaviour
{
    private Vector2 collidepos;
    private Vector2 otherVelocity;
    private GameObject parent;
    private Rigidbody2D parentRb;
    private Material material;

    bool isDissolving;
    float fade = 2f;

    void Awake()
    {
        parent = transform.parent.gameObject;
        parent.AddComponent<PushableDestruction>();
        parentRb = parent.GetComponent<Rigidbody2D>();
        material = parent.GetComponent<SpriteRenderer>().material;
    }

    void FixedUpdate()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = parent.transform.rotation;

        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade <= 0)
            {
                fade = 0;
                isDissolving = false;
            }

            material.SetFloat("_Fade", fade);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            otherVelocity = other.GetContact(0).rigidbody.velocity;
            collidepos = other.GetContact(0).point;
            parentRb.AddForceAtPosition(-otherVelocity, collidepos,ForceMode2D.Impulse);
            isDissolving = true;
        }
    }
}