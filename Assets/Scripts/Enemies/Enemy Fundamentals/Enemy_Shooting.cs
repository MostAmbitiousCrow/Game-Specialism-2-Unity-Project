using System.Collections;
using System.Reflection;
using UnityEngine;

public class Enemy_Shooting_State : IEnemyState // By Samuel White
{
    //========================================
    // The Shooting/Attacking aspect of the Enemy.
    // Attack Process is triggered by the StartAttacking function, of which is triggered by the completed movement function
    //========================================

    Coroutine c;
    Transform target;

    public void OnDeath(Enemy_Character_Data data)
    {
        if (c != null) { data.StopCoroutine(c); c = null; }
        data.ChangeState(data.IdleState);
        data.ReturnEnemy();
    }

    public void OnEnter(Enemy_Character_Data data)
    {
        c = data.StartCoroutine(AttackProcess(data));
        target = GameData.isMultiplayer ? GetClosestPlayer(data.transform) : GameData.playerOne;
    }

    public void OnExit(Enemy_Character_Data data)
    {

    }

    public void OnHurt(Enemy_Character_Data data)
    {

    }

    // Update is called once per frame
    IEnumerator AttackProcess(Enemy_Character_Data data)
    {
        yield return new WaitForSeconds(data.attackData.initialDelay);
        while (true)
        {

            Attack(data);
            yield return new WaitForSeconds(data.attackData.fireInterval);
        }
    }

    private void Attack(Enemy_Character_Data data)
    {
        (GameObject GO, FieldInfo SO) = Bullet_Pool_System.instance.GetBullet(data.projectileData.ID); // Get Enemy Bullet
        if (GO != null && SO != null)
        {
            Quaternion rot = Quaternion.LookRotation(target.position);
            GO.transform.SetPositionAndRotation(data.transform.position, rot);
            SO.SetValue(GO.GetComponent(SO.DeclaringType), data.projectileData); //TODO Might need to update this!
            GO.SetActive(true);
            AudioManager.PlayEnemySound(data.attackData.shootSound, 1);
        }
        else
        {
            Debug.LogWarning("Failed to get bullet from pool.");
        }
    }

    public static Transform GetClosestPlayer(Transform enemy)
    {
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform player in GameData.players)
        {
            float distance = Vector3.Distance(enemy.position, player.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }
}
