using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{

    public Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(new Vector2(50f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.gameManager.DeactivateTuto();
        }
        //_rigidbody.AddForce(new Vector2(50f, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.gameManager.ActivateTuto();
        if (collision.tag == "Ennemy")
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "LeftCollider")
        {
            collision.GetComponentInParent<BouncingWall>().LeftHit();
        }
        else if (collision.tag == "RightCollider")
        {
            collision.GetComponentInParent<BouncingWall>().RightHit();
        }
        else if (collision.tag == "TopCollider")
        {
            collision.GetComponentInParent<BouncingWall>().UpHit();
        }
        else if (collision.tag == "BotCollider")
        {
            collision.GetComponentInParent<BouncingWall>().DownHit();
        }
    }


}
