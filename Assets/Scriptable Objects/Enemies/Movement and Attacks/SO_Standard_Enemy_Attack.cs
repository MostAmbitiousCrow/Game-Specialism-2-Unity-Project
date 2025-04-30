using UnityEngine;

[CreateAssetMenu(fileName = "Standard Enemy Attack Data", menuName = "ScriptableObjects/Enemies/Attacks/Standard Types", order = 0)]
public class SO_Standard_Enemy_Attack : ScriptableObject // By Samuel White
{
    // The Scriptable Object Class for providing enemies data on how they should attack
    [Header("Attack Settings")]

    [Min(0f)] public float fireInterval = .2f;
    [Min(0f)] public float initialDelay = .5f;
    [Range(0f, 20f)] public float attackange = 20f;

    [Space(10)]

    [Header("Audio")]

    public EnemyCategory.EnemySoundTypes shootSound;
    public EnemyCategory.EnemySoundTypes deathSound;
}