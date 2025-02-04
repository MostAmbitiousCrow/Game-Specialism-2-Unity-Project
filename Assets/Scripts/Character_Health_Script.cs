using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "ScriptableObjects/CharacterHealth", order = 1)]
public class CharacterHealth : ScriptableObject
{
    public string prefabName;

    public float health;
}