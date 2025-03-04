using System.Reflection;
using UnityEngine;

public class Pool_Enemy_Test : MonoBehaviour
{
    [SerializeField] Transform playerT;
    public ScriptableObject enemyData;
    public ScriptableObject projectileData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
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
