using UnityEngine;

public class Player_Data : MonoBehaviour
{
    //========================================
    // Stores each player data.
    //========================================

    [Header("Player Data")]
    public int playerNumber = 0; // 0 = Player 1, 1 = Player 2
    public Transform view; // The player camera view
    public Player_Health playerHealth;
    public Player_Shoot_Flight playerShoot;
}
