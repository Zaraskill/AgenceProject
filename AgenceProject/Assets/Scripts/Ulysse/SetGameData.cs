using UnityEngine;

public class SetGameData : MonoBehaviour
{
    [Header("Glass Brick")]
    public float glassHealth;
    public float glassHighHealthJump;
    public ParticleSystem destructFX;

    [Header("Pushable Brick")]
    public float timerPushableDestroy;

    [Header("Enemy material")]
    public Material enemyDeathMaterial;

    void Awake()
    {
        GameData.glassHealth = glassHealth;
        GameData.glassHighHealthJump = glassHighHealthJump;
        GameData.destructFX = destructFX;
        GameData.timerPushableDestroy = timerPushableDestroy;
        GameData.enemyDeathMaterial = enemyDeathMaterial;
    }
}