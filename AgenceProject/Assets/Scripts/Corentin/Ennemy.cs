using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public BoxCollider2D collide;
    public SpriteRenderer[] spritesR;
    private Material[] dissolveMat = new Material[6];
    private Rigidbody2D rb;
    private Animator animator;
    private float deathTriggerID;
    private Vector2 leftAngle, rightAngle;
    [SerializeField] bool isDying, isFalling;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //SetDeathTrigger();
    }

    void Update()
    {
        animator.SetFloat("verticalVelocity", rb.velocity.y);
        isFalling = IsFalling();
    }

    private bool IsFalling()
    {
        if (rb.velocity.y < -0.1f)
        {
            if (!isFalling)
                AudioManager.instance.RandomPlay("SFX_Ennemi_Falling_", 1, 3);
            return true;
        }
        else
            return false;
    }

    public void DissolveEnemy()
    {
        for (int i = 0; i < spritesR.Length; i++)
        {
            dissolveMat[i] = spritesR[i].material;
            StartCoroutine(VFXManager.instance.DestroyingDissolve(this.gameObject, dissolveMat[i], 1f));
        }
    }

    public bool IsDying()
    {
        return isDying;
    }

    public void Die()
    {
        isDying = true;
        animator.SetBool("isDying", true);
        LevelManager.levelManager.EnemyDeath();
        Destroy(this.gameObject, 1f);
    }

    //private void SetDeathTrigger()
    //{
    //    BoxCollider2D[] triggers;

    //    triggers = GetComponents<BoxCollider2D>();
    //    if (triggers[0].size.x < triggers[1].size.x)
    //        deathTriggerID = triggers[0].GetInstanceID();
    //    else
    //        deathTriggerID = triggers[1].GetInstanceID();
    //}



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDying)
            return;

        if (collision.gameObject.tag == "PushableWall" || collision.gameObject.tag == "DestructibleWall")
        {
            leftAngle = new Vector2((collide.bounds.center.x - collide.bounds.extents.x), (collide.bounds.center.y + collide.bounds.extents.y));
            rightAngle = new Vector2((collide.bounds.center.x + collide.bounds.extents.x), (collide.bounds.center.y + collide.bounds.extents.y));
            ContactPoint2D[] contact = collision.contacts;
            foreach (ContactPoint2D point in contact)
            {                
                if (point.point.x >= (leftAngle.x - 0.3f) && point.point.y >= (rightAngle.y - 0.3f))
                {
                    Die();
                    animator.SetBool("isDying",true);
                    AudioManager.instance.RandomPlay("SFX_Ennemi_Stunned_", 1, 5);
                    VFXManager.instance.PlayOnPositon("Stunned", transform.position);
                    break;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            animator.SetBool("triggeredByPlayer", true);
        //if (GetInstanceID() == deathTriggerID && !isDying)
        //{
        //    AudioManager.instance.RandomPlay("enemy_", 1, 5);
        //    Die();
        //    animator.SetBool("isDying",true);
        //    Material mat = GetComponent<SpriteRenderer>().material;
        //    StartCoroutine(VFXManager.instance.DestroyingDissolve(this.gameObject, mat, 0.7f));
        //    //other.animator.Play("Eat");
        //}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            animator.SetBool("triggeredByPlayer", false);
    }
}
