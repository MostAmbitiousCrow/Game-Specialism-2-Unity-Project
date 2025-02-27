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
            [Tooltip("")]
            public enum SpawnType { Single, Multiple}
            public SpawnType spawnType;
            [Tooltip("")]
            public GameObject enemy;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
