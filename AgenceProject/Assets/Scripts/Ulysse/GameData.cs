using UnityEngine;

public static class GameData
{
    [Header("Glass Brick")]
    public static float glassHealth;
    public static float glassHighHealthJump;
    public static ParticleSystem destructFX;

    [Header("Pushable Brick")]
    public static float timerPushableDestroy;

    //Enemy Death material
    public static Material enemyDeathMaterial;
}