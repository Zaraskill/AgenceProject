using UnityEngine;

public class PushableDestruction : MonoBehaviour
{
    public bool isDestroying;
    private Material mat;
    private float fade;

    void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
        fade = BrickData.timerPushableDestroy;
    }

    void FixedUpdate()
    {
        if (isDestroying)
        {
            fade -= Time.deltaTime;

            if (fade <= 0)
            {
                fade = 0;
                isDestroying = false;
            }

            mat.SetFloat("_Fade", fade / BrickData.timerPushableDestroy);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject, BrickData.timerPushableDestroy);
            isDestroying = true;
            Debug.Log("Destroy");
        }
    }
}