using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Projectile Data", menuName = "ScriptableObjects/Projectiles/Enemy Projectiles/Basic Enemy Projectile", order = 0)]
public class SO_Proj_Eni_Bas : ScriptableObject // By Samuel White
{
    [Header("Projectile Settings")]

    [Header("ID")]
    [Tooltip("Player = 0, Imp = 1, Succubus = 2, Limb Demon = 3, LockJaw = 4, Chef Demon = 5.")]
    [Range(0, 10)] public int ID = 0; // The ID of the projectile

    [Header("Movement")]
    public bool useMoveAcceleration = false; // If true, the projectile will start at a slower speed and Accelerate to the start speed, or the opposite

    [Range(0, 10f)] public float moveStartSpeed = 2; // The speed at which the projectile starts

    [Range(0, 10f)] public float moveEndSpeed = 0; // The speed at which the projectile ends

    public AnimationCurve moveAccelerationCurve; // The Acceleration curve that the projectile will follow

    [Header("Homing Options")]
    public bool useHoming = false; // If true, the projectile will home in on the player

    public bool useHomingAcceleration = false; // If true, the projectile will start homing at a slower speed and Accelerate to the start speed, or the opposite

    [Range(0, 10f)] public float homeStartStrength = 1; // The start strength of the homing

    [Range(0, 10f)] public float homeEndStrength = 1; // The end strength of the homing

    public AnimationCurve homeAccelerationCurve; // The Acceleration curve that the homing will follow

    [Header("Damage")]
    [Range(0, 100)] public int projectileDamage = 1; // The damage the projectile will deal

    [Header("Life Time")]
    [Range(.1f, 20f)] public float projectileLifeTime = 5; // The time before the projectile is disabled/returned to pool

    [Header("Size")]
    [Range(.1f, 10)] public float projectileSize = 1; // The size of the projectile

    [Header("Rotation")]
    public bool useRotation = false; // If true, the projectile will rotate at a constant speed

    public bool useAngularAcceleration = false; // If true, the projectile will start rotating at a slower speed and Accelerate to the start speed, or the opposite

    [Range(-180f, 180f)] public float rotateStartSpeedX = 0; // The speed at which the projectile starts

    [Range(-180f, 180f)] public float rotateEndSpeedX = 0; // The speed at which the projectile ends

    public AnimationCurve rotateAccelerationCurveX; // The Acceleration curve that the projectile will follow

    [Space(5)]

    [Range(-180f, 180f)] public float rotateStartSpeedY = 0; // The speed at which the projectile starts

    [Range(-180f, 180f)] public float rotateEndSpeedY = 0; // The speed at which the projectile ends

    public AnimationCurve rotateAccelerationCurveY; // The Acceleration curve that the projectile will follow

    [Header("Audio")]
    public EnemyCategory.EnemySoundTypes destroySound; // The sound the projectile will play when returned to the pool
    public EnemyCategory.EnemySoundTypes travelSound; // The sound the projectile will make while traveling
}

