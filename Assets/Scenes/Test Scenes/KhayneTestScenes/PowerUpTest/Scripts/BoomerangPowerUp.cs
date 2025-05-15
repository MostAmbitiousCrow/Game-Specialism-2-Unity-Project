using UnityEngine;
using UnityEngine.InputSystem;

public class BoomerangPowerUp : MonoBehaviour // By Khayne Lutchmun.
{
    [Header("Boomerang Power-Up data")]
    [SerializeField] bool ready = true;
    [SerializeField] bool readValue = false;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform player;
    [SerializeField] GameObject boomerangPrefab;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float nextFireTime;
    [SerializeField] float PowerUpTime = 20f;
    [SerializeField] int maxFires = 2; // Maximum number of times the boomerang can be fired
    private int fireCount = 0; // Counter for the number of times the boomerang has been fired

    void Start()
    {
        readValue = false; // Ensure readValue is false at the start
    }

    private void Update()
    {
        if (ready)
        {
            FireCheck();
            PowerUpTimer();
        }
    }

    public void PowerUpTrigger()
    {
        if (ready && fireCount < maxFires)
        {
            Fire();
        }
    }

    public void PowerUpDeactivate()
    {
        enabled = false;
        ready = false;
    }

    public void PowerUpTimer()
    {
        if (PowerUpTime > 0)
        {
            PowerUpTime -= Time.deltaTime;
        }
        else
        {
            PowerUpDeactivate();
        }
    }

    public void FireCheck()
    {
        if (readValue && Time.time >= nextFireTime && fireCount < maxFires)
        {
            Fire();
        }
    }

    private void Fire()
    {
        nextFireTime = Time.time + fireRate;
        GameObject boomerang = Instantiate(boomerangPrefab, firePoint.position, firePoint.rotation);
        WaffleBullet boomerangScript = boomerang.GetComponent<WaffleBullet>();
        boomerangScript.Initialize(player, this); // Pass the BoomerangPowerUp script to the boomerang
        fireCount++;
    }

    public void FireInput(InputAction.CallbackContext context)
    {
        readValue = context.ReadValueAsButton();
    }

    public void ResetFireCount()
    {
        fireCount = 0; // Reset the fire count when the boomerang is caught
    }
}
