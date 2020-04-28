using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{

    private Rigidbody2D rgbd;
    public BoxCollider2D collide;
    private Vector2 leftAngle, rightAngle;
    [SerializeField] bool isDying;


    // Start is called before the first frame update
    void Start()
    {
        leftAngle = new Vector2((collide.bounds.center.x - collide.bounds.extents.x), (collide.bounds.center.y + collide.bounds.extents.y));
        rightAngle = new Vector2((collide.bounds.center.x + collide.bounds.extents.x), (collide.bounds.center.y + collide.bounds.extents.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsDying()
    {
        return isDying;
    }

    public void Die()
    {
        isDying = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.tag == "StickyWall" || collision.gameObject.tag == "PushableWall" || collision.gameObject.tag == "DestructibleWall")
        {
            if (isDying)
            {
                return;
            }
            ContactPoint2D[] contact = collision.contacts;
            foreach (ContactPoint2D point in contact)
            {                
                if ( point.point.x >= (leftAngle.x - 0.3f) && point.point.y >= (rightAngle.y - 0.3f))
                {
                    isDying = true;
                    Debug.Log("Die !");
                    LevelManager.levelManager.EnnemyDeath();
                    Destroy(this.gameObject);
                    break;
                }
            }
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        if (isDying)
    //        {
    //            return;
    //        }
    //        collision.GetComponent<Ennemy>().Die();
    //        LevelManager.levelManager.EnnemyDeath();

    //        animator.Play("Eat");
    //        Destroy(collision.gameObject);
    //        Debug.Log("Die !");
    //    }
    //}

}
