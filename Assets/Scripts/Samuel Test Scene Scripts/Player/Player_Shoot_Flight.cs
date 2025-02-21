using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEngine.Assertions.Must;

public class Player_Shoot_Flight : MonoBehaviour // By Samuel White
{
    [Header("Player Shoot Controls")]
    [SerializeField] private ScriptableObject projectileData;
    [SerializeField] private bool isShooting = false;
    [SerializeField] float fireRate = .2f;
    private float t = 0;

    [Header("Ring")]
    [SerializeField] GameObject enemyDetectRing;
    [SerializeField] Material enemyDetectRingMaterial;

    [Header("Enemy Detection")]
    [Range(1, 9.9f)] [SerializeField] float detectRadius = 5;
    [Range(1, 10)] [SerializeField] float detectRange = 10;

    [Range(0, 2)] [SerializeField] int closestEnemiesRange = 2;

    [SerializeField] List<GameObject> detectedEnemies;
    [SerializeField] List<GameObject> closestEnemies;
    [SerializeField] LayerMask enemyLayer;

    [Header("Debug")]
    [SerializeField] bool enableDebug = true;
    [SerializeField] Mesh mesh;

    void Start()
    {
        enemyDetectRingMaterial = enemyDetectRing.GetComponent<Renderer>().material;
    }
    
    void Update()
    {
        Shooting();
        DetectEnemies();
    }

    private void DetectEnemies()
    {
        closestEnemies.Clear();
        detectedEnemies.Clear();
        Vector3 pPos = transform.position;
        Collider[] colliders = Physics.OverlapCapsule(pPos, new(pPos.x, pPos.y, pPos.z * 5), detectRadius, enemyLayer);
        float closestDistance = float.MaxValue;

        foreach (var item in colliders)
        {
            detectedEnemies.Add(item.gameObject);
        }

        foreach (GameObject item in detectedEnemies)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemies.Add(item);
            }
        }
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

    public void Shoot()
    {
        (GameObject GO, FieldInfo SO) = Bullet_Pool_System.instance.GetBullet(0);
        if (GO != null && SO != null)
        {
            GO.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            SO.SetValue(GO.GetComponent(SO.DeclaringType), projectileData);
            GO.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Failed to get bullet from pool.");
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
            // Gizmos.DrawWireSphere(transform.position, detectRadius);
            Quaternion rot = Quaternion.identity;
            Gizmos.DrawWireMesh(mesh, 0, new(transform.position.x, transform.position.y, transform.position.z + detectRange /2),
                Quaternion.Euler(90, rot.y, 0), new(detectRadius, detectRange/2, detectRadius));
        }

        enemyDetectRingMaterial.SetFloat("_Radius", detectRadius * .05f);
        print(enemyDetectRingMaterial.GetFloat("_Radius") / .1f);
        // enemyDetectRingMaterial.SetFloat("_Thickness", detectRange);
    }
}
