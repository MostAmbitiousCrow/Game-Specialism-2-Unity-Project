using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test_Enemy_Shoot_Controller : MonoBehaviour // By Samuel White
{
    public UnityEvent[] attackEvents;
    public Transform player;

    [System.Serializable]
    public class AttackProjectiles
    {
        public int poolingSpawnCount;
        public List<Test_Enemy_Projectile> availableProjectiles;
        public List<Test_Enemy_Projectile> activeProjectiles;
    }
    public List<AttackProjectiles> storedProjectiles;

    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Shoot()
    {
        Debug.Log("Enemy Shoot");
        attackEvents[0].Invoke();

    }
}
