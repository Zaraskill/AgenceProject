using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingWall : MonoBehaviour
{

    private Rigidbody2D _rigidbody;
    public float reboundForce = 10f;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("rb find");
            _rigidbody.AddForce(transform.up * reboundForce);
        }        
    }
}
