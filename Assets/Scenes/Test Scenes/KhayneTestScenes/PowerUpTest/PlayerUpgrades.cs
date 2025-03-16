using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("Player Upgrades")]
    public bool Sprinkle = false;
    public bool Marsh = false;
    public bool Bubble = false;
    public bool Waffle = false;
    public bool Flake = false;
    // Start is called before the first frame update
    void Start()
    {
        Sprinkle = false;
        Marsh = false;
        Bubble = false;
        Waffle = false;
        Flake = false;
    }

    void Update()
    {
        if (Sprinkle == true)
        {
            Debug.Log("Sprinkle Upgrade");
                        // Example from another script
            Player_Shoot_Flight playerShootFlight = FindObjectOfType<Player_Shoot_Flight>();
            playerShootFlight.FireRateSprinkle = 0.1f; // Set the fire rate
        }
        if (Sprinkle == false)
        {
            Player_Shoot_Flight playerShootFlight = FindObjectOfType<Player_Shoot_Flight>();
            playerShootFlight.FireRateSprinkle = 0.2f; // Set the fire rate
        }
        if (Marsh == true)
        {
            Debug.Log("Marsh Upgrade");
        }
        if (Bubble == true)
        {
            Debug.Log("Bubble Upgrade");
        }
        if (Waffle == true)
        {
            Debug.Log("Waffle Upgrade");
        }
        if (Flake == true)
        {
            Debug.Log("Flake Upgrade");
        }
    }


}
