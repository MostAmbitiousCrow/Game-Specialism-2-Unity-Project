using System.Collections;
using UnityEngine;

public class New_Level_Manager : MonoBehaviour // By Samuel White
{
    public static New_Level_Manager instance;
    [SerializeField] SO_Level_Data levelData; // Level data used to spawn enemies and obstacles
    [Space(10)]

    [SerializeField] GameObject portalPrefab; // Portal object used for spawning enemies via a portal

    public bool paused;
    public float playTime = 0;

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


    private IEnumerator WaveTimer()
    {
        bool waveActive = false;
        float waveTime = 0;
        int currentWave = 0;
        int waveCount = levelData.waves.Count;
        SO_Level_Data.Wave wave = levelData.waves[currentWave];

        while (true)
        {
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

    private bool GetActiveObjects(SO_Level_Data.Wave wave)
    {
        int c = 0;
        foreach (var item in wave.activeObjects)
        {
            if (!item.activeSelf) c++;
        }
        return c >= wave.defeatedEnemiesReqirement;
    }

    private IEnumerator SpawnEnemyTimer(SO_Level_Data.Wave.EnemySpawn ES)
    {
        if (ES.enemyInfo.timeOfAppearance > 0) 
            yield return new WaitForSeconds(ES.enemyInfo.timeOfAppearance);
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
        ECD.StartEnterance();
        Debug.Log($"{ECD.name} Spawned. Enter Type: {ECD.spawnType}");
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
