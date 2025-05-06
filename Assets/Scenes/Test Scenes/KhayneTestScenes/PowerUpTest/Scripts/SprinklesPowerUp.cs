using UnityEngine;

public class SprinklesPowerUp : MonoBehaviour
{
    // This script is for the sprinkles power-up.
    bool ready = true; // Indicates if the power-up is ready to be used.
    [SerializeField] GameObject SprinklesMinigun; // The sprinkles minigun prefab.
    [SerializeField] GameObject bullet; // The sprinkles minigun prefab.
    public void PowerUpTrigger() // Called when the power-up is triggered.
    {
        if (ready) // Check if the power-up is ready.
        {
            SprinklesMinigun.SetActive(true); // Activate the sprinkles minigun.
            
            for (int i = 0; i < 60; i++) // Loop to shoot the minigun 60 times.
            {

            }
        }
    }

    public void PowerUpDeactivate() // Called when the power-up is deactivated.
    {
        enabled = false; // Disable this script.
        ready = false; // Set the power-up to not ready.
        SprinklesMinigun.SetActive(false); // Deactivate the sprinkles minigun.
    }
}
