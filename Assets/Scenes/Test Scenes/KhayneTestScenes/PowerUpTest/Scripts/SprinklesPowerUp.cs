using UnityEngine;

public class SprinklesPowerUp : MonoBehaviour
{
    // This script is for the sprinkles power-up.
    bool ready = true;
    public void PowerUpTrigger()
    {
        if (ready)
        {
            
        }
    }

    public void PowerUpDeactivate()
    {
        // Called as to end the power-up.
        enabled = false;
        ready = false;
    }
}
