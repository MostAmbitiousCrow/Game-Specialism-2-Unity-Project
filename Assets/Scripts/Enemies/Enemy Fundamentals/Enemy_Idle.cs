using UnityEngine;

public class Enemy_Idle : IEnemyState // By Samuel White
{
    //========================================
    // The Idle aspect of the enemy.
    // Enemy will idle until it is triggered to move or attack.
    //========================================
    

    public void OnEnter(Enemy_Character_Data data)
    {
        return;
    }

    public void OnExit(Enemy_Character_Data data)
    {
        return;
    }

    public void OnDeath(Enemy_Character_Data data)
    {
        data.ChangeState(data.IdleState);
        data.gameObject.SetActive(false);
        data.ReturnEnemy();
    }

    public void TriggerLeave(Enemy_Character_Data data)
    {
        data.reverseMovement = true;
        data.ChangeState(data.MoveState);
    }
}
