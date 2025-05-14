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
        c = data.StartCoroutine(ChooseMovement(data));
    }

    IEnumerator ChooseMovement(Enemy_Character_Data data)
    {
        switch (data.spawnType)
        {
            case SO_Level_Data.Wave.EnemySpawn.EnemyInfo.SpawnType.Portal:
                yield return Portal(data);
                break;
            case SO_Level_Data.Wave.EnemySpawn.EnemyInfo.SpawnType.Behind:
                yield return Behind(data);
                break;
            case SO_Level_Data.Wave.EnemySpawn.EnemyInfo.SpawnType.Front:
                yield return Front(data);
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
        data.character.SetActive(data.reverseMovement);

        data.portalAnimator.gameObject.SetActive(true);
        data.portalAnimator.Play(0);
        data.transform.SetPositionAndRotation(data.targetPosition, Quaternion.LookRotation(-Vector3.forward, Vector3.up));

        yield return new WaitForSeconds(.5f / Settings_Manager.gameSpeed);
        data.character.SetActive(!data.reverseMovement);

        yield return new WaitForSeconds(.5f / Settings_Manager.gameSpeed);

        if(!data.reverseMovement)data.ChangeState(data.ShootState);
        else data.ReturnEnemy();
        yield break;
    }

    IEnumerator Behind(Enemy_Character_Data data)
    {
        float t = data.reverseMovement ? 1 : 0;
        Vector3 endPos = data.targetPosition;
        Vector3 startPos = new(endPos.x, endPos.y, -20);

        while (data.reverseMovement ? t > 0 : t < 1)
        {
            t = Arch(t, startPos, endPos, data);
            yield return null;
        }

        data.transform.rotation = Quaternion.LookRotation(-Vector3.forward, Vector3.up);

        if (!data.reverseMovement) data.ChangeState(data.ShootState);
        else data.ReturnEnemy();
    }

    IEnumerator Front(Enemy_Character_Data data)
    {
        float t = data.reverseMovement ? 1 : 0;
        Vector3 endPos = data.targetPosition;
        Vector3 startPos = new(endPos.x, endPos.y, endPos.z + 40);

        while (data.reverseMovement ? t > 0 : t < 1)
        {
            t = Arch(t, startPos, endPos, data);
            yield return null;
        }

        data.transform.rotation = Quaternion.LookRotation(-Vector3.forward, Vector3.up);

        if (!data.reverseMovement) data.ChangeState(data.ShootState);
        else data.ReturnEnemy();
    }

    public void OnDeath(Enemy_Character_Data data)
    {
        if (c != null) { data.StopCoroutine(c); c = null; }
        data.ReturnEnemy();
        data.ChangeState(data.IdleState);
    }

    static float Arch(float progress, Vector3 startPos, Vector3 endPos, Enemy_Character_Data data) //Modified Code Lines From: https://gamedev.stackexchange.com/questions/183507/add-parabola-curve-to-straight-movetowards-movement
    {
        float stepSize = 1;
        float arcHeight = 2;

        progress = data.reverseMovement
            ? Mathf.Max(progress - Global_Game_Speed.GetDeltaTime() * stepSize, 0.0f)
            : Mathf.Min(progress + Global_Game_Speed.GetDeltaTime() * stepSize, 1.0f);

        float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);
        Vector3 nextPos = Vector3.Lerp(startPos, endPos, progress);
        nextPos.y += parabola * arcHeight;

        data.transform.LookAt(nextPos, data.transform.forward);
        data.transform.position = nextPos;

        return progress;
    }
}
