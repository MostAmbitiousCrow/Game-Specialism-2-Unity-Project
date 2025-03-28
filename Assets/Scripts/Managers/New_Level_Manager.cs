using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class New_Level_Manager : MonoBehaviour // By Samuel White
{
    public static New_Level_Manager instance;

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
                public SO_Standard_Enemy_Movement movementData;

                [Tooltip("Enemy Projectile Data, provided by its corresponding scriptable object data. Defines how the enemies projectiles will work.")]
                public SO_Projectile_Enemy_Attack projectileData;

                [Tooltip("Enemy Functionality Data. Must correspond to the enemy.")]
                public ScriptableObject enemyFunctionalityData;

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

        [Header("Detection")]
        public List<GameObject> activeObjects;

        [Header("Debug")]
        public bool debugActive = true;
        public Color debugColour = Color.red;
    }
    public List<Wave> waves;
    [SerializeField] GameObject portalPrefab; // Portal object used for spawning enemies via a portal

    public bool paused;
    public float playTime = 0;

    private void Awake()
    {
        paused = true;
        if(instance = null) instance = this;
    }
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
        bool waveActive = false;
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
                    foreach (var item in wave.enemySpawnInfo)
                    {
                        if (item.cloneAmount > 0) // Has Clones?
                        {
                            if (item.cloneSpawnDelay > 0) StartCoroutine(SpawnClonesRoutine(item)); // Has timer
                            else SpawnEnemy(item, new()); // No timer, spawn
                        }
                        else if (item.enemyInfo.timeOfAppearance > 0) StartCoroutine(SpawnEnemyTimer(item)); // No Clones, has a timer?
                        else SpawnEnemy(item, new()); // No time, just spawn enemy
                    }
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
            //if (GetActiveObjects(wave))
            //{
            //    waveActive = false;
            //    wave.activeObjects.Clear();
            //}
            yield return new WaitForFixedUpdate();
            waveTime+= Time.fixedDeltaTime;
            print(waveTime);
            yield return null;
        }
    }

    private bool GetActiveObjects(Wave wave)
    {
        int c = 0;
        foreach (var item in wave.activeObjects)
        {
            if (!item.activeSelf) c++;
        }
        return c >= wave.defeatedEnemiesReqirement;
    }

    private IEnumerator SpawnEnemyTimer(Wave.EnemySpawn ES)
    {
        if (ES.enemyInfo.timeOfAppearance > 0) 
            yield return new WaitForSeconds(ES.enemyInfo.timeOfAppearance);
        SpawnEnemy(ES, new());
        yield break;
    }
    private IEnumerator SpawnClonesRoutine(Wave.EnemySpawn ES)
    {
        Vector2 offset = new();
        for (int i = 0; i < ES.cloneAmount + 1; i++)
        {
            if (ES.cloneSpawnDelay > 0) yield return new WaitForSeconds(ES.cloneSpawnDelay);
            SpawnEnemy(ES, offset);
            offset += ES.clonesOffset;
            yield return null;
        }
        yield break;
    }

    private IEnumerator SpawnObstacles(Wave wave)
    {

        yield break;
    }

    private IEnumerator SpawnPowerUps(Wave wave)
    {
        
        yield break;
    }

    public void SpawnEnemy(Wave.EnemySpawn item, Vector3 offset)
    {
        Debug.Log("Activated");
        Enemy_Character_Data ECD = New_Enemy_Pool_System.instance.GetEnemy(item.enemyInfo.enemyID);
        ECD.gameObject.SetActive(true);
        ECD.targetPosition = item.enemyInfo.targetSpawnPosition + offset;
        ECD.enemyFunctionalityData = item.enemyInfo.enemyFunctionalityData;
        ECD.enemyMovement.movementData = item.enemyInfo.movementData;
        ECD.enemyShooting.attackData = item.enemyInfo.projectileData;
        ECD.spawnType = item.enemyInfo.enterType;
        ECD.enemyMovement.StartEnterance();
        Debug.Log($"{ECD.name} Spawned. Enter Type: {ECD.spawnType}");
    }

    #region Debug
    private void OnDrawGizmosSelected()
    {
        foreach (var w in waves)
        {
            if (!w.debugActive) return;
            Gizmos.color = w.debugColour;
            foreach (var si in w.enemySpawnInfo)
            {
                if (si.cloneAmount > 0)
                {
                    Vector2 offset = new();
                    for (int i = 0; i < si.cloneAmount + 1; i++)
                    {
                        //Gizmos.DrawWireSphere(si.enemyInfo.targetSpawnPosition + (Vector3)offset, .5f);
                        Gizmos.DrawCube(si.enemyInfo.targetSpawnPosition + (Vector3)offset, new Vector3(1, 1, 1) * .5f);
                        offset += si.clonesOffset;
                    }
                }
                else
                {
                    //Gizmos.DrawWireSphere(si.enemyInfo.targetSpawnPosition, .5f);
                    Gizmos.DrawCube(si.enemyInfo.targetSpawnPosition, new Vector3(1, 1, 1) * .5f);
                }
            }
        }
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new(0, -2.5f, 10), new(11, 4, 1));
    }
    #endregion
}
