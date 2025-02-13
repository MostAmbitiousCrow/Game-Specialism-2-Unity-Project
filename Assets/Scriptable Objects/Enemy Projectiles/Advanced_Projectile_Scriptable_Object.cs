using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "ScriptableObjects/Enemies/Test/EnemyProjectiles/Advanced Projectile", order = 2)]
public class Advanced_Projectile_Scriptable_Object : ScriptableObject // By Samuel White
{
    public GameObject projectilePrefab;

    [Range(.1f, 100)] public float projectileStartSpeed = 10;
    [Range(.1f, 100)] public float projectileEndSpeed = 10;

    [Range(.1f, 100)] public float accellerationTime = 1;
    [Range(.1f, 100)] public float decellerationTime = 1;

    [Range(0, 100)] public float projectileDamage = 1;

    [Range(.1f, 10)] public float projectileLifeTime = 5;

    [Range(0, 10)] public float projectileSize = 1;
}