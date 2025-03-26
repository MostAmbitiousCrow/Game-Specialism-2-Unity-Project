using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "ScriptableObjects/Projectiles/Player Projectiles", order = 0)]
public class PlayerProjectileData : ScriptableObject
{
    [Header("Projectile Settings")]
    [Header("Movement")]
    public bool useMoveAcceleration = false; // If true, the projectile will start at a slower speed and Accelerate to the start speed, or the opposite

    [Range(0, 10f)] public float moveStartSpeed = 2; // The speed at which the projectile starts

    [Range(0, 10f)] public float moveEndSpeed = 0; // The speed at which the projectile ends

    public AnimationCurve moveAccelerationCurve; // The Acceleration curve that the projectile will follow

    [Header("Homing")]
    public bool canHome = true;
    [Range(0, 2)] public float timeTilHome = .5f;
    [Range(0, 10)] public float homingStrength = 2;

    [Header("Damage")]
    [Range(0, 100)] public float projectileDamage = 1; // The damage the projectile will deal

    [Header("Life Time")]
    [Range(.1f, 20f)] public float projectileLifeTime = 5; // The time before the projectile is disabled

    [Header("Size")]
    [Range(.1f, 10)] public float projectileSize = 1; // The size of the projectile
}