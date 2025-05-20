using UnityEngine;

public class Enemy_Frozen : IEnemyState // By Samuel White
{
    //========================================
    // The enemy frozen state.
    // Will move forward and smash when collided with the player.
    //========================================
    
    public void OnEnter(Enemy_Character_Data data)
    {
        // if (data.enemyMaterial == null)
        //     Debug.LogError($"{data.name} is Missing their Material");
        // else
        //     data.enemyMaterial.SetInt("_IsFrozen", 1);
        data.Animator.SetBool("Frozen", true);
        data.hitBox.enabled = false;
        data.transform.tag = "FrozenEnemy";

        AudioManager.PlayEnemySound(EnemyCategory.EnemySoundTypes.Enemy_Frozen, .5f);

        data.StartCoroutine(FrozenMoveForward(data));
    }

    public void OnExit(Enemy_Character_Data data)
    {
        // data.enemyMaterial.SetInt("_IsFrozen", 0);
        data.Animator.SetBool("Frozen", false);
        data.hitBox.enabled = true;
        data.transform.tag = "EnemyB";
    }

    public void OnDeath(Enemy_Character_Data data)
    {
        data.ReturnEnemy();
        data.ChangeState(data.IdleState);
    }

    private System.Collections.IEnumerator FrozenMoveForward(Enemy_Character_Data data)
    {
        while (true)
        {
            yield return new WaitUntil(() => !GameData.isPaused); // Pause coroutine when the game is paused

            // Move forward
            data.transform.Translate(data.frozenSpeed * Global_Game_Speed.GetDeltaTime() * Vector3.forward);

            // Check for collision with the player
            if (Physics.BoxCast(data.transform.position, new Vector3(1, 1, 1), Vector3.forward, out RaycastHit hit, 
                Quaternion.identity, 1, LayerMask.GetMask("Player"))) // TODO Inefficent, optimise when possible!
            {
                if (hit.collider.CompareTag("Player"))
                {
                    int playerNum = hit.collider.GetComponent<Player_Character_Data>().playerNumber;
                    GameManager.instance.AwardScore(playerNum, GameManager.ScoreContext.Enemy_Frozen_Smashed);
                    ParticleManager.instance.PlayEnemyParticle(ParticleManager.EnemyParticlesType.EnemyFreeze_Explode, data.transform.position);
                    AudioManager.PlayEnemySound(EnemyCategory.EnemySoundTypes.Enemy_Frozen_Smashed, .5f);
                    GameManager.playerData[playerNum].kills++;

                    data.Animator.SetBool("Frozen", false);
                    data.transform.tag = "EnemyB";

                    data.ReturnEnemy();
                    yield break;
                }
            }
            yield return null;
        }
    }
}
