using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckListVelocity : MonoBehaviour
{
    public GameObject PushableComponent;
    public GameObject DestructibleComponent;
    public Rigidbody2D Player;

    public List<Rigidbody2D> Children;
    //public float checkDuration = 1f;

    
    void Start()
    {
        /*
        foreach (Transform child in transform)
        {
            if (child.tag == "Player" || child.tag == "PushableWall" || child.tag == "DestructibleWall")
            {
                Children.Add(child.GetComponent<Rigidbody2D>());
            }
        }*/
        if (PushableComponent != null)
        {
            foreach (Transform childComponent in PushableComponent.transform)
            {
                foreach (Transform child in childComponent)
                {
                    Children.Add(child.GetComponent<Rigidbody2D>());
                }
            }
        }
        if (DestructibleComponent != null)
        {
            foreach (Transform child in DestructibleComponent.transform)
            {
                Children.Add(child.GetComponent<Rigidbody2D>());
            }
        }
    }


    public void CheckMoving()
    {
        StartCoroutine(Checking());
    }

    public void StopCheck()
    {
        StopCoroutine(Checking());
    }

    IEnumerator Checking()
    {
        yield return new WaitForSeconds(1f);
        bool active = true;
        while (active)
        {
            if (Player.velocity.magnitude <= 0.01f)
            {
                int objectDoesntSleep = 0;

                foreach (Rigidbody2D child in Children)
                {
                    if (child != null && child.velocity.magnitude > 0.01f)
                        objectDoesntSleep++;
                }

                if (objectDoesntSleep == 0)
                {
                    active = false;
                }
            }

            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Laucher Ready !");
        PlayerController.throwAllowed = true;
        GameManager.gameManager.gameState = GameManager.STATE_PLAY.verificationThrow;
    }
}
