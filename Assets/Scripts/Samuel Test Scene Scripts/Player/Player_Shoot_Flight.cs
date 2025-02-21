using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Shoot_Flight : MonoBehaviour // By Samuel White
{
    [SerializeField] private ScriptableObject scriptableObject;
    [SerializeField] private bool isShooting = false;
    [SerializeField] float fireRate = .2f;
    private float t = 0;
    
    void Update()
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
        else t = 0;
    }

    public void Shoot()
    {
        (GameObject GO, FieldInfo SO) = Bullet_Pool_System.instance.GetBullet(0);
        if (GO != null && SO != null)
        {
            GO.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            SO.SetValue(GO.GetComponent(SO.DeclaringType), scriptableObject);
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
}
