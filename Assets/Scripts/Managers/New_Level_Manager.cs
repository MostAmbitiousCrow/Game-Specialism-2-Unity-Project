using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        StartCoroutine(WaveTimer());
    }

    private void Update()
    {
        if (GameData.isPaused) return;
        playTime += Global_Game_Speed.GetDeltaTime();
    }

    #region Wave Timer
    private IEnumerator WaveTimer()
    {
        bool waveActive = false;
        waveTime = 0;
        currentWave = 0;
        int waveCount = levelData.waves.Count;
        SO_Level_Data.Wave wave = levelData.waves[currentWave];

        //yield return new WaitForSeconds(wave.waveStartTime);

        while (true)
        {
            yield return new WaitUntil(() => !GameData.isPaused); // Resume when the game is paused

            if (waveTime >= wave.waveStartTime && !waveActive)
            {
                wave = levelData.waves[currentWave];

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
                waveCount++;
                waveActive = true;
            }
            
            if (waveTime < wave.waveStartTime)
            {
                Debug.Log("Poo");
                yield return null;
            }
            
            if (GetActiveObjects(wave) && waveTime > wave.waveDuration)
            {
                waveActive = false;
                foreach (var item in activeObjects)
                {
                    if (item.isActiveAndEnabled) item.TriggerLeave();
                }
                activeObjects.Clear();
            }
            if (!waveActive)
            {
                waveTime = 0;
                currentWave++;
                Debug.Log($"Wave {waveCount} completed.");
            }
            if (currentWave >= waveCount)
            {
                yield return new WaitForSeconds(2);
                Player_Game_UI_Manager.instance.ShowResultsMenu(true);
                Debug.Log($"Wave {waveCount} completed. Stopping Wave Spawner, Showing Results Menu.");
                break;
            }
            waveTime+= Global_Game_Speed.GetDeltaTime();
            yield return null;
        }
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

    private IEnumerator SpawnEnemyTimer(SO_Level_Data.Wave.EnemySpawn ES)
    {
        float t = 0;
        while (t < ES.enemyInfo.timeOfAppearance)
        {
            yield return new WaitUntil(() => !GameData.isPaused);
            t += Global_Game_Speed.GetDeltaTime();
        }
        SpawnEnemy(ES, new());
        yield break;
    }
    private IEnumerator SpawnClonesRoutine(SO_Level_Data.Wave.EnemySpawn ES)
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

    #region Spawn Obstacles
    private IEnumerator SpawnObstacles(SO_Level_Data.Wave wave)
    {

        yield break;
    }
    #endregion

    #region Spawn PowerUps

    private IEnumerator SpawnPowerUps(SO_Level_Data.Wave wave)
    {
        
        yield break;
    }
    #endregion

    #region Spawn Enemy

    public void SpawnEnemy(SO_Level_Data.Wave.EnemySpawn item, Vector3 offset)
    {
        Debug.Log("Activated");
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
