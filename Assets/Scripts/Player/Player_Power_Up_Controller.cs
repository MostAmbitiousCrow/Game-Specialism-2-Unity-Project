using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Power_Up_Controller : MonoBehaviour // By Samuel White && Khayne Lutchmun
{
    [Header("Power Up Scripts")]
    [SerializeField] BubbleGumPowerUp bubbleGumPowerUp;
    [SerializeField] FlakePowerUp flakePowerUp;
    [SerializeField] BoomerangPowerUp boomerangPowerUp;
    [SerializeField] SprinklesPowerUp sprinklesPowerUp;

    public enum PowerUpType
    {
        None, BubbleGum, Flake, Boomerang, Sprinkles
    }
    public PowerUpType powerUpType;

    public void SelectPowerUp(PowerUpType type)
    {
        powerUpType = type;
        if (type == PowerUpType.None) return;

        switch (type)
        {
            case PowerUpType.BubbleGum:
                bubbleGumPowerUp.enabled = true;
                break;

            case PowerUpType.Flake:
                flakePowerUp.enabled = true;
                break;

            case PowerUpType.Boomerang:
                boomerangPowerUp.enabled = true;
                break;

            case PowerUpType.Sprinkles:
                sprinklesPowerUp.enabled = true;
                break;
        }
    }

    public void OnPowerUp(InputValue context)
    {
        if (powerUpType != PowerUpType.None && context.isPressed)
        {
            // Trigger the power-up's trigger method here:
            switch (powerUpType)
            {
                case PowerUpType.BubbleGum:
                    bubbleGumPowerUp.PowerUpTrigger();
                    break;

                case PowerUpType.Flake:
                    flakePowerUp.PowerUpTrigger();
                    break;

                case PowerUpType.Boomerang:
                    boomerangPowerUp.PowerUpTrigger();
                    break;

                case PowerUpType.Sprinkles:
                    sprinklesPowerUp.PowerUpTrigger();
                    break;
            }
            Debug.Log("Power-up triggered");
        }
    }

    public void DeactivatePowerUp()
    {
        // Deactivate the power-up here:
        switch (powerUpType)
        {
            case PowerUpType.BubbleGum:
                bubbleGumPowerUp.PowerUpDeactivate();
                break;

            case PowerUpType.Flake:
                flakePowerUp.PowerUpDeactivate();
                break;

            case PowerUpType.Boomerang:
                boomerangPowerUp.PowerUpDeactivate();
                break;

            case PowerUpType.Sprinkles:
                sprinklesPowerUp.PowerUpDeactivate();
                break;
        }

        powerUpType = PowerUpType.None;
        Debug.Log("Power-up deactivated");
    }
}