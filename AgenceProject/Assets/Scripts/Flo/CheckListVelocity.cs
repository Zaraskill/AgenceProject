using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckListVelocity : MonoBehaviour
{

    public List<Rigidbody2D> Children;

    
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Player" || child.tag == "BouncyWall" || child.tag == "DestructibleWall")
            {
                Children.Add(child.GetComponent<Rigidbody2D>());
            }
        }
    }


    public void CheckMoving()
    {
        StartCoroutine(Checking());
    }

    IEnumerator Checking()
    {
        bool active = true;
        while (active)
        {
            int objectDoesntSleep = 0;

            foreach (Rigidbody2D child in Children)
            {
                if (child != null && child.velocity.magnitude > 0.01f)
                    objectDoesntSleep++;
            }

            if(objectDoesntSleep == 0)
            {
                active = false;
            }

            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Laucher Ready !");
        PlayerController.throwAllowed = true;
    }

}
