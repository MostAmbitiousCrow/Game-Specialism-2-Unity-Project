using System.Collections;
using System.Reflection;
using UnityEngine;

public class Pool_Enemy_Test : MonoBehaviour // By Samuel White
    // This is a enemy ai test script for testing basic enemy behaviour-
    // while the designers write the information for enemies in the GDD
{
    [SerializeField] Transform playerT;
    public Test_Enemy_Beh_SO enemyData;
    public ScriptableObject projectileData;

    public Vector3 destination { private get; set; }
    private Vector3 startPos;

    private bool canShoot;
    private float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        float y = 1;
        while (y > 0)
        {
            y -= Time.deltaTime / enemyData.transitionTime;
            transform.position += Vector3.Lerp(destination, startPos, y);
            yield return null;
        }
        InvokeRepeating(nameof(Shoot), 0, enemyData.attackTime);

        yield break;
    }

    //void Update()
    //{
    //    if (canShoot) ShootTimer();
    //}

    //void ShootTimer()
    //{
    //    t += Time.deltaTime;
    //    if (t >= enemyData.attackTime)
    //    {
    //        Shoot();
    //        t = 0;
    //    }
    //}

    public void Shoot()
    {
        if (canShoot)
        {
            (GameObject GO, FieldInfo SO) = Enemy_Pool_System.instance.GetBullet(0);
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
}
