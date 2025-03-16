using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Shoot_Flight : MonoBehaviour // By Samuel White
{
    [Header("Player Shoot Controls")]
    [SerializeField] private ScriptableObject projectileData;
    [SerializeField] private bool isShooting = false;
    [SerializeField] float fireRate = .2f;
    private float t = 0;

    [Header("Ring")]
    [SerializeField] Transform[] enemyDetectRings;
    [SerializeField] Material enemyDetectRingMaterial;

    [Header("Enemy Detection")]
    [Range(1, 9.9f)] [SerializeField] float detectRadius = 5;
    [Range(1, 16)] [SerializeField] float detectRange = 10;

    [Range(0, 4)] [SerializeField] int closestEnemiesRange = 2;

    [SerializeField] List<Transform> detectedEnemies;
    [SerializeField] LayerMask enemyLayer;

    [Header("Debug")]
    [SerializeField] bool enableDebug = true;
    [SerializeField] Mesh mesh;
    [SerializeField] bool debugAffectMaterial = false;

    void Start()
    {
        // enemyDetectRingMaterial = enemyDetectRing.GetComponent<Renderer>().material;
    }
    
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

        foreach (var item in colliders)
        {
            detectedEnemies.Add(item.transform);
        }

        detectedEnemies.Sort((t1, t2) => // https://discussions.unity.com/t/sorting-a-list-by-distance-to-an-object/178943/3
        {
            return Vector3.Distance(t1.transform.position, transform.position)
                .CompareTo(Vector3.Distance(t2.transform.position, transform.position));
        });
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
        for (int i = 0; i < Mathf.Clamp(detectedEnemies.Count, 0, closestEnemiesRange); i++)
        {
            (GameObject GO, FieldInfo SO) = Bullet_Pool_System.instance.GetBullet(0);
            if (GO != null && SO != null)
            {
                Quaternion look = detectedEnemies.Count == 0 ? Quaternion.identity : Quaternion.LookRotation(detectedEnemies[i].position - transform.position);
                GO.transform.SetPositionAndRotation(transform.position, look);
                SO.SetValue(GO.GetComponent(SO.DeclaringType), projectileData);
                GO.SetActive(true);
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
    public float FireRateSprinkle // Added by Khayne for the sprinkle powerup, makes it so it doesn't have to be set to a public variable.
    {
    get { return fireRate; }
    set { fireRate = Mathf.Max(0, value); }
    }
}
