using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_Enemy_Pool_System : MonoBehaviour // By Samuel White
{
    [Header("Bullet Pool Settings")]
    
    public static New_Enemy_Pool_System instance; // Set instance to be acessed by other scripts

    [System.Serializable]
    public class EnemyType // Create a class for each type of bullet
    {
        public string name = "Insert Enemy Name"; // The name of the enemy and type

        public GameObject prefab; // The prefab of the enemy type
        public int poolSize = 4; // The pool size of the enemies type

        // [System.Serializable]
        // public class EnemyInfo
        // {
        //     public GameObject GO;
        //     [HideInInspector]
        //     public Enemy_Character_Data ECD;
        // }
        [HideInInspector]
        public Enemy_Character_Data ECD;

        // public Queue<EnemyInfo> infoPool = new(); // The created pool of the enemy type // https://discussions.unity.com/t/queues-in-unityscript/61623 < Thank you Unity Forums
        public Queue<Enemy_Character_Data> enemyPool = new(); // The created pool of the enemy type scripts // https://discussions.unity.com/t/queues-in-unityscript/61623 < Thank you Unity Forums
    }
    public List<EnemyType> enemyTypes = new(); // The list of bullet types

    void Start()
    {
        instance = this;
        foreach (var item in enemyTypes)
        {
            for (int i = 0; i < item.poolSize; i++)
            {
                if (item.prefab == null) { Debug.LogError("Missing GameObject (Prefab)"); return; }

                GameObject enemy = Instantiate(item.prefab);
                enemy.name = $"{item.name} {i}";
                enemy.SetActive(false);

                Enemy_Character_Data ECD = enemy.GetComponent<Enemy_Character_Data>();
                item.enemyPool.Enqueue(ECD);

                // EnemyType.EnemyInfo EI = new();

                // // EI.GO = enemy;
                // // EI.ECD = enemy.GetComponent<Enemy_Character_Data>();

                // // // Ensure the Enemy Info Class has values
                // // if (!EI.ECD || !EI.GO)
                // // {
                // //     Debug.LogError("Missing Enemy (Prefab)");
                // //     return;
                // // }
                // // item.infoPool.Enqueue(EI);
            }
        }
    }

    public Enemy_Character_Data GetEnemy(int ID)
    {
        Enemy_Character_Data ECD = enemyTypes[ID].enemyPool.Dequeue();

        if (enemyTypes[ID].enemyPool.Count > 0)
        {
            print($"Spawned {ECD.name}");
            return ECD;
        }
        else
        {
            Debug.LogWarning($"No {enemyTypes[ID].name}'s remaining");
            return null;
        }
    }

    // public (EnemyType.EnemyInfo, int) GetEnemy(int ID)
    // {
    //     EnemyType.EnemyInfo info = enemyTypes[ID].infoPool.Dequeue();

    //     if (enemyTypes[ID].infoPool.Count > 0)
    //     {
    //         print($"Spawned {info.GO}");
    //         return (info, ID);
    //     }
    //     else
    //     {
    //         Debug.LogWarning($"No {info} remaining");
    //         return (null, 0);
    //     }
    // }

    public void ReturnEnemy(Enemy_Character_Data ECD)
    {
        EnemyType ET = enemyTypes[ECD.enemyID];
        ECD.gameObject.SetActive(false);
        enemyTypes[ECD.enemyID].enemyPool.Enqueue(ECD);
        print($"{ET.name}s Remaining: {ET.enemyPool.Count}"); // Debug
    }

    // public void ReturnEnemy(EnemyType.EnemyInfo info, int ID)
    // {
    //     info.GO.SetActive(false);
    //     enemyTypes[ID].infoPool.Enqueue(info);
    //     print($"{info.GO}s Remaining: {enemyTypes[ID].infoPool.Count}"); // Debug
    // }

    //public void ReturnEnemy(GameObject enemy, int BID, FieldInfo BFI) // Recieved bullets are deactivated and returned to the pool
    //{
    //    enemy.SetActive(false);
    //    enemyTypes[BID].pool.Enqueue(enemy);
    //    if (BFI != null) enemyTypes[BID].fieldInfos.Enqueue(BFI);
    //    else Debug.LogError("Field Info not found");
    //    print($"Items Remaining: {enemyTypes[BID].PoolCount}. Field Info: {BFI}");
    //}
}
