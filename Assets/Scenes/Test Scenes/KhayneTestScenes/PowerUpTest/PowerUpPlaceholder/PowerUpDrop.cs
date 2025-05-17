using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    [SerializeField] Player_Power_Up_Controller.PowerUpType powerUpType; // Set this in the Inspector or via code

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player's projectile collided with the power-up
        if (other.CompareTag("PlayerProjectile"))
        {
            // Find the player in the scene (assumes single player)
            Player_Power_Up_Controller playerPowerUpController = FindObjectOfType<Player_Power_Up_Controller>();

            if (playerPowerUpController != null) 
            {
                playerPowerUpController.SelectPowerUp(powerUpType); 
                Debug.Log("Power-up collected: " + powerUpType); 
            }
            else
            {
                Debug.LogWarning("Player_Power_Up_Controller not found in the scene!");
            }

            // Destroy the power-up drop after collection
            Destroy(gameObject);
        }
    }
}