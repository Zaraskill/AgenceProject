using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public float verticalSpeed = 0f;
    public float verticalSpeedMax = 10f;
    public float gravity = 5f;
    private float sideApply = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetGravity()
    {
        verticalSpeed = 0f;
    }

    public float UpdateGravity()
    {
        verticalSpeed -= gravity * Time.fixedDeltaTime;
        if (verticalSpeed < - verticalSpeedMax)
        {
            verticalSpeed = -verticalSpeedMax;
        }
        return verticalSpeed;
    }

    public void ChangeSideGravity()
    {
        sideApply = -sideApply;
    }


}
