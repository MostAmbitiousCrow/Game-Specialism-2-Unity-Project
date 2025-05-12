using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty Data", menuName = "ScriptableObjects/Game Data/Difficulty", order = 1)]
public class SO_Difficulty_Data : ScriptableObject // By Samuel White
{
    [Serializable]
    public class DifficultyData
    {
        public float enemyHealth_Multiplier = 1;
        public float enemyDamage_Multiplier = 1;
        public float enemy_Multiplier = 1;
        public int playerLives = 3;
        public float playerHealth_Multiplier = 1;
    }
    public DifficultyData difficultyData;
}
