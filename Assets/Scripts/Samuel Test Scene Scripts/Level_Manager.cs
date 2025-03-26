using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour // By Samuel White
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        [System.Serializable]
        public class EnemySpawn
        {
            [Tooltip("Enemy Info")]
            [System.Serializable]
            public class EnemyInfo
            {
                public string name;

                [Header("Enemy Info")]
                [Tooltip("The ID of the enemy, corresponding to the order in the list of the Enemy Pool")]
                public int enemyID;

                [Tooltip("Enemy Data, provided by its corresponding scriptable object data.")]
                public ScriptableObject enemyData;

                [Tooltip("Enemy Projectile Data, provided by its corresponding scriptable object data. Defines how the enemies projectiles will work.")]
                public ScriptableObject projectileData;

                [Tooltip("The time of which the enemy spawns, from the duration of the beginning of the wave.")]
                public float timeOfAppearance = 1f;
                public enum SpawnType { Portal, Behind, Front }
                [Tooltip("The way the enemy/enemies will spawn into the scene. Portal: A portal will appear directly in the scene under the Target Position, the enemy/enemies will emerge from the portal based on the appearance rate. Behind: the Enemy/Enemies will appear behind the player camera and move in towards the assigned Target Position. Front: Enemy/Enemies will appear from the distance and move towards their Target Position.")]
                public SpawnType enterType;

                [Tooltip("The position in which the enemy will enter the scene towards.")]
                public Vector3 targetPos = new(0,0,10);

                [Header("Clone Controls")]
                [Range(1, 16)] public int cloneAmount = 1;
                [Range(0.1f, 10f)] public float cloneSpawnRate = .5f;
            }
            public List<EnemyInfo> enemyInfo;

            public float startTime = 0; // Time after the time the wave started in which enemies will spawn (a delay?)
        }
        public EnemySpawn enemySpawnInfo;

        public float waveDuration;
        public float waveStartTime;

        public bool showDebug;
        public Color debugColor = Color.red;
    }
    public List<Wave> waves;

    public float playTime = 0;
    private float nextTime;
    private bool waveActive;
    public bool paused;

    private Coroutine timerRoutine;

    [SerializeField] int currentWave = 0;
    public static int enemiesRemaining = 0;

    public void StartWaves()
    {
        if (timerRoutine != null) StopCoroutine(timerRoutine);
        paused = false;
        timerRoutine = StartCoroutine(GameTimer());
    }

    private void FixedUpdate()
    {
        if (!paused) playTime += Time.fixedDeltaTime;
    }

    IEnumerator GameTimer()
    {
        foreach (var item in waves)
        {
            Wave.EnemySpawn EP = item.enemySpawnInfo;

            nextTime = item.waveStartTime;
            yield return new WaitUntil(() => playTime > nextTime); // Only spawn enemies once the next wave time is met
            waveActive = true;
            float timeStarted = playTime;

            foreach (var enemy in EP.enemyInfo)
            {
                if (playTime! > enemy.timeOfAppearance + timeStarted) yield return null;

                if (enemy.cloneAmount > 1)
                {
                    StartCoroutine(CloneRoutine(enemy));
                    enemiesRemaining++;
                }
                else
                {
                    if (enemy.timeOfAppearance! < .1f) yield return new WaitForSeconds(enemy.timeOfAppearance);
                    GetEnemy(enemy);
                    enemiesRemaining++;
                }

            }

            while (waveActive)
            {
                if (enemiesRemaining < 1)
                {
                    waveActive = false; // If X amount enemies are defeated...
                    enemiesRemaining = 0;
                }
                yield return null;
            }
            // Next Wave
            yield return null;
        }
        yield break;
    }

    IEnumerator CloneRoutine(Wave.EnemySpawn.EnemyInfo info)
    {
        float rate = info.cloneSpawnRate;
        for (int i = 0; i < info.cloneAmount; i++) // Repeat/Add clones over a given amount at a given rate
        {
            GetEnemy(info);
            yield return new WaitForSeconds(rate);
        }
        yield break;
    }

    void GetEnemy(Wave.EnemySpawn.EnemyInfo EI)
    {
        (Enemy_Pool_System.EnemyType.EnemyInfo _EI, int _ID) = Enemy_Pool_System.instance.GetEnemy(EI.enemyID);
        {
            print($"Grabbed: {_EI.GO}");
            _EI.GO.SetActive(true);
            _EI.SC.SendMessage("ST", EI.enterType); // Trigger the enemy Transition, with its type
            _EI.fiED.SetValue(_EI.SC, EI.enemyData); // Set the enemy data to the enemy data Scriptable Object
            _EI.fiPD.SetValue(_EI.SC, EI.projectileData); // Set the projectile data the enemy will shoot
        }
    }

    private void OnDrawGizmosSelected()
    {
        return;
        foreach (var item in waves)
        {
            if (item.showDebug)
            {
                Gizmos.color = item.debugColor;
                foreach (var item1 in item.enemySpawnInfo.enemyInfo)
                {
                    
                }
            }
        }
    }
}
