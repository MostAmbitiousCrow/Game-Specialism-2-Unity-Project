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
                ECD.enemyMaterial = enemy.GetComponentInChildren<Renderer>().material;

                item.enemyPool.Enqueue(ECD);
            }
        }
    }

    public Enemy_Character_Data GetEnemy(int ID)
    {
        if (enemyTypes[ID].enemyPool.Count > 0)
        {
            Enemy_Character_Data ECD = enemyTypes[ID].enemyPool.Dequeue();
            Debug.Log($"Spawned {ECD.name}");
            return ECD;
        }
        else
        {
            Debug.LogWarning($"No {enemyTypes[ID].name}'s remaining");
            return null;
        }
    }

    public void ReturnEnemy(Enemy_Character_Data ECD)
    {
        EnemyType ET = enemyTypes[ECD.enemyID];
        ECD.gameObject.SetActive(false);
        enemyTypes[ECD.enemyID].enemyPool.Enqueue(ECD);
        Debug.Log($"{ET.name}s Remaining: {ET.enemyPool.Count}"); // Debug
    }
}
