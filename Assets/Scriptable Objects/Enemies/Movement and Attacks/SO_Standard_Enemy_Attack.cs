using UnityEngine;

[CreateAssetMenu(fileName = "Standard Enemy Attack Data", menuName = "ScriptableObjects/Enemies/Attacks/Standard Types", order = 0)]
public class SO_Standard_Enemy_Attack : ScriptableObject // By Samuel White
{
    // The Scriptable Object Class for providing enemies data on how they should attack
    [Header("Attack Settings")]

    [Tooltip("The time between each attack.")]
    [Min(0f)] public float fireInterval = .2f;
    
    [Tooltip("The delay before the enemy begins the attack process.")]
    [Min(0f)] public float initialDelay = .5f;
    
    [Tooltip("The delay before each attack process / processes.")]
    [Min(0f)] public float attackDelay = 1f;

    [Tooltip("The amount of attacks in each attack process.")]
    [Min(0f)] public int attackAmount = 1;

    [Tooltip("Should the enemy infinitely repeat the attack process?")]
    public bool infiniteAttack;

    [Tooltip("The amount of cycles the enemy will perform before leaving. Infinite attack will override this.")]
    [Min(1)] public int cycles = 0;

    [Tooltip("Should the enemy always attack directly at the player position?")]
    public bool aimAtTarget = true;

    [Space(5)]
    [Tooltip("Animatethe enemy fire point?")]
    public bool animateFirePoint = true;
    public AnimationCurve aimRotationCurveX = new();
    public AnimationCurve aimRotationCurveY = new();
    public AnimationCurve aimRotationCurveZ = new();

    [Space(10)]

    [Header("Animation")]
    public bool loopAttackAnimation;
    public float animationDelay = 0.1f;

    [Space(10)]

    [Header("Audio")]

    public EnemyCategory.EnemySoundTypes shootSound;
    public EnemyCategory.EnemySoundTypes deathSound;
}