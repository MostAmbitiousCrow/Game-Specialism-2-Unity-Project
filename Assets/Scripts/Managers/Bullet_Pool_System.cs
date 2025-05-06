using System.Collections.Generic;
using UnityEngine;

public class Bullet_Pool_System : MonoBehaviour // By Samuel White // Add this script to the scene. It will create a pool of bullets that can be used and returned.
{ 
    [Header("Bullet Pool Settings")]
    
    public static Bullet_Pool_System instance; // Set instance to be acessed by other scripts

    [System.Serializable]
    public class EnemyBulletType // Create a class for each type of bullet
    {
        public string name = "Bullet"; // The name of the bullet type

        public GameObject prefab; // The prefab of the bullet type
        public int poolSize = 200; // The pool size of the bullet type

        // The pool of the bullet type // https://discussions.unity.com/t/queues-in-unityscript/61623 < Thank you Unity Forums
        public Queue<Projectile_Enemy> pool = new();

        public int PoolCount => pool.Count; // The count of the pool
    }
    public List<EnemyBulletType> enemyBulletTypes = new(); // The list of bullet types

    [System.Serializable]
    public class PlayerBullets
    {
        public string name = "Bullet"; // The name of the bullet type

        public GameObject prefab; // The prefab of the bullet type
        public int poolSize = 200; // The pool size of the bullet type

        public Queue<Projectile_Player_Flight> pool = new();

        public int PoolCount => pool.Count; // The count of the pool
    }
    public PlayerBullets playerBullets = new(); // The list of the player bullets

    void Start()
    {
        instance = this;

        // Create the enemy bullets
        foreach (var item in enemyBulletTypes)
        {
            GameObject folder = new (item.name + " Folder");
            Debug.Log($"Created {folder.name} folder");
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject bullet = Instantiate(item.prefab);

                bullet.transform.SetParent(folder.transform);
                
                bullet.name = item.name + " Bullet " + i;
                bullet.SetActive(false);
                item.pool.Enqueue(bullet.GetComponent<Projectile_Enemy>());
            }
        }

        // Create the player bullets
        GameObject folder2 = new (playerBullets.name + " Folder");
        for (int i = 0; i < playerBullets.poolSize; i++)
        {
            GameObject bullet = Instantiate(playerBullets.prefab);
            bullet.transform.SetParent(folder2.transform);
            bullet.name = playerBullets.name + " Bullet " + i;
            bullet.SetActive(false);
            playerBullets.pool.Enqueue(bullet.GetComponent<Projectile_Player_Flight>());
        }
    }

    public Projectile_Enemy GetEnemyBullet(int ID) // Provide bullet to calling script from the pool
    {
        if (enemyBulletTypes.Count > 0 && enemyBulletTypes[ID].pool.Count > 0)
        {
            Projectile_Enemy projectile = enemyBulletTypes[ID].pool.Dequeue();
            return projectile;
        }
        else
        {
            // Optionally expand the pool if really needed
            // GameObject bullet = Instantiate(bulletPrefab);
            // return bullet;
            Debug.LogWarning($"No bullets in the pool {ID}");
            return null;
        }
    }

    public void ReturnEnemyBullet(Projectile_Enemy projectile, int BID) // Recieved bullets are deactivated and returned to the pool
    {
        projectile.gameObject.SetActive(false);
        enemyBulletTypes[BID].pool.Enqueue(projectile);
    }

    public Projectile_Player_Flight GetPlayerBullet() // Provide bullet to calling script from the pool
    {
        if (playerBullets.pool.Count > 0)
        {
            Projectile_Player_Flight projectile = playerBullets.pool.Dequeue();
            return projectile;
        }
        else
        {
            Debug.LogWarning($"No bullets in the pool");
            return null;
        }
    }

    public void ReturnPlayerBullet(Projectile_Player_Flight projectile, int BID) // Recieved bullets are deactivated and returned to the pool
    {
        projectile.gameObject.SetActive(false);
        playerBullets.pool.Enqueue(projectile);
    }
}
