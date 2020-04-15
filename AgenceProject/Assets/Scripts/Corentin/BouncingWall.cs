using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingWall : Wall
{

    private Rigidbody2D _rigidbody;
    public float reboundForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("rb find");
            _rigidbody.AddForce(transform.up * force);
        }
    }

    public override void ActivateWall(Rigidbody2D rigidbody)
    {
        throw new System.NotImplementedException();
    }
}
