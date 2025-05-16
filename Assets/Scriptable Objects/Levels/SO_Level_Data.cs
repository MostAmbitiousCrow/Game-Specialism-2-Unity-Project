using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_Data", menuName = "ScriptableObjects/Level_Data", order = 1)]
public class SO_Level_Data : ScriptableObject // By Samuel White
{
    [Serializable]
    public class Wave
    {
        public string waveName;

        [Serializable]
        public class EnemySpawn
        {
            [Header("Clone Controls")]
            public int cloneAmount = 0;
            public float cloneSpawnDelay = .5f;
            public Vector2 clonesOffset = new();

            [Tooltip("Enemy Info")]
            [System.Serializable]
            public class EnemyInfo
            {
                public string name;

                [Header("Enemy Info")]
                [Tooltip("The ID of the enemy, corresponding to the order in the list of the Enemy Pool")]
                public Enemy enemyID;
                public enum Enemy
                {
                    Imp, Succubus, Limb_Demon, LockJaw, Chef_Demon
                }

                [Tooltip("Enemy Movement Data, provided by its corresponding scriptable object data. Defines how the enemies will move.")]
                public SO_Standard_Enemy_Movement movementData;

                [Tooltip("Enemy Projectile Data, provided by its corresponding scriptable object data. Defines how the enemies projectiles will act.")]
                public SO_Proj_Eni_Bas projectileData;

                [Tooltip("Enemy Attack Data, provided by its corresponding scriptable object data. Defines how the enemies will attack.")]
                public SO_Standard_Enemy_Attack attackData;

                [Tooltip("The time of which the enemy spawns, from the duration of the beginning of the wave.")]
                public float timeOfAppearance = 1f;

                public Vector3 targetSpawnPosition = new(0, 0, 10);
                public enum SpawnType { Portal, Behind, Front }
                [Tooltip("The way the enemy/enemies will spawn into the scene. Portal: A portal will appear directly in the scene under the Target Position, the enemy/enemies will emerge from the portal based on the appearance rate. Behind: the Enemy/Enemies will appear behind the player camera and move in towards the assigned Target Position. Front: Enemy/Enemies will appear from the distance and move towards their Target Position.")]
                public SpawnType enterType;
            }
            public EnemyInfo enemyInfo;
        }
        [Header("Enemy Spawn")]
        public List<EnemySpawn> enemySpawnInfo;

        [Serializable]
        public class ObstacleSpawn
        {
            public string obstacleName;
            public GameObject obstaclePrefab;
            public Vector2 spawnPosition = new();
            public float timeOfAppearance = 1f;
        }
        [Header("Obstacle Spawn")]
        public List<ObstacleSpawn> obstacleSpawnInfo;

        [Serializable]
        public class PowerUpSpawn
        {
            public string powerUpName;
            public GameObject powerUpPrefab;
            // Insert Powerup EnumSelect Here //TODO
            public Vector3 spawnPosition = new(0, 0, 20); // Where the power-up box will spawn
            public float timeOfAppearance = 1f; // Time from the start of the wave when this object will spawn
        }
        [Header("Power-Up Spawn")]
        public List<PowerUpSpawn> powerUpSpawnInfo;

        [Header("Wave Settings")]
        public float waveDuration;
        public float waveStartTime;
        public int defeatedEnemiesReqirement = 0;

        [Header("Debug")]
        public bool debugActive = true;
        public Color debugColour = Color.red;
    }
    public List<Wave> waves;
}
