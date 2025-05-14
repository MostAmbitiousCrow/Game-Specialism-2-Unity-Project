using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Shoot_Flight : MonoBehaviour // By Samuel White
{
    [SerializeField] Player_Character_Data playerData;

    [Header("Player Shoot Controls")]
    [SerializeField] private ScriptableObject projectileData;
    [SerializeField] private bool isShooting = false;
    [SerializeField] float fireRate = .2f;
    [SerializeField] private Transform shootPointA, shootPointB;
    private float t = 0;
    private bool lG;
    [Space(10)]
    [SerializeField] Sprite[] bullet_Sprites;
    private int spriteCount;
    private int currentBullet;

    [Header("Player Character")]
    public Transform Character;

    [Header("Ring")]
    [SerializeField] Transform[] enemyDetectRings;
    [SerializeField] Material enemyDetectRingMaterial;

    [Header("Enemy Detection")]
    [Range(1, 20f)] [SerializeField] float detectRadius = 5;
    [Range(1, 20)] [SerializeField] float detectRange = 10;

    [Range(0, 4)] [SerializeField] int closestEnemiesRange = 2;

    public List<Transform> detectedEnemies;
    public static Transform targetEnemy;
    [SerializeField] LayerMask enemyLayer;

    [Header("Freeze Meter")]
    public bool freezeModeActive = false;
    public float freezeMeter = 0;
    public float freezeMeterMax = 100;
    [SerializeField] float freezeModeTime = 16;
    [SerializeField] float freezeMeterDecayRate = 1;
    [Space(10)]
    [SerializeField] Sprite[] frozenBullet_Sprites;

    [Header("Debug")]
    [SerializeField] bool enableDebug = true;
    
    void Start()
    {
        transform.GetChild(0).parent = null; // Unparent the rings
        playerData.view = Camera.main.transform;
        spriteCount = bullet_Sprites.Length;
    }

    void Update()
    {
        if(GameData.isPaused) return;
        Shooting();
        DetectEnemies();
        UpdateRings();
    }

    void DetectEnemies()
    {
        detectedEnemies.Clear();
        Vector3 pPos = transform.position;
        Vector3 dir = (transform.position - playerData.view.position).normalized;
        Collider[] colliders = Physics.OverlapCapsule(pPos, transform.position + dir * detectRange, detectRadius / 2, enemyLayer);
        // https://roundwide.com/physics-overlap-capsule/

        if (colliders.Length == 0) { targetEnemy = null; Character.rotation = Quaternion.identity;return; }

        foreach (var item in colliders)
        {
            detectedEnemies.Add(item.transform);
        }

        detectedEnemies.Sort((t1, t2) => // https://discussions.unity.com/t/sorting-a-list-by-distance-to-an-object/178943/3
        {
            return Vector3.Distance(t1.transform.position, transform.position)
                .CompareTo(Vector3.Distance(t2.transform.position, transform.position));
        });
        targetEnemy = detectedEnemies?[0]; // Assign Target Enemy to Player Projectiles
        Character.rotation = Quaternion.LookRotation(targetEnemy.position - transform.position); // Rotate player to face target enemy
    }

    void Shooting()
    {
        if (isShooting || Settings_Manager.playerAutoShoot) 
        {
            t += Global_Game_Speed.GetDeltaTime();
            if (t >= fireRate)
            {
                Shoot();
                t = 0;
            }
        }
    }

    void UpdateRings()
    {
        int c = Mathf.Clamp(detectedEnemies.Count, 0, closestEnemiesRange); // Count of Targetted enemies from closest enemy range.

        foreach (var item in enemyDetectRings) item.gameObject.SetActive(false); // TODO - Temporary fix, optimize this to only disable the rings that are not needed.

        for (int i = 0; i < c; i++)
        { enemyDetectRings[i].position = detectedEnemies[i].position + new Vector3(0, 0, -.1f); enemyDetectRings[i].gameObject.SetActive(true); }
        // if (c < closestEnemiesRange)
        // {

        //     for (int i = c; i < closestEnemiesRange; i++)
        //     { enemyDetectRings[i].position = new(); enemyDetectRings[i].gameObject.SetActive(false); }
        // }
    }

    public void Shoot()
    {
        if (detectedEnemies.Count != 0)
        {
            for (int i = 0; i < Mathf.Clamp(detectedEnemies.Count, 0, closestEnemiesRange); i++)
            {
                Projectile_Player_Flight p = Bullet_Pool_System.instance.GetPlayerBullet(playerData.playerNumber);
                if (p != null)
                {
                    Transform pos = lG ? shootPointA : shootPointB;
                    Quaternion look = detectedEnemies.Count == 0 ? Quaternion.LookRotation(transform.forward)
                        : Quaternion.LookRotation(detectedEnemies[i].position - pos.position);
                    p.transform.SetPositionAndRotation(pos.position, look);

                    p.spriteRenderer.sprite = freezeModeActive ? frozenBullet_Sprites[currentBullet] : bullet_Sprites[currentBullet];
                    currentBullet = (currentBullet + 1) % spriteCount; // Damn that's cool! (if current bullet is modular to the spritecount, set as zero) https://discussions.unity.com/t/c-what-is/505394/4

                    p.gameObject.SetActive(true);
                    lG = !lG;
                }
                else Debug.LogWarning("Failed to get bullet from pool.");
            }
        }
        else
        {
            Projectile_Player_Flight p = Bullet_Pool_System.instance.GetPlayerBullet(playerData.playerNumber);
            if (p != null)
            {
                Transform pos = lG ? shootPointA : shootPointB;
                // Vector3 dir = transform.position - playerData.view.position;
                Quaternion rot = Quaternion.LookRotation(transform.forward);
                p.transform.SetPositionAndRotation(pos.position, rot);

                p.spriteRenderer.sprite = freezeModeActive ? frozenBullet_Sprites[currentBullet] : bullet_Sprites[currentBullet];
                currentBullet = (currentBullet + 1) % spriteCount;

                p.gameObject.SetActive(true);
                lG = !lG;
            }
            else
            {
                Debug.LogWarning("Failed to get bullet from pool.");
            }
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed) // Button pressed
        {
            isShooting = true;
            Debug.Log("Shooting started.");
        }
        else if (context.canceled) // Button released
        {
            isShooting = false;
            Debug.Log("Shooting stopped.");
        }
    }

    public void OnFreezeMeter(InputAction.CallbackContext  context)
    {
        if (context.performed && freezeMeter >= freezeMeterMax && !freezeModeActive)
        {
            freezeModeActive = true;
            freezeMeter = freezeMeterMax;
            StartCoroutine(FreezeModeTimer());
            ParticleManager.instance.PlayPlayerParticle(ParticleManager.PlayerParticlesType.PlayerActivateFreezeMode, transform.position);
        }
    }

    public void UpdateFreezeMeter()
    {
        if (!freezeModeActive)
        {
            freezeMeter += 1;
        }
    }

    IEnumerator FreezeModeTimer()
    {
        while (freezeModeActive)
        {
            freezeMeter -=  Global_Game_Speed.GetDeltaTime() / freezeModeTime;
            if (freezeMeter <= -.1f)
            {
                freezeModeActive = false;
                freezeMeter = 0;
                yield break;
            }
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        if (enableDebug)
        {
            Gizmos.color = Color.green;
            if (playerData.view == null) return;

            // Calculate capsule parameters
            Vector3 pPos = transform.position;
            Vector3 dir = (transform.position - playerData.view.position).normalized;
            Vector3 pEnd = pPos + dir * detectRange;
            float radius = detectRadius / 2;

            // Draw the capsule
            Gizmos.DrawWireSphere(pPos, radius); // Draw the start sphere
            Gizmos.DrawWireSphere(pEnd, radius); // Draw the end sphere
            Gizmos.DrawLine(pPos + Vector3.up * radius, pEnd + Vector3.up * radius); // Connect top edges
            Gizmos.DrawLine(pPos - Vector3.up * radius, pEnd - Vector3.up * radius); // Connect bottom edges
            Gizmos.DrawLine(pPos + Vector3.right * radius, pEnd + Vector3.right * radius); // Connect right edges
            Gizmos.DrawLine(pPos - Vector3.right * radius, pEnd - Vector3.right * radius); // Connect left edges

            // Debug lines for direction
            Debug.DrawLine(transform.position, playerData.view.position, Color.blue); // Line to camera
            Debug.DrawLine(transform.position, transform.position + dir * detectRange, Color.red); // Line to detection range
        }
    }
}
