using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_Level_Manager : MonoBehaviour // By Samuel White
{
    public static New_Level_Manager instance;
    [SerializeField] SO_Level_Data levelData; // Level data used to spawn enemies and obstacles
    [Space(10)]

    [SerializeField] GameObject portalPrefab; // Portal object used for spawning enemies via a portal

    public bool paused;
    public float playTime = 0;
    [SerializeField] float waveTime;
    [SerializeField] int currentWave;
    [SerializeField] bool waveActive;
    private Coroutine waveRoutine;

    [Header("Detection")]
    public List<Enemy_Character_Data> activeObjects;

    private void Awake()
    {
        paused = true;
        instance = this;
    }
    public void StartWaves()
    {
        paused = false;
        waveRoutine = StartCoroutine(WaveTimer());
    }
    public void EndWaves()
    {
        StopCoroutine(waveRoutine);

        foreach (var item in activeObjects)
        {
            item.ReturnEnemy();
        }
        Debug.Log("Waves Ended");
    }

    private void Update()
    {
        if (GameData.isPaused) return;
        playTime += Global_Game_Speed.GetDeltaTime();
    }

    #region Wave Timer
    private IEnumerator WaveTimer()
    {
        waveTime = 0;
        currentWave = 0;
        int totalWaves = levelData.waves.Count;

        while (currentWave < totalWaves)
        {
            SO_Level_Data.Wave wave = levelData.waves[currentWave];
            waveActive = false;
            waveTime = 0;

            // Wait for wave start time
            while (waveTime < wave.waveStartTime)
            {
                yield return new WaitUntil(() => !GameData.isPaused);
                waveTime += Global_Game_Speed.GetDeltaTime();
            }

            // Spawn enemies, obstacles, powerups for this wave
            if (wave.enemySpawnInfo != null)
            {
                foreach (var item in wave.enemySpawnInfo)
                {
                    if (item.cloneAmount > 0)
                    {
                        StartCoroutine(SpawnClonesRoutine(item));
                        // if (item.cloneSpawnDelay > 0) StartCoroutine(SpawnClonesRoutine(item));
                        // else 
                        // {
                        //     for (int i = 0; i < item.cloneAmount + 1; i++) // Create Clones
                        //     {
                        //         StartCoroutine(SpawnEnemyTimer(item, offset));
                        //         offset += item.clonesOffset;   
                        //     }
                        // }
                        // offset = new(); // Reset Offset
                    }
                    else if (item.enemyInfo.timeOfAppearance > 0) StartCoroutine(SpawnEnemyTimer(item));
                    else StartCoroutine(SpawnEnemyTimer(item));
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

            waveActive = true;

            // Wait for wave duration or until all required enemies are defeated
            waveTime = 0;
            while (waveTime < wave.waveDuration)
            {
                yield return new WaitUntil(() => !GameData.isPaused);
                waveTime += Global_Game_Speed.GetDeltaTime();

                // if (GetActiveObjects(wave)) // <<< Removed because this was causing the issue with waves ending too early // TODO Rework
                // {
                //     // All required enemies defeated, break early
                //     break;
                // }
            }

            // End of wave: trigger leave on all active objects
            foreach (var item in activeObjects)
            {
                if (item != null && item.isActiveAndEnabled) item.TriggerLeave();
            }
            activeObjects.Clear();
            playTime = 0;

            currentWave++;
            Debug.Log($"Wave {currentWave} completed.");
        }

        // All waves complete
        yield return new WaitForSeconds(2);
        Player_Game_UI_Manager.instance.ShowResultsMenu(true);
        Debug.Log("All waves completed. Stopping Wave Spawner, Showing Results Menu.");
    }
    #endregion

    private bool GetActiveObjects(SO_Level_Data.Wave wave)
    {
        int c = 0;
        foreach (var item in activeObjects)
        {
            if (!item.gameObject.activeSelf) c++;
        }
        return c >= wave.defeatedEnemiesReqirement;
    }

    #region Spawn Enemy Timers
    private IEnumerator SpawnEnemyTimer(SO_Level_Data.Wave.EnemySpawn ES)
    {
        // float t = 0;
        while (playTime < ES.enemyInfo.timeOfAppearance)
        {
            yield return new WaitUntil(() => !GameData.isPaused);
            // t += Global_Game_Speed.GetDeltaTime();
            yield return null;
        }
        SpawnEnemy(ES, Vector3.zero);
        yield break;
    }
    private IEnumerator SpawnClonesRoutine(SO_Level_Data.Wave.EnemySpawn ES)
    {
        Vector2 offset = new();
        // float t = 0;
        for (int i = 0; i < ES.cloneAmount + 1; i++)
        {
            if (ES.cloneSpawnDelay > 0)
            {
                while (playTime < ES.enemyInfo.timeOfAppearance + (ES.cloneSpawnDelay * i))
                {
                    yield return new WaitUntil(() => !GameData.isPaused);
                    // t += Global_Game_Speed.GetDeltaTime();
                    yield return null;
                }
            }
            else
            {
                while (playTime < ES.enemyInfo.timeOfAppearance)
                {
                    yield return new WaitUntil(() => !GameData.isPaused);
                    // t += Global_Game_Speed.GetDeltaTime();
                    yield return null;
                }
            }
            SpawnEnemy(ES, offset);
            offset += ES.clonesOffset;
            // t = 0;
            yield return null;
        }
        yield break;
    }
    #endregion

    #region Spawn Obstacles
    private IEnumerator SpawnObstacles(SO_Level_Data.Wave wave)
    {
        // Implement obstacle spawning logic here
        yield break;
    }
    #endregion

    #region Spawn PowerUps

    private IEnumerator SpawnPowerUps(SO_Level_Data.Wave wave)
    {
        // Implement powerup spawning logic here
        yield break;
    }
    #endregion

    #region Spawn Enemy

    public void SpawnEnemy(SO_Level_Data.Wave.EnemySpawn item, Vector3 offset)
    {
        Enemy_Character_Data ECD = New_Enemy_Pool_System.instance.GetEnemy((int)item.enemyInfo.enemyID);
        ECD.gameObject.SetActive(true);
        ECD.targetPosition = item.enemyInfo.targetSpawnPosition + offset;

        ECD.attackData = item.enemyInfo.attackData;
        ECD.movementData = item.enemyInfo.movementData;
        ECD.projectileData = item.enemyInfo.projectileData;

        ECD.enemyID = (int)item.enemyInfo.enemyID;
        ECD.spawnType = item.enemyInfo.enterType;
        activeObjects.Add(ECD);
        ECD.StartEnterance();
        // Debug.Log($"{ECD.name} Spawned. Enter Type: {ECD.spawnType}");
    }
    #endregion

    #region Debug
    private void OnDrawGizmosSelected()
    {
        if (levelData == null) return;
        foreach (var w in levelData.waves)
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
