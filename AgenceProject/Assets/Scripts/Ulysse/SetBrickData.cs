using UnityEngine;

public class SetBrickData : MonoBehaviour
{
    [Header("Glass Brick")]
    public float glassHealth;
    public float glassHighHealthJump;
    public ParticleSystem destructFX;

    [Header("Pushable Brick")]
    public float timerPushableDestroy;

    void Awake()
    {
        BrickData.glassHealth = glassHealth;
        BrickData.glassHighHealthJump = glassHighHealthJump;
        BrickData.destructFX = destructFX;
        BrickData.timerPushableDestroy = timerPushableDestroy;
    }
}