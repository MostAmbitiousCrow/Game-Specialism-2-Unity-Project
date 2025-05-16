using UnityEngine;
using UnityEngine.InputSystem;

public class FlakePowerUp : MonoBehaviour // By Khayne Lutchmun.
{
    [Header("Flake Power-Up data")] // Header for the power-up data
    [SerializeField] bool ready = true; // Indicates if the power-up is ready to be used
    [SerializeField] bool readValue = false; // Input value for firing
    [SerializeField] Transform firePoint; // Fire point for the first bullet
    [SerializeField] Transform firePoint2; // Second fire point for the second bullet
    [SerializeField] GameObject bulletPrefab; // Prefab of the bullet to be instantiated
    [SerializeField] float fireRate = 1f; // Time between shots
    [SerializeField] int bulletsPerShot = 9; // Number of bullets per shot
    [SerializeField] float horizontalSpreadAngle = 5f; // Angle between bullets
    [SerializeField] float verticalSpreadAngle = 5f; // New variable for vertical spread
    private float nextFireTime; // Time when the next shot can be fired
    [SerializeField] float PowerUpTime = 20f; // Duration of the power-up
    [SerializeField] PlayerInput FlakePlayerInput; // Reference to the player input component
    [SerializeField] bool timerEnabled = false; // Indicates if the timer is enabled

    void Start() 
    {
        timerEnabled = false; // Disable the timer at the start
        FlakePlayerInput = GetComponent<PlayerInput>(); // Get the PlayerInput component
        FlakePlayerInput.enabled = false; // Disable the player input component
    }

    private void Update() 
    {
        FireCheck(); // Check if the fire button is pressed and if the power-up is ready
        PowerUpTimer(); // Update the power-up timer
    }

    public void PowerUpTrigger() 
    {
        ready = true; // Set the power-up to ready
        if (!timerEnabled)
        { 
            PowerUpTime = 10f; // Reset the power-up time.
            timerEnabled = true; // Enable the timer.  
        }  
        
        FlakePlayerInput.enabled = true; // Enable the player input component
        
        if (ready)
        {
            //Fire();
        }
    }

    public void PowerUpDeactivate()
    {
        enabled = false;
        ready = false;
        FlakePlayerInput.enabled = false; // Disable the player input component
    }

    public void PowerUpTimer() 
    {
        if (PowerUpTime > 0) 
        {
            PowerUpTime -= Time.deltaTime; // Decrease the power-up time
        }
        else
        {
            PowerUpDeactivate(); // Deactivate the power-up when time runs out
        }
    }

    public void FireCheck()
    {
        if (readValue && Time.time >= nextFireTime) // Check if the fire button is pressed and if it's time to fire
        {
            Fire();
        }
    }

    private void Fire()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float horizontalAngle = Random.Range(-horizontalSpreadAngle, horizontalSpreadAngle); // Random horizontal angle
            float verticalAngle = Random.Range(-verticalSpreadAngle, verticalSpreadAngle); // Random vertical angle
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(verticalAngle, horizontalAngle, 0); // Combine fire point rotation with calculated angles
            Instantiate(bulletPrefab, firePoint.position, rotation); // Instantiate the bullet at the fire point
            Instantiate(bulletPrefab, firePoint2.position, rotation); // Instantiate the bullet at the 2nd fire point
        }  

        nextFireTime = Time.time + fireRate;
    }

    public void FireInput(InputAction.CallbackContext context)
    {
        readValue = context.ReadValueAsButton();
    }
}
