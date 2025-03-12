using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Player_Shoot_Flight : MonoBehaviour // By Samuel White
{
    [Header("Player Shoot Controls")]
    [SerializeField] private ScriptableObject projectileData;
    [SerializeField] private bool isShooting = false;
    [SerializeField] float fireRate = .2f;
    [SerializeField] Transform shootPointA, shootPointB;
    private float t = 0;
    private bool lG;

    [Header("Ring")]
    [SerializeField] Transform[] enemyDetectRings;
    [SerializeField] Material enemyDetectRingMaterial;

    [Header("Enemy Detection")]
    [Range(1, 9.9f)] [SerializeField] float detectRadius = 5;
    [Range(1, 16)] [SerializeField] float detectRange = 10;

    [Range(0, 4)] [SerializeField] int closestEnemiesRange = 2;

    public List<Transform> detectedEnemies;
    public static Transform targetEnemy;
    [SerializeField] LayerMask enemyLayer;

    [Header("Debug")]
    [SerializeField] bool enableDebug = true;
    [SerializeField] Mesh mesh;
    [SerializeField] bool debugAffectMaterial = false;
    
    void Update()
    {
        Shooting();
        DetectEnemies();
        UpdateRings();
    }

    private void DetectEnemies()
    {
        detectedEnemies.Clear();
        Vector3 pPos = transform.position;
        Collider[] colliders = Physics.OverlapCapsule(pPos, new(pPos.x, pPos.y, pPos.z + detectRange), detectRadius / 2, enemyLayer); // https://roundwide.com/physics-overlap-capsule/
        Debug.DrawLine(pPos, new(pPos.x, pPos.y, pPos.z + detectRange));

        if (colliders.Length == 0) { targetEnemy = null; return; }

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
    }

    private void Shooting()
    {
        if (isShooting)
        {
            t += Time.deltaTime;
            if (t >= fireRate)
            {
                Shoot();
                t = 0;
            }   
        }
    }

    private void UpdateRings()
    {
        int c = Mathf.Clamp(detectedEnemies.Count, 0, closestEnemiesRange); // Count of Targetted enemies from closest enemy range.

        foreach (var item in enemyDetectRings) item.gameObject.SetActive(false); // TODO - Temporary fix, optimize this to only disable the rings that are not needed.

        for (int i = 0; i < c; i++)
        { enemyDetectRings[i].position = detectedEnemies[i].position; enemyDetectRings[i].gameObject.SetActive(true); }
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
                (GameObject GO, FieldInfo SO) = Bullet_Pool_System.instance.GetBullet(0);
                if (GO != null && SO != null)
                {
                    Transform pos = lG ? shootPointA : shootPointB;
                    Quaternion look = detectedEnemies.Count == 0 ? Quaternion.identity : Quaternion.LookRotation(detectedEnemies[i].position - pos.position);
                    GO.transform.SetPositionAndRotation(pos.position, look);
                    SO.SetValue(GO.GetComponent(SO.DeclaringType), projectileData);
                    GO.SetActive(true);
                    lG = !lG;
                }
                else
                {
                    Debug.LogWarning("Failed to get bullet from pool.");
                }   
            }
        }
        else
        {
            (GameObject GO, FieldInfo SO) = Bullet_Pool_System.instance.GetBullet(0);
            if (GO != null && SO != null)
            {
                Transform pos = lG ? shootPointA : shootPointB;
                GO.transform.SetPositionAndRotation(pos.position, Quaternion.identity);
                SO.SetValue(GO.GetComponent(SO.DeclaringType), projectileData);
                GO.SetActive(true);
                lG = !lG;
            }
            else
            {
                Debug.LogWarning("Failed to get bullet from pool.");
            }
        }
    }

    public void Shooting(InputAction.CallbackContext context)
    {
        isShooting = context.ReadValueAsButton();
    }

    void OnDrawGizmos()
    {
        if (enableDebug)
        {
            Gizmos.color = Color.green;
            Quaternion rot = Quaternion.identity;
            Gizmos.DrawWireMesh(mesh, 0, new(transform.position.x, transform.position.y, transform.position.z + detectRange /2 - (detectRange * .25f)),
                Quaternion.Euler(90, rot.y, 0), new(detectRadius, detectRange/2, detectRadius));
            
            if (debugAffectMaterial) enemyDetectRingMaterial.SetFloat("_Radius", detectRadius * .05f);
        }
    }
}
