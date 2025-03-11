using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour // By Samuel White
{
    public class Wave
    {
        public string waveName;
        public class EnemySpawn
        {
            [Tooltip("Enemy Info")]
            public class EnemyInfo
            {
                public int enemyID;
                public ScriptableObject enemyData;
                public float timeAppearance = 1f;
                public Vector3 targetPos = new();
                public enum SpawnType { Portal, Behind, Front }
                public SpawnType enterType;

                [Header("Clone Controls")]
                [Range(1, 16)] public int amount = 1;
            }
            public EnemyInfo enemyInfo;

            [Tooltip("")]
            public int multipleEnemyCount = 2;
            [Tooltip("")]
            public Vector3 enemySpawnPos = new();
            public Vector3[] multipleEnemySpawnPoss;

            public float timeAppearance = 0;
        }
        public EnemySpawn enemySpawnInfo;

        public float waveDuration;

        public bool showDebug;
        public Color debugColor = Color.red;
    }
    [SerializeField] List<Wave> waves;

    public float playTime = 0;
    private Coroutine timerRoutine;

    // Start is called before the first frame update
    public void StartTimer()
    {
        StopCoroutine(timerRoutine);
        timerRoutine = StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        //waves[0].enemySpawnInfo.enemyInfo.enemyID
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
