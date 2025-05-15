using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Character_Data : MonoBehaviour
{
    //========================================
    // Stores each player data.
    //========================================

    [Header("Player Data")]
    public int playerNumber = 0; // 0 = Player 1, 1 = Player 2
    public Transform view; // The player camera view
    public Player_Health playerHealth;
    public Player_Shoot_Flight playerShoot;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!GameData.isPaused)
            {
                Player_Game_UI_Manager.instance.ShowPauseMenu(true);
                Debug.Log($"{name} Paused Game");
            }
            else
            {
                Player_Game_UI_Manager.instance.ShowPauseMenu(false);
                Debug.Log($"{name} Unpaused Game");  
            }
        }
    }
}
