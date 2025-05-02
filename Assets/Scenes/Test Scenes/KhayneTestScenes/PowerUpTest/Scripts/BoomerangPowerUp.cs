using UnityEngine;

public class BoomerangPowerUp : MonoBehaviour
{
    // This script is for the boomerang power-up.
    bool ready;
    
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
