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

        public Queue<GameObject> pool = new(); // The created pool of the enemy type // https://discussions.unity.com/t/queues-in-unityscript/61623 < Thank you Unity Forums
        public Queue<FieldInfo> fieldInfos = new(); // The Scriptable Object field infos of the bullet type

        public int PoolCount => pool.Count; // The count of the pool
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
                item.pool.Enqueue(enemy);
                // Ensure the component at index 1 has the field "scriptable_Object"
                var component = enemy.GetComponentAtIndex(1);
                if (component != null)
                {
                    var fieldInfo = component.GetType().GetField("enemyData");
                    if (fieldInfo != null)
                    {
                        item.fieldInfos.Enqueue(fieldInfo);

                    }
                    else
                    {
                        Debug.LogWarning($"Field 'scriptable_Object' not found on component {component.GetType().Name}");
                    }
                }
                else
                {
                    Debug.LogWarning("Component at index 1 not found on enemy prefab");
                }
            }
        }
    }

    public (GameObject, FieldInfo) GetBullet(int ID) // Provide bullet to calling script from the pool
    {
        if (enemyTypes.Count > 0 && enemyTypes[ID].pool.Count > 0)
        {
            GameObject enemy = enemyTypes[ID].pool.Dequeue();
            FieldInfo fieldInfo = enemyTypes[ID].fieldInfos.Dequeue();
            // enemy.SetActive(true);
            print($"{enemy} : {fieldInfo}");
            return (enemy, fieldInfo);
        }
        else
        {
            // Optionally expand the pool if needed
            // GameObject enemy = Instantiate(enemyPrefab);
            // return enemy;
            Debug.LogWarning($"No bullets in the pool {ID}");
            return (null, null);
        }
    }

    public void ReturnEnemy(GameObject enemy, int BID, FieldInfo BFI) // Recieved bullets are deactivated and returned to the pool
    {
        enemy.SetActive(false);
        enemyTypes[BID].pool.Enqueue(enemy);
        if (BFI != null) enemyTypes[BID].fieldInfos.Enqueue(BFI);
        else Debug.LogError("Field Info not found");
        print($"Items Remaining: {enemyTypes[BID].PoolCount}. Field Info: {BFI}");
    }
}
