using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "ScriptableObjects/BulletPatterns/BulletPattern", order = 0)]
public class Bul_Pat_Adv_SO : ScriptableObject // By Samuel White
{
    public GameObject projectilePrefab;

    [Range(.1f, 100)] public float projectileStartSpeed = 10;
    [Range(.1f, 100)] public float projectileEndSpeed = 10;

    public bool useAccelleration = false;
    [Range(.1f, 100)] public float accellerationTime = 1;
    [Range(.1f, 100)] public float decellerationTime = 1;

    public bool useTilt = false;
    [Range(-180, 180)] public float projectileTilt = 15;

    [Range(.1f, 100)] public float tiltAccellerationTime = 0;
    [Range(.1f, 100)] public float tiltDecellerationTime = 0;

    [Range(0, 100)] public float projectileDamage = 1;

    [Range(.1f, 10)] public float projectileLifeTime = 5;

    public bool useScale = false;
    [Range(0, 10)] public float projectileStartSize = 1;
    [Range(0, 10)] public float projectileEndSize = 2;
    
    [Range(.1f, 100)] public float accellerateScaleTime = 1;
    [Range(.1f, 100)] public float decellerateScaleTime = 1;
}