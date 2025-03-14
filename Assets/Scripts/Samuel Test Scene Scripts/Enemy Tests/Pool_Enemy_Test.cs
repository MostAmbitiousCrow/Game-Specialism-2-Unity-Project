using System.Collections;
using System.Reflection;
using UnityEngine;

public class Pool_Enemy_Test : MonoBehaviour // By Samuel White
    // This is a enemy ai test script for testing basic enemy behaviour -
    // while the designers write the information for enemies in the GDD
{
    [SerializeField] Transform playerT; //TODO replace with game manager player variable
    public Test_Enemy_Beh_SO enemyData; // How the enemy will behave
    public ScriptableObject projectileData; // What projectile settings the enemy will shoot

    public Enemy_Pool_System.EnemyType.EnemyInfo data;
    public int ID;

    public Vector3 destination { private get; set; }
    private Vector3 startPos;

    private bool canShoot;
    private float t = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        startPos = transform.position;
    }

    public void ST(Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType spawnType) // Start Transition
    {
        
        StartCoroutine(Transition(spawnType));
    }

    IEnumerator Transition(Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType spawnType)
    {
        float c = 1;

        switch (spawnType)
        {
            case Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Portal:
                gameObject.SetActive(false);
                yield return new WaitForSeconds(enemyData.transitionTime);
                gameObject.SetActive(true);
                break;
            case Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Behind:
                while (c > 0)
                {
                    c -= Time.deltaTime / enemyData.transitionTime;
                    transform.position += Vector3.Lerp(destination, startPos, c);
                    yield return null;
                }
                break;
            case Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Front:
                while (c > 0)
                {
                    c -= Time.deltaTime / enemyData.transitionTime;
                    transform.position += Vector3.Lerp(destination, startPos, c);
                    yield return null;
                }
                break;
        }
        InvokeRepeating(nameof(Shoot), 0, enemyData.attackTime);

        yield break;
    }

    public void Shoot()
    {
        if (canShoot)
        {
            (GameObject GO, FieldInfo SO) = Bullet_Pool_System.instance.GetBullet(0);
            if (GO != null && SO != null)
            {
                Quaternion look = Quaternion.LookRotation(playerT.position - transform.position);
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

    public void Defeated()
    {
        Enemy_Pool_System.instance.ReturnEnemy(data, ID);
    }
}
