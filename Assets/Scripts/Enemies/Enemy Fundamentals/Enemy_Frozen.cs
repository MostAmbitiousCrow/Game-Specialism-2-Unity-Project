using UnityEngine;

public class Enemy_Frozen : IEnemyState // By Samuel White
{
    //========================================
    // The Frozen aspect of the enemy.
    // Enemy will be frozen and unable to move or attack.
    //========================================

    public void OnEnter(Enemy_Character_Data data)
    {
        data.enemyMaterial.SetInt("_IsFrozen", 1);
        data.gameObject.tag = "FrozenEnemy";

        AudioManager.PlayEnemySound(EnemyCategory.EnemySoundTypes.Enemy_Frozen, 1);
    }

    public void OnExit(Enemy_Character_Data data)
    {
        data.enemyMaterial.SetInt("_IsFrozen", 1);
        data.gameObject.tag = "EnemyB";
    }

    public void OnHurt(Enemy_Character_Data data)
    {
        
    }

    public void OnDeath(Enemy_Character_Data data)
    {
        data.gameObject.SetActive(false);
        data.ReturnEnemy();
        data.ChangeState(data.IdleState);
    }
}
