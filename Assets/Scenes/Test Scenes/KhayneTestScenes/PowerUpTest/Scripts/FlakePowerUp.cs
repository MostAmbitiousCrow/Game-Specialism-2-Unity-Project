using UnityEngine;

public class FlakePowerUp : MonoBehaviour
{
    // This script is for the flake power-up.
    bool ready = true;    
    public void PowerUpTrigger()
    {
        // Called when the power-up is triggered.
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
