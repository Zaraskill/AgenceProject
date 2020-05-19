using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckListVelocity : MonoBehaviour
{
    public GameObject PushableComponent;
    public GameObject DestructibleComponent;

    public List<Rigidbody2D> Children;

    private int childrenLenght;

    
    void Start()
    {
        if (PushableComponent != null)
        {
            foreach (Transform childComponent in PushableComponent.transform)
            {
                foreach (Transform child in childComponent)
                {
                    if(child.gameObject.activeSelf)
                        Children.Add(child.GetComponent<Rigidbody2D>());
                }
            }
        }
        if (DestructibleComponent != null)
        {
            foreach (Transform child in DestructibleComponent.transform)
            {
                if (child.gameObject.activeSelf)
                    Children.Add(child.GetComponent<Rigidbody2D>());
            }
        }
        childrenLenght = Children.Count;
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
        int i;
        while (active)
        {
            i = 0;
            if (PlayerController.playerState == PlayerState.idle)
            {
                foreach (Rigidbody2D child in Children)
                {
                    if (child != null)
                    {
                        if (child.velocity.magnitude > 0.01f)
                            break;
                        if (child.gameObject.tag == "PushableWall" && child.gameObject.GetComponent<PushableDestruction>().isDestroying)
                            break;
                    }
                    i++;
                }
                if (i == childrenLenght)
                    active = false;
            }
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Laucher Ready !");
        PlayerController.throwAllowed = true;
        GameManager.gameManager.gameState = GameManager.STATE_PLAY.verificationThrow;
        GameManager.gameManager.IsntChecking();
    }
}
