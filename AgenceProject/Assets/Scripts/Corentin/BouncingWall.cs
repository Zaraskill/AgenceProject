using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingWall : MonoBehaviour
{
    public enum SIDE_JUMP { LEFT, RIGHT, UP, DOWN}
    private SIDE_JUMP sideJump;

    //Rebound
    public float reboundForce;

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

    public bool IsMovable()
    {
        return isMovable;
    }

    public void ReboundPlayer(Rigidbody2D rigidbody)
    {
        switch (sideJump)
        {
            case SIDE_JUMP.UP:
                rigidbody.AddForce(transform.up * reboundForce);
                break;
            case SIDE_JUMP.RIGHT:
                rigidbody.AddForce(transform.right * reboundForce);
                break;
            case SIDE_JUMP.LEFT:
                rigidbody.AddForce(-transform.right * reboundForce);
                break;
            case SIDE_JUMP.DOWN:
                rigidbody.AddForce(-transform.up * reboundForce);
                break;
            default:
                break;
        }
        if (isMovable)
        {
            isDestructionActivated = true;
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
