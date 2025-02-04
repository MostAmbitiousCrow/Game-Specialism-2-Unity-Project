using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controls : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float movementSpeed = 5;
    private Vector2 inputDirection;

    [Header("")]
    [SerializeField] Rigidbody2D rb;

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(inputDirection.x * movementSpeed, inputDirection.y * movementSpeed);
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void FireInput(InputAction.CallbackContext context)
    {
        print("Fired");
    }
}
