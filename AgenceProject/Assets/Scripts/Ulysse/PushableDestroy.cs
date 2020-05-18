using UnityEngine;

public class PushableDestruction : MonoBehaviour
{
    public bool isDestroying;

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