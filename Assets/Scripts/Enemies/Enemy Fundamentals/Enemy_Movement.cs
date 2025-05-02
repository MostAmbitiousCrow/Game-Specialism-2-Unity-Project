using System.Collections;
using UnityEngine;

public class Enemy_Movement_State : IEnemyState // By Samuel White
{
    //========================================
    // The Movement aspect of the enemy.
    // Enemy will move, or spawn towards its target destination (assigned by the New Level manager).
    // It will either transition from infront, behind or via a portal.
    //========================================

    private Coroutine c;

    public void OnEnter(Enemy_Character_Data data)
    {
        switch (data.spawnType)
        {
            case New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Portal:
                c = data.StartCoroutine(Portal(data));
                Debug.Log($"{data.name} Spawned as Portal");
                break;
            case New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Behind:
                c = data.StartCoroutine(Behind(data));
                Debug.Log($"{data.name} Spawned from Behind");
                break;
            case New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Front:
                c = data.StartCoroutine(Front(data));
                Debug.Log($"{data.name} Spawned at the Front");
                break;
        }
    }

    public void OnExit(Enemy_Character_Data data)
    {
        if (c != null)
        {
            data.StopCoroutine(c);
            c = null;
        }
    }

    IEnumerator Portal(Enemy_Character_Data data)
    {
        data.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f / Global_Game_Speed.GetDeltaTime());
        data.gameObject.SetActive(true);
        data.ChangeState(data.ShootState);
        yield break;
    }

    IEnumerator Behind(Enemy_Character_Data data)
    {
        float t = 0;
        Vector3 startPos = new(0, 0, -10);
        Vector3 targetPos = data.targetPosition;
        while (t < 1)
        {
            t = Arch(t, startPos, data);
            yield return null;
        }
        // data.transform.LookAt(data.targetPosition, data.transform.forward);
        data.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
        data.ChangeState(data.ShootState);
        yield break;
    }

    IEnumerator Front(Enemy_Character_Data data)
    {
        float t = 0;
        Vector3 startPos = new(0, 0, 10);
        Vector3 targetPos = data.targetPosition;
        while (t < 1)
        {
            t = Arch(t, startPos, data);
            yield return null;
        }
        // data.transform.LookAt(data.targetPosition, data.transform.forward);
        data.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
        data.ChangeState(data.ShootState);
        yield break;
    }

    public void OnHurt(Enemy_Character_Data data)
    {
        throw new System.NotImplementedException();
    }

    public void OnDeath(Enemy_Character_Data data)
    {
        if (c != null) { data.StopCoroutine(c); c = null; }
        data.ReturnEnemy();
        data.ChangeState(data.IdleState);
    }

    static float Arch(float progress, Vector3 startPos, Enemy_Character_Data data) //Modified Code Lines From: https://gamedev.stackexchange.com/questions/183507/add-parabola-curve-to-straight-movetowards-movement
    {
        float stepSize = 1;
        float arcHeight = 1;

        // Increment our progress from 0 at the start, to 1 when we arrive.
        progress = Mathf.Min(progress + Global_Game_Speed.GetDeltaTime() * stepSize, 1.0f);

        // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
        float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);

        // Travel in a straight line from our start position to the target.        
        Vector3 nextPos = Vector3.Lerp(startPos, data.targetPosition, progress);

        // Then add a vertical arc in excess of this.
        nextPos.y += parabola * arcHeight;

        // Continue as before.
        data.transform.LookAt(nextPos, data.transform.forward);
        data.transform.position = nextPos;

        return progress;
    }
}
