using System.Collections;
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
        target = GameData.isMultiplayer ? GetClosestPlayer(data.transform) : GameManager.playerData[0].playerObject.transform;
    }

    public void OnExit(Enemy_Character_Data data)
    {
        if (c != null)
        {
            data.StopCoroutine(c);
            c = null;
        }
        data.Animator.SetTrigger("Idle");
    }

    public void OnHurt(Enemy_Character_Data data)
    {

    }

    IEnumerator AttackProcess(Enemy_Character_Data data)
    {
        yield return new WaitForSeconds(data.attackData.initialDelay);

        while (data.attackData.cycles > 0 || data.attackData.infiniteAttack)
        {
            if (data.attackData.infiniteAttack)
            {
                if (data.attackData.attackDelay > 0)
                yield return new WaitForSeconds(data.attackData.attackDelay);

                data.Animator.SetTrigger("Attack");
                data.Animator.SetBool("Looping", data.attackData.loopAttackAnimation);

                for (int i = 0; i < data.attackData.attackAmount; i++)
                {
                    data.Animator.SetTrigger("Attack");
                    if (data.attackData.animationDelay > 0 && !data.attackData.loopAttackAnimation)
                    yield return new WaitForSeconds(data.attackData.animationDelay);
                    Attack(data);
                    if (data.attackData.fireInterval > 0)
                    yield return new WaitForSeconds(data.attackData.fireInterval);
                }
                data.Animator.SetBool("Looping", false);
                data.Animator.ResetTrigger("Attack");

                yield return null;
            }
            else
            {
                for (int i = 0; i < data.attackData.cycles; i++)
                {
                    if (data.attackData.attackDelay > 0)
                    yield return new WaitForSeconds(data.attackData.attackDelay);

                    data.Animator.SetTrigger("Attack");
                    data.Animator.SetBool("Looping", data.attackData.loopAttackAnimation);

                    for (int a = 0; a < data.attackData.attackAmount; a++)
                    {
                        data.Animator.SetTrigger("Attack");
                        if (data.attackData.animationDelay > 0 && !data.attackData.loopAttackAnimation)
                        yield return new WaitForSeconds(data.attackData.animationDelay);
                        Attack(data);
                        if (data.attackData.fireInterval > 0)
                        yield return new WaitForSeconds(data.attackData.fireInterval);
                    }
                    data.Animator.SetBool("Looping", false);

                    yield return null;
                }
            }
        }
    }

    private void Attack(Enemy_Character_Data data) //TODO Simplify
    {
        if (data.projectileSpawnPoints.Length < 2)
        {
            Projectile_Enemy p = Bullet_Pool_System.instance.GetEnemyBullet(data.projectileData.ID); // Get Enemy Bullet
            if (p != null)
            {
                Quaternion rot = Quaternion.LookRotation(data.attackData.aimAtTarget ? target.position : data.projectileSpawnPoints[0].forward * -1);
                p.transform.SetPositionAndRotation(data.transform.position, rot);
                p.scriptable_Object = data.projectileData;
                p.Target = target;
                p.gameObject.SetActive(true);
                AudioManager.PlayEnemySound(data.attackData.shootSound, 1);
            }
            else
            {
                Debug.LogWarning("Failed to get bullet from pool.");
            }
        }
        else
        {
            for (int i = 0; i < data.projectileSpawnPoints.Length; i++)
            {
                Projectile_Enemy p = Bullet_Pool_System.instance.GetEnemyBullet(data.projectileData.ID); // Get Enemy Bullet
                if (p != null)
                {
                    Quaternion rot = Quaternion.LookRotation(data.attackData.aimAtTarget ? target.position : data.projectileSpawnPoints[i].forward);
                    p.transform.SetPositionAndRotation(data.transform.position, rot);
                    p.scriptable_Object = data.projectileData;
                    p.gameObject.SetActive(true);
                    AudioManager.PlayEnemySound(data.attackData.shootSound, 1);
                }
                else
                {
                    Debug.LogWarning("Failed to get bullet from pool.");
                }
            }
        }
    }

    void RotatePoints(Enemy_Character_Data data)
    {
        for (int i = 0; i < data.projectileSpawnPoints.Length; i++)
        {
            Vector3 targetPos = data.projectileSpawnPoints[i].position;
            targetPos.y = data.transform.position.y;
            data.projectileSpawnPoints[i].LookAt(targetPos);
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
