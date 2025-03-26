using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Bullet_Pool_System : MonoBehaviour // By Samuel White // Add this script to the scene. It will create a pool of bullets that can be used and returned.
{ 
    [Header("Bullet Pool Settings")]
    
    public static Bullet_Pool_System instance; // Set instance to be acessed by other scripts

    [System.Serializable]
    public class BulletType // Create a class for each type of bullet
    {
        public string name = "Bullet"; // The name of the bullet type

        public GameObject prefab; // The prefab of the bullet type
        public int poolSize = 200; // The pool size of the bullet type

        public Queue<GameObject> pool = new(); // The pool of the bullet type // https://discussions.unity.com/t/queues-in-unityscript/61623 < Thank you Unity Forums
        public Queue<FieldInfo> fieldInfos = new(); // The Scriptable Object field infos of the bullet type

        public int PoolCount => pool.Count; // The count of the pool
    }
    public List<BulletType> bulletTypes = new(); // The list of bullet types

    void Start()
    {
        instance = this;
        foreach (var item in bulletTypes)
        {
            GameObject folder = Instantiate(new GameObject(), new(), Quaternion.identity);
            folder.name = item.name + " Folder";
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject bullet = Instantiate(item.prefab);

                bullet.transform.SetParent(folder.transform);
                
                bullet.name = "Bullet " + i;
                bullet.SetActive(false);
                item.pool.Enqueue(bullet);
                // Ensure the component at index 1 has the field "scriptable_Object"
                var component = bullet.GetComponentAtIndex(1);
                if (component != null)
                {
                    var fieldInfo = component.GetType().GetField("scriptable_Object");
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
                    Debug.LogWarning("Component at index 1 not found on bullet prefab");
                }
            }
        }
    }

    public (GameObject, FieldInfo) GetBullet(int ID) // Provide bullet to calling script from the pool
    {
        if (bulletTypes.Count > 0 && bulletTypes[ID].pool.Count > 0)
        {
            GameObject bullet = bulletTypes[ID].pool.Dequeue();
            FieldInfo fieldInfo = bulletTypes[ID].fieldInfos.Dequeue();
            // bullet.SetActive(true);
            //print($"{bullet} : {fieldInfo}");
            return (bullet, fieldInfo);
        }
        else
        {
            // Optionally expand the pool if needed
            // GameObject bullet = Instantiate(bulletPrefab);
            // return bullet;
            Debug.LogWarning($"No bullets in the pool {ID}");
            return (null, null);
        }
    }

    public void ReturnBullet(GameObject bullet, int BID, FieldInfo BFI) // Recieved bullets are deactivated and returned to the pool
    {
        bullet.SetActive(false);
        bulletTypes[BID].pool.Enqueue(bullet);
        if (BFI != null) bulletTypes[BID].fieldInfos.Enqueue(BFI);
        else Debug.LogError("Field Info not found");
        //print($"Items Remaining: {bulletTypes[BID].PoolCount}. Field Info: {BFI}");
    }
}
