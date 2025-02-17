using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Some insiration and code taken from https://youtu.be/8TqY6p-PRcs?t=253

public class ProjectilePlayerShoot : MonoBehaviour // Made by Khayne
{
    [Header("Projectile Values")]
    [SerializeField] GameObject ProjectileNormal; // Reference to the projectile prefab
    [SerializeField] GameObject ProjectileFast; // Reference to the projectile prefab
    [SerializeField] GameObject ProjectileFastest; // Reference to the projectile prefab
    [SerializeField] Transform FirePoint; // Reference to the point where the projectile will be created
    
    [Header("Upgrade Values")]
    [SerializeField] bool NormalUpgrade = true; // If the player has the normal upgrade
    [SerializeField] bool FastUpgrade = false; // If the player has the fast upgrade
    [SerializeField] bool FastestUpgrade = false; // If the player has the fastest upgrade
    
    public void FireInput(InputAction.CallbackContext context)// When the fire input is pressed
    {
        if (context.performed)
        {
            if (FastestUpgrade) // If the player has the fastest upgrade
            {
                FireFastest(); // Fire the fastest projectile
            }

            if (FastUpgrade) // If the player has the fast upgrade
            {
                FireFast(); // Fire the fast projectile
            }

            if (NormalUpgrade) // If the player has the normal upgrade
            {
                FireNormal(); // Fire the normal projectile
            }
        }
    }

    void FireNormal()
    {
       Instantiate(ProjectileNormal, FirePoint.position, transform.rotation); // Create a new projectile at the the fire point

    }

    void FireFast()
    {
        Instantiate(ProjectileFast, FirePoint.position, transform.rotation); // Create a new fast projectile at the the fire point
    }

    void FireFastest()
    {
        Instantiate(ProjectileFastest, FirePoint.position, transform.rotation); // Create a new fastest projectile at the the fire point
    }
}
