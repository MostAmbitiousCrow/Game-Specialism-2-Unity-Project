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

    public New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType spawnType;
    public Vector3 targetPosition;

    [HideInInspector] public Material enemyMaterial;
    public Animator Animator;

    [Header("Shooting")]
    public Transform[] projectileSpawnPoints;
    public Transform spawnPointRoot;

    void Awake()
    {
        // Initialize the current state to a default state
        currentState = IdleState;
        currentState.OnEnter(this);

        health = maxHealth;
        if (characterMeshRenderer != null) characterMaterial = characterMeshRenderer.material;
        else if (characterSpriteRenderer != null) characterMaterial = characterSpriteRenderer.material;
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

    [Header("Health Settings")]
    [SerializeField] int maxHealth = 5;
    [SerializeReference] float health;

    [Header("Visual Effects")]
    [SerializeField] Material characterMaterial;
    [SerializeField] MeshRenderer characterMeshRenderer;
    [SerializeField] SpriteRenderer characterSpriteRenderer;

    [SerializeField] float damageFlashDuration = 0.1f;
    [SerializeField] AnimationCurve damageFlashCurve;

    private float flashT;
    private bool flashing;

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void Damage(int value)
    {
        health -= value;
        DamageFlash();
        if(health <= 0) ReturnEnemy();
    }

    public void Heal(int value)
    {
        if(health < maxHealth) health += value;
    }

    void DamageFlash()
    {
        Debug.Log($"{name} Flashed");
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
            characterMaterial.SetFloat("_Flash", damageFlashCurve.Evaluate(Mathf.InverseLerp(0, Settings_Manager.damageFlashIntensity, flashT)));
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
    void OnHurt(Enemy_Character_Data enemy_Character_Data);
    void OnDeath(Enemy_Character_Data enemy_Character_Data);
}
