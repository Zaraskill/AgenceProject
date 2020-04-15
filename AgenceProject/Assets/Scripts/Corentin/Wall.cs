using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wall : MonoBehaviour
{
    public abstract void ActivateWall(Rigidbody2D rigidbody);

}
