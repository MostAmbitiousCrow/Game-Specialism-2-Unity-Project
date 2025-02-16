using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "ScriptableObjects/Projectiles/Enemy Projectiles/Basic Enemy Projectile", order = 0)]
public class SO_Proj_Eni_Bas : ScriptableObject // By Samuel White
{
    [Header("Projectile Settings")]
    [Header("Movement")]
    public bool useMoveAcceleration = false; // If true, the projectile will start at a slower speed and Accelerate to the start speed, or the opposite

    [Range(0, 10f)] public float moveStartSpeed = 2; // The speed at which the projectile starts

    [Range(0, 10f)] public float moveEndSpeed = 0; // The speed at which the projectile ends

    public AnimationCurve moveAccelerationCurve; // The Acceleration curve that the projectile will follow

    [Header("Damage")]
    [Range(0, 100)] public float projectileDamage = 1; // The damage the projectile will deal

    [Header("Life Time")]
    [Range(.1f, 20f)] public float projectileLifeTime = 5; // The time before the projectile is disabled

    [Header("Size")]
    [Range(.1f, 10)] public float projectileSize = 1; // The size of the projectile

    [Header("Rotation")]
    public bool useRotation = false; // If true, the projectile will rotate at a constant speed

    public bool useAngularAcceleration = false; // If true, the projectile will start rotating at a slower speed and Accelerate to the start speed, or the opposite

    [Range(-180f, 180f)] public float rotateStartSpeed = 0; // The speed at which the projectile starts

    [Range(-180f, 180f)] public float rotateEndSpeed = 0; // The speed at which the projectile ends

    public AnimationCurve rotateAccelerationCurve; // The Acceleration curve that the projectile will follow
}

