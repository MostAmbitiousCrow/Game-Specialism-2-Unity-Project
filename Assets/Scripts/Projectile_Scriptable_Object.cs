using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile_Scriptable_Object", menuName = "Projectile_Scriptable_Object", order = 0)]
public class Projectile_Scriptable_Object : ScriptableObject
{
    public float projectileSpeed = 10;
    public float projectileDamage = 1;
}
