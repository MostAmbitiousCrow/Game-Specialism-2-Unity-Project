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
            public class EnemyInfo
            {
                [Header("Enemy Info")]
                [Tooltip("The ID of the enemy, corresponding to the order in the list of the Enemy Pool")]
                public int enemyID;
                [Tooltip("Enemy Data, provided by its corresponding scriptable object data.")]
                public ScriptableObject enemyData;
                [Tooltip("The time")]
                public float timeOfAppearance = 1f;
                public enum SpawnType { Portal, Behind, Front }
                [Tooltip("The way the enemy/enemies will spawn into the scene. Portal: A portal will appear directly in the scene under the Target Position, the enemy/enemies will emerge from the portal based on the appearance rate. Behind: the Enemy/Enemies will appear behind the player camera and move in behind ")]
                public SpawnType enterType;
                [Tooltip("")]
                public Vector3 targetPos = new();

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
    public List<Wave> waves;

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
