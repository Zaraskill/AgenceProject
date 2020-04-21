using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingWall : MonoBehaviour
{
    public enum SIDE_JUMP { LEFT, RIGHT, UP, DOWN}
    private SIDE_JUMP sideJump;

    //Rebound
    public float reboundForce = 10f;

    //Physique
    public bool isMovable;
    private Rigidbody2D _rigidbody;

    //Destruction
    private bool isDestructionActivated;
    public float timerDestruction;

    void Update()
    {
        if (isDestructionActivated)
        {
            timerDestruction -= Time.deltaTime;
            if (timerDestruction <= 0f)
            {
                Destroy(this.gameObject);
            }
        }
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
            if (isMovable)
            {
                isDestructionActivated = true;
            }
        }
    }

    #region Side collisions

    public void UpHit()
    {
        sideJump = SIDE_JUMP.UP;
    }

    public void DownHit()
    {
        sideJump = SIDE_JUMP.DOWN;
    }

    public void LeftHit()
    {
        sideJump = SIDE_JUMP.LEFT;
    }

    public void RightHit()
    {
        sideJump = SIDE_JUMP.RIGHT;
    }

    #endregion
}
