using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "CharacterHealth/CharacterHealth", order = 1)]
public class CharacterHealthStats : ScriptableObject
{
    public int maxHealth;
    public float movementSpeed;
}