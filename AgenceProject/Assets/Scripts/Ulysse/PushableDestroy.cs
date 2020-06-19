using UnityEngine;

public class PushableDestroy : MonoBehaviour
{
    public bool isDestroying;
    private Material mat;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !isDestroying)
        {
            StartCoroutine(VFXManager.instance.DestroyingDissolve(gameObject, mat, BrickData.timerPushableDestroy));
            isDestroying = true;
        }
    }
}