using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Made by Khayne

public class MovementPlayer : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] float MoveSpeed = 5; // Speed at which the player moves
    private Vector2 InputDirection; // Stores the direction of player input

    [Header("Rigidbody")]
    [SerializeField] Rigidbody2D rbKhayne; // Reference to the player's Rigidbody2D component

    void FixedUpdate()
    {
        // Update the player's velocity based on input direction and move speed
        rbKhayne.velocity = new Vector2(InputDirection.x * MoveSpeed, InputDirection.y * MoveSpeed);
    }

    public void MoveInputKhayne(InputAction.CallbackContext context)
    {
        // Read the input direction from the input context
        InputDirection = context.ReadValue<Vector2>();
    }
}
