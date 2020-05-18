using UnityEngine;

public static class BrickData
{
    [Header("Glass Brick")]
    public static float glassHealth;
    public static float glassHighHealthJump;
    public static ParticleSystem destructFX;

    [Header("Pushable Brick")]
    public static float timerPushableDestroy;
}