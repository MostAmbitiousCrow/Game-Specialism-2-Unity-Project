using System.Collections;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour // By Samuel White
{
    //========================================
    // The Movement aspect of the enemy.
    // Enemy will move, or spawn towards its target destination (assigned by the New Level manager).
    // It will either transition from infront, behind or via a portal.
    //========================================

    public Enemy_Character_Data data;
    public SO_Standard_Enemy_Movement movementData;
    private Coroutine coroutine;

    public void StartEnterance()
    {
        Debug.Log($"{name} Started");
       coroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopEnterance()
    {
        if (coroutine != null) StopCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        float p = 0;
        Vector3 startPos;
        switch (data.spawnType)
        {
            case New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Portal:
                gameObject.SetActive(false);
                yield return new WaitForSeconds(1); //TODO Improve
                gameObject.SetActive(true);

                Debug.Log($"{name} Spawned as Portal");
                break;
            case New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Behind:
                Debug.Log($"{name} Spawned from Behind");
                while (p < 1)
                {
                    startPos = new(0, 0, -10);
                    p = Arch(p, startPos);
                    //Debug.Log($"{name} spawning from behind | Time: {p}");
                    yield return new WaitForFixedUpdate();
                }
                //transform.LookAt(player, transform.forward);
                break;
            case New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Front:
                Debug.Log($"{name} Spawned at the Front");
                while (p < 1)
                {
                    startPos = new(0, 0, 30);
                    p = Arch(p, startPos);
                    //Debug.Log($"{name} spawning from behind | Time: {p}");
                    yield return new WaitForFixedUpdate();
                }
                break;
        }
        data.enemyShooting.StartAttacking(); // Once enemy has reached destination, start attacking
        yield break;
    }

    float Arch(float progress, Vector3 startPos) //Modified Code Lines From: https://gamedev.stackexchange.com/questions/183507/add-parabola-curve-to-straight-movetowards-movement
    {
        float stepSize = 1;
        float arcHeight = 1;

        // Increment our progress from 0 at the start, to 1 when we arrive.
        progress = Mathf.Min(progress + Time.fixedDeltaTime * stepSize, 1.0f);

        // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
        float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);

        // Travel in a straight line from our start position to the target.        
        Vector3 nextPos = Vector3.Lerp(startPos, data.targetPosition, progress);

        // Then add a vertical arc in excess of this.
        nextPos.y += parabola * arcHeight;

        // Continue as before.
        transform.LookAt(nextPos, transform.forward);
        transform.position = nextPos;

        return progress;
    }
}
