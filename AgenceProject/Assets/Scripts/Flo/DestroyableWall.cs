using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : Wall
{
    public override void ActivateWall(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

}
