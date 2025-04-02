using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy_Character_Data : MonoBehaviour
{
    public int enemyID;
    public Enemy_Movement enemyMovement;
    public Enemy_Shooting enemyShooting;
    public ScriptableObject enemyFunctionalityData;
    public New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType spawnType;
    public Vector3 targetPosition;

    private void Awake()
    {
        if (enemyMovement == null)
            enemyMovement = GetComponent<Enemy_Movement>();
        if (enemyShooting == null)
            enemyShooting = GetComponent<Enemy_Shooting>();
    }

    public void ReturnEnemy()
    {
        if(New_Enemy_Pool_System.instance.isActiveAndEnabled)
        {
            enemyShooting.StopAttacking();
            enemyMovement.StopEnterance();
            New_Enemy_Pool_System.instance.ReturnEnemy(this);
        }
    }
}
