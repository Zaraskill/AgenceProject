using UnityEngine;

public class PushableDestroy : MonoBehaviour
{
    public bool isDestroying;
    private Material mat;
    private float fade;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        fade = BrickData.timerPushableDestroy;
    }

    void Update()
    {
        if (isDestroying)
        {
            fade -= Time.deltaTime;

            if (fade > 0)
            {
                float ft = fade / BrickData.timerPushableDestroy;
                mat.SetFloat("_Fade", ft);
            }
            else
            {
                isDestroying = false;
            }
            
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