using UnityEngine;
using UnityEngine.InputSystem;

public class SprinklesPowerUp : MonoBehaviour
{
    // This script is for the sprinkles power-up.
    bool ready = true; // Indicates if the power-up is ready to be used.
    [SerializeField] GameObject sprinklesGun; // The prefab for the sprinkles gun.
    [SerializeField] Transform shootPointMinigun; // The point from where the sprinkles will shoot.
    [SerializeField] GameObject bulletPrefab; // The prefab for the bullet.
    [SerializeField] float fireRateSprinkles = 0.025f; // The rate at which the sprinkles will shoot.
    [SerializeField] int maxbullets = 60; // The maximum number of bullets that
    [SerializeField] int currentBullets; // The current number of bullets.
    private float nextFireTime; // The next time the sprinkles will shoot.

    private void Start()
    {
        currentBullets = maxbullets; // Initialize the current bullets to the maximum.
        sprinklesGun.SetActive(false); // Deactivate the sprinkles gun at the start.
    }
    private void Update()
    {

    }

    public void PowerUpTrigger() // Called when the power-up is triggered.
    {
        if (ready) // Check if the power-up is ready.
        {
            sprinklesGun.SetActive(true); // Activate the sprinkles gun.
            Fire(); // Start firing the sprinkles.
        }
    }


    public void PowerUpDeactivate() // Called when the power-up is deactivated.
    {
        enabled = false; // Disable this script.
        ready = false; // Set the power-up to not ready.
        sprinklesGun.SetActive(false); // Deactivate the sprinkles gun.
    }

    private void Fire()
    {
        if (currentBullets > 0) // Check if there are bullets left.
        {
            if (Time.time >= nextFireTime) // Check if it's time to fire again.
            {
                nextFireTime = Time.time + fireRateSprinkles; // Set the next fire time.
                SpawnBullet(); // Fire the sprinkles.
            }
        }
        
        else
        {
            PowerUpDeactivate(); // Deactivate the power-up if no bullets are left.
        }
    }

    public void SpawnBullet()
    {
        Instantiate(bulletPrefab, shootPointMinigun.position, shootPointMinigun.rotation); // Instantiate the bullet prefab at the shoot point.
        currentBullets--; // Decrease the current bullets count.
    }

    public void FireInput(InputAction.CallbackContext context) // Called when the fire input is triggered.
    {
        Fire(); // Trigger the power-up.
    }
}
