using System.Reflection;
using UnityEngine;

public class Test_Shoot_Projectile : MonoBehaviour // By Samuel White
{
    [SerializeField] private float direction = 0;
    [SerializeField] private bool showDebug = true;
    // [SerializeField] private float destroyTime = 10;

    [SerializeField] private ScriptableObject scriptableObject; // The scriptable object to be assigned to the projectile

    // [SerializeField] private FieldInfo[] bulletFields;
    // [SerializeField] private FieldInfo bulletField;

    public void Shoot()
    {
        (GameObject GO, FieldInfo SO) = Bullet_Pool_System.instance.GetBullet(0);
        if (GO != null && SO != null)
        {
            GO.transform.SetPositionAndRotation(transform.position, Quaternion.identity * Quaternion.Euler(0, 0, direction));
            SO.SetValue(GO.GetComponent(SO.DeclaringType), scriptableObject);
            GO.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Failed to get bullet from pool.");
        }
    }

    void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, direction) * Vector3.right);
        }
        if (direction < 0) direction = 359;
        else if (direction > 360) direction = 0;
    }
}
