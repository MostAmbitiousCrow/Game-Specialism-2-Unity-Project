using System.Collections;
using UnityEngine;

public class Enemy_Character_Data : MonoBehaviour //  By Samuel White
{
    //========================================
    // The Enemy Character Data class.
    // This class is used to store an enemies data.
    // It also contains the states for the enemy character.
    //========================================

    IEnemyState currentState;

    public int enemyID;
    [HideInInspector] public Enemy_Movement_State MoveState { get; private set; } = new();
    [HideInInspector] public Enemy_Shooting_State ShootState { get; private set; } = new();
    [HideInInspector] public Enemy_Idle IdleState { get; private set; } = new();
    [HideInInspector] public Enemy_Frozen FrozenState { get; private set; } = new();

    [HideInInspector] public SO_Standard_Enemy_Movement movementData;
    [HideInInspector] public ScriptableObject enemyFunctionalityData;
    [HideInInspector] public SO_Standard_Enemy_Attack attackData;
    [HideInInspector] public SO_Proj_Eni_Bas projectileData;
    public bool reverseMovement;

    public SO_Level_Data.Wave.EnemySpawn.EnemyInfo.SpawnType spawnType;
    public Vector3 targetPosition;

    [HideInInspector] public Material enemyMaterial;
    public Animator Animator;
    public GameObject character;
    public Animator portalAnimator;
    public BoxCollider hitBox;

    [Header("Shooting")]
    public Transform[] projectileSpawnPoints;
    public Transform spawnPointRoot;

    [Header("Status")]
    public bool isFrozen = false;
    [SerializeField] float frozenValue = 0;

    [Header("Health Settings")]
    [SerializeField] int maxHealth = 5;
    [SerializeReference] float health;

    [Header("Visual Effects")]
    [SerializeField] MeshRenderer characterMeshRenderer;
    [SerializeField] SpriteRenderer characterSpriteRenderer;

    [SerializeField] float damageFlashDuration = 0.1f;
    [SerializeField] AnimationCurve damageFlashCurve;

    [Header("Freeze Settings")]
    public float frozenSpeed = 5;

    private float flashT;
    private bool flashing;

    void Awake()
    {
        // Initialize the current state to a default state
        currentState = IdleState;
        currentState.OnEnter(this);

        health = maxHealth;
        if (characterMeshRenderer != null) enemyMaterial = characterMeshRenderer.material;
        else if (characterSpriteRenderer != null) enemyMaterial = characterSpriteRenderer.material;
    }

    public void StartEnterance()
    {
        ChangeState(MoveState);
    }

    public void ChangeState(IEnemyState newState)
    {
        if (newState == null)
        {
            Debug.LogError("New state is null. Cannot change state.");
            return;
        }

        currentState?.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);
    }

    public void ReturnEnemy()
    {
        if (New_Enemy_Pool_System.instance.isActiveAndEnabled)
        {
            gameObject.SetActive(false);
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.localScale = Vector3.one;
            New_Enemy_Pool_System.instance.ReturnEnemy(this);
        }
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void TriggerLeave()
    {
        if(isFrozen) ReturnEnemy(); // If in Frozen State, JUST return the enemy.
        else // If not, reverse enter transition
        {
            reverseMovement = true;
            ChangeState(MoveState);   
        }
    }


    public void Damage(int damage, bool bulletFrozen, int playerNumber)
    {
        if (isFrozen) return; // If the enemy is frozen, do not take damage

        if (bulletFrozen)
        {
            frozenValue += .1f;
            AudioManager.PlayPlayerSound(PlayerCategory.PlayerSoundTypes.Frozen_Shot_Hit, 1);
            ParticleManager.instance.PlayEnemyParticle(ParticleManager.EnemyParticlesType.EnemyFreeze_Hit, transform.position);
            if (frozenValue >= 1)
            {
                ParticleManager.instance.PlayEnemyParticle(ParticleManager.EnemyParticlesType.EnemyFreeze, transform.position);
                GameManager.instance.AwardScore(playerNumber, GameManager.ScoreContext.Enemy_Frozen);
                frozenValue = 1;
                isFrozen = true;
                frozenValue = 0;
                ChangeState(FrozenState);
            }
        }
        else
        {
            health -= damage;
            GameManager.playerData[playerNumber].characterData.playerShoot.UpdateFreezeMeter(); // Add Freeze Meter Points to Player
            
            AudioManager.PlayEnemySound(EnemyCategory.EnemySoundTypes.Enemy_Hit, 1);
            ParticleManager.instance.PlayEnemyParticle(ParticleManager.EnemyParticlesType.EnemyCharacter_Hit, transform.position);
            GameManager.instance.AwardScore(playerNumber, GameManager.ScoreContext.Enemy_Hit);
        }
        DamageFlash();
        if(health <= 0) 
        {
            GameManager.instance.AwardScore(playerNumber, GameManager.ScoreContext.Enemy_Defeated);
            ParticleManager.instance.PlayEnemyParticle(ParticleManager.EnemyParticlesType.EnemyDeath, transform.position);
            AudioManager.PlayEnemySound(attackData.deathSound, 1);
            reverseMovement = false;
            isFrozen = false;
            ReturnEnemy();
        }
    }

    public void Heal(int value)
    {
        if(health < maxHealth) health += value;
    }

    void DamageFlash()
    {
        // Debug.Log($"{name} Flashed");
        if (!flashing) StartCoroutine(DamageFlashCoroutine());
        else flashT = 0;
    }

    IEnumerator DamageFlashCoroutine()
    {
        flashT = 0;
        flashing = true;
        while (flashT < 1)
        {
            flashT += Global_Game_Speed.GetDeltaTime() / damageFlashDuration;
            enemyMaterial.SetFloat("_Flash", damageFlashCurve.Evaluate(Mathf.InverseLerp(0, Settings_Manager.damageFlashIntensity, flashT))); // TODO Rework
            yield return new WaitForEndOfFrame();
        }
        flashing = false;
        yield break;
    }
}

public interface IEnemyState
{
    void OnEnter(Enemy_Character_Data enemy_Character_Data);
    void OnExit(Enemy_Character_Data enemy_Character_Data);
    void OnDeath(Enemy_Character_Data enemy_Character_Data);
}
