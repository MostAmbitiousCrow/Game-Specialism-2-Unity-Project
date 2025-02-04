using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "ScriptableObjects/CharacterHealth", order = 1)]
public class ZombieStats : ScriptableObject
{
    public int maxHealth;
    public float movementSpeed;
}