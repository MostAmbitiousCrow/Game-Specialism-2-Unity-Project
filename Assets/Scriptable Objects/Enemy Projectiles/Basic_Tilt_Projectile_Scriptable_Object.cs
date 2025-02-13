using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "ScriptableObjects/Enemies/Test/EnemyProjectiles/Basic Tilt Projectile", order = 1)]
public class Basic_Tilt_Projectile_Scriptable_Object : ScriptableObject // By Samuel White
{
    public GameObject projectilePrefab;

    [Range(.1f, 100)] public float projectileStartSpeed = 10;
    
    [Range(-180, 180)] public float projectileTilt = 15;

    [Range(0, 100)] public float projectileDamage = 1;

    [Range(.1f, 10)] public float projectileLifeTime = 5;

    [Range(0, 10)] public float projectileSize = 1;
}