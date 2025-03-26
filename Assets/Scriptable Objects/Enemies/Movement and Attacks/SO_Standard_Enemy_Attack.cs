using UnityEngine;

[CreateAssetMenu(fileName = "Standard Enemy Movement Type", menuName = "ScriptableObjects/Enemies/Attacks/Standard Types", order = 0)]
public class SO_Projectile_Enemy_Attack : ScriptableObject
{
    [Header("Projectile Settings")]

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
    [Range(0, 100)] public float projectileDamage = 1; // The damage the projectile will deal

    [Header("Life Time")]
    [Range(.1f, 20f)] public float projectileLifeTime = 5; // The time before the projectile is disabled/returned to pool

    [Header("Size")]
    [Range(.1f, 10)] public float projectileSize = 1; // The size of the projectile

    [Header("Rotation")]
    public bool useRotation = false; // If true, the projectile will rotate at a constant speed

    public bool useAngularAcceleration = false; // If true, the projectile will start rotating at a slower speed and Accelerate to the start speed, or the opposite

    [Range(-180f, 180f)] public float rotateStartSpeed = 0; // The speed at which the projectile starts

    [Range(-180f, 180f)] public float rotateEndSpeed = 0; // The speed at which the projectile ends

    public AnimationCurve rotateAccelerationCurve; // The Acceleration curve that the projectile will follow

    [Header("Audio")]
    public AudioClip startSound; // The sound the projectile will make when fired
    public AudioClip travelSound; // The sound the projectile will make while traveling
}