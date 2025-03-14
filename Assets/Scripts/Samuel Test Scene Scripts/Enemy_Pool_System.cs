using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Enemy_Pool_System : MonoBehaviour // By Samuel White // Add this script to the scene. It will create a pool of bullets that can be used and returned.
{
    [Header("Bullet Pool Settings")]
    
    public static Enemy_Pool_System instance; // Set instance to be acessed by other scripts

    [System.Serializable]
    public class EnemyType // Create a class for each type of bullet
    {
        public string name = "Enemy"; // The name of the enemy and type

        public GameObject prefab; // The prefab of the enemy type
        public int poolSize = 4; // The pool size of the enemies type

        [System.Serializable]
        public class EnemyInfo
        {
            public GameObject GO;
            public FieldInfo fiED;
            public FieldInfo fiPD;
            public FieldInfo fiTP;

            public Component SC;
        }

        public Queue<EnemyInfo> infoPool = new(); // The created pool of the enemy type // https://discussions.unity.com/t/queues-in-unityscript/61623 < Thank you Unity Forums
        //public Queue<FieldInfo> fieldInfos = new(); // The Scriptable Object field infos of the bullet type

        public int PoolCount => infoPool.Count; // The count of the pool
    }
    public List<EnemyType> enemyTypes = new(); // The list of bullet types

    void Start()
    {
        instance = this;
        foreach (var item in enemyTypes)
        {
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject enemy = Instantiate(item.prefab);
                enemy.name = $"{item.name} {i}";
                enemy.SetActive(false);
                EnemyType.EnemyInfo p = new();

                p.SC = enemy.GetComponentAtIndex(1);
                p.GO = enemy;

                // Ensure the Enemy Info Variable has values
                if (p.SC == null || p.GO == null)
                {
                    Debug.LogError("Missing GameObject (Prefab) and/or Enemy Data (Scriptable Object)");
                    return;
                }
                p.fiPD = p.SC.GetType().GetField("projectileData");
                p.fiED = p.SC.GetType().GetField("enemyData");
                p.fiTP = p.SC.GetType().GetField("destination");
                item.infoPool.Enqueue(p);
            }
        }
    }

    public (EnemyType.EnemyInfo, int) GetEnemy(int ID)
    {
        EnemyType.EnemyInfo info = enemyTypes[ID].infoPool.Dequeue();

        if (enemyTypes[ID].infoPool.Count > 0)
        {
            print($"Spawned {info.GO}");
            return (info, ID);
        }
        else
        {
            Debug.LogWarning($"No {info} remaining");
            return (null, 0);
        }
    }

    public void ReturnEnemy(EnemyType.EnemyInfo info, int ID)
    {
        info.GO.SetActive(false);
        enemyTypes[ID].infoPool.Enqueue(info);
        print($"{info.GO}s Remaining: {enemyTypes[ID].infoPool.Count}"); // Debug
    }

    //public void ReturnEnemy(GameObject enemy, int BID, FieldInfo BFI) // Recieved bullets are deactivated and returned to the pool
    //{
    //    enemy.SetActive(false);
    //    enemyTypes[BID].pool.Enqueue(enemy);
    //    if (BFI != null) enemyTypes[BID].fieldInfos.Enqueue(BFI);
    //    else Debug.LogError("Field Info not found");
    //    print($"Items Remaining: {enemyTypes[BID].PoolCount}. Field Info: {BFI}");
    //}
}
