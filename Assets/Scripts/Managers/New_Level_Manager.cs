using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_Level_Manager : MonoBehaviour // By Samuel White
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;

        [System.Serializable]
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
                public int enemyID;

                [Tooltip("Enemy Data, provided by its corresponding scriptable object data.")]
                public SO_Standard_Enemy_Movement enemyData;

                [Tooltip("Enemy Projectile Data, provided by its corresponding scriptable object data. Defines how the enemies projectiles will work.")]
                public SO_Projectile_Enemy_Attack projectileData;

                [Tooltip("The time of which the enemy spawns, from the duration of the beginning of the wave.")]
                public float timeOfAppearance = 1f;
                public enum SpawnType { Portal, Behind, Front }
                [Tooltip("The way the enemy/enemies will spawn into the scene. Portal: A portal will appear directly in the scene under the Target Position, the enemy/enemies will emerge from the portal based on the appearance rate. Behind: the Enemy/Enemies will appear behind the player camera and move in towards the assigned Target Position. Front: Enemy/Enemies will appear from the distance and move towards their Target Position.")]
                public SpawnType enterType;
            }
            public EnemyInfo enemyInfo;
        }
        [Header("Enemy Spawn")]
        public List<EnemySpawn> enemySpawnInfo;
        
        [System.Serializable]
        public class ObstacleSpawn
        {
            public string obstacleName;
            public GameObject obstaclePrefab;
            public Vector2 spawnPosition = new();
            public float timeOfAppearance = 1f;
        }
        [Header("Obstacle Spawn")]
        public List<ObstacleSpawn> obstacleSpawnInfo;

        [System.Serializable]
        public class PowerUpSpawn
        {
            public string powerUpName;
            public GameObject powerUpPrefab;
            public float timeOfAppearance = 1f;
        }
        [Header("Power-Up Spawn")]
        public List<PowerUpSpawn> powerUpSpawnInfo;

        [Header("Wave Settings")]
        public float waveDuration;
        public float waveStartTime;
        public int defeatedEnemiesReqirement = 0;

        [Header("Detection")]
        public List<GameObject> activeObjects;

    }
    public List<Wave> waves;

    public bool paused;
    public float playTime = 0;

    // Start is called before the first frame update
    public void StartWaves()
    {
        paused = false;
        StartCoroutine(WaveTimer());
    }

    private void FixedUpdate()
    {
        if (paused) return;
        playTime += Time.deltaTime;
    }


    private IEnumerator WaveTimer()
    {
        bool waveActive = true;
        float waveTime = 0;
        int currentWave = 0;
        int waveCount = waves.Count;
        Wave wave = waves[currentWave];

        while (true)
        {
            if (waveTime >= wave.waveStartTime && !waveActive)
            {
                wave = waves[currentWave];

                if (wave.enemySpawnInfo != null)
                {
                    StartCoroutine(SpawnEnemies(wave));
                }
                if (wave.obstacleSpawnInfo != null)
                {
                    StartCoroutine(SpawnObstacles(wave));
                }
                if (wave.powerUpSpawnInfo != null)
                {
                    StartCoroutine(SpawnPowerUps(wave));
                }
                currentWave++;
                waveActive = true;
            }

            if (currentWave >= waveCount) break;
            if (GetActiveObjects(wave))
            {
                waveActive = false;
                wave.activeObjects.Clear();
            }
            yield return new WaitForFixedUpdate();
            waveTime+= Time.fixedDeltaTime;
        }
    }

    private bool GetActiveObjects(Wave wave)
    {
        int c = 0;
        foreach (var item in wave.activeObjects)
        {
            if (!item.activeSelf) c++;
        }
        if (c >= wave.defeatedEnemiesReqirement) return true;
        return false;
    }

    private IEnumerator SpawnEnemies(Wave wave)
    {
        List<Wave.EnemySpawn> ESI = wave.enemySpawnInfo;
        foreach (var ES in ESI)
        {
            if (ES.cloneAmount > 1)
            {
                if (ES.enemyInfo.timeOfAppearance > 0) yield return new WaitForSeconds(ES.enemyInfo.timeOfAppearance);
                // StartCoroutine(CloneRoutine(ES));
                for (int i = 0; i < ES.cloneAmount; i++)
                {
                    if (ES.cloneSpawnDelay > 0) yield return new WaitForSeconds(ES.cloneSpawnDelay);
                    wave.activeObjects.Add(New_Enemy_Pool_System.instance.GetEnemy(ES.enemyInfo.enemyID).gameObject);
                }
            }
            else
            {
                if (ES.enemyInfo.timeOfAppearance > 0) yield return new WaitForSeconds(ES.enemyInfo.timeOfAppearance);
                wave.activeObjects.Add(New_Enemy_Pool_System.instance.GetEnemy(ES.enemyInfo.enemyID).gameObject);
            }
        }
    }

    private IEnumerator SpawnObstacles(Wave wave)
    {

        yield break;
    }

    private IEnumerator SpawnPowerUps(Wave wave)
    {
        
        yield break;
    }

    private IEnumerator CloneRoutine(Wave.EnemySpawn ES)
    {
        Vector2 offset = ES.clonesOffset;
        for (int i = 0; i < ES.cloneAmount; i++)
        {
            if (ES.cloneSpawnDelay > 0) yield return new WaitForSeconds(ES.cloneSpawnDelay);
            New_Enemy_Pool_System.instance.GetEnemy(ES.enemyInfo.enemyID);
        }
    }
}
