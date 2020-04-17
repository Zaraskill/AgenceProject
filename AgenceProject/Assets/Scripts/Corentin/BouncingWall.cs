using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingWall : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public enum SIDE_JUMP { LEFT, RIGHT, UP, DOWN}
    //private Vector2 leftUpAngle, rightUpAngle, leftBotAngle, rightBotAngle;
    public float reboundForce = 10f;
    public SIDE_JUMP sideJump;

    void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            switch (sideJump)
            {
                case SIDE_JUMP.UP:
                    _rigidbody.AddForce(transform.up * reboundForce);
                    break;
                case SIDE_JUMP.RIGHT:
                    _rigidbody.AddForce(transform.right * reboundForce);
                    break;
                case SIDE_JUMP.LEFT:
                    _rigidbody.AddForce(-transform.right * reboundForce);
                    break;
                case SIDE_JUMP.DOWN:
                    _rigidbody.AddForce(-transform.up * reboundForce);
                    break;
                default:
                    break;
            }
        }        
    }    
}
