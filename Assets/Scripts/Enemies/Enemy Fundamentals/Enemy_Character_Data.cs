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

    public SO_Standard_Enemy_Movement movementData;
    public ScriptableObject enemyFunctionalityData;
    public SO_Standard_Enemy_Attack attackData;
    public SO_Proj_Eni_Bas projectileData;

    public New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType spawnType;
    public Vector3 targetPosition;

    [HideInInspector] public Material enemyMaterial;

    void Awake()
    {
        // Initialize the current state to a default state
        currentState = IdleState;
        currentState.OnEnter(this);
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
}

public interface IEnemyState
{
    void OnEnter(Enemy_Character_Data enemy_Character_Data);
    void OnExit(Enemy_Character_Data enemy_Character_Data);
    void OnHurt(Enemy_Character_Data enemy_Character_Data);
    void OnDeath(Enemy_Character_Data enemy_Character_Data);
}
