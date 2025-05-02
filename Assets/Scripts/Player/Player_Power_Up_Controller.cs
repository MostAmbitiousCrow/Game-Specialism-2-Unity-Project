using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Power_Up_Controller : MonoBehaviour // By Samuel White && Khayne Lutchmun
{
    // Add the PowerUp scripts here.

    [Header("Power Up Scripts")]
    [SerializeField] private BubbleGumPowerUp bubbleGumPowerUp;
    [SerializeField] private FlakePowerUp flakePowerUp;
    [SerializeField] private BoomerangPowerUp boomerangPowerUp;
    [SerializeField] private SprinklesPowerUp sprinklesPowerUp;

    private Player_Power_Up_Controller selectedPowerUp;

    public enum PowerUpType
    {
        None, BubbleGum, Flake, Boomerang, Sprinkles
    }
    public PowerUpType powerUpType;

    public void SelectPowerUp(PowerUpType type)
    {
        if (type == PowerUpType.None) return;

        switch (type)
        {
            case PowerUpType.BubbleGum:
                selectedPowerUp = bubbleGumPowerUp;
                break;

            case PowerUpType.Flake:
                selectedPowerUp = flakePowerUp;
                break;

            case PowerUpType.Boomerang:
                selectedPowerUp = boomerangPowerUp;
                break;

            case PowerUpType.Sprinkles:
                selectedPowerUp = sprinklesPowerUp;
                break;
        }
    }

    public void TriggerPowerUp(InputAction.CallbackContext context)
    {
        if (selectedPowerUp != null && context.ReadValueAsButton())
        {
            // Check if the power-up is ready and can be triggered here:
            // if (!selectedPowerUp.ready) return;

            // Trigger the power-up's trigger method here:
            // selectedPowerUp.TriggerPowerUp();

            // Trigger the power-up's trigger method here:
            // selectedPowerUp.TriggerPowerUp();
            Debug.Log("Power-up triggered");
        }
    }

    public void DeactivatePowerUp()
    {
        // Deactivate the power-up here:

        powerUpType = PowerUpType.None;
        Debug.Log("Power-up deactivated");
    }
}