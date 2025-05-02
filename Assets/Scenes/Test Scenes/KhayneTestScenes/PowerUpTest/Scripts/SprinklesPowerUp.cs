using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SprinklesPowerUp : Player_Power_Up_Controller
{
    // This script is for the sprinkles power-up.
    public bool ready = true;
    
    public void TriggerPowerUp()
    {
        if (ready)
        {
            Player_Shoot_Flight playerShootFlight = FindObjectOfType<Player_Shoot_Flight>();
            playerShootFlight.FireRateSprinkle = 0.1f; // Set the fire rate
        }
    }
}
