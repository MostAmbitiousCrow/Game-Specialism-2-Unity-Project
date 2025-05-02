using UnityEngine;

public class BubbleGumPowerUp : MonoBehaviour
{
    // This script is for the bubble gum power-up.
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
