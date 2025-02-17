using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controls_Flight : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField] private float moveSpeed = 10f;
    public float screenWidth, screenHeight;
    public bool useCursorMovement = false;
    [SerializeField] Vector2 cursorPosition;

    [SerializeField] Vector2 inputDirection;

    [Header("Components")]
    [SerializeField] Rigidbody rb;

    void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (useCursorMovement)
        {
            Vector2 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
            inputDirection = (cursorPosition - playerPosition).normalized;
        }
        rb.velocity = new Vector3(inputDirection.x, inputDirection.y, 0) * moveSpeed;
        rb.position = new Vector2(Mathf.Clamp(rb.position.x, -screenWidth, screenWidth), Mathf.Clamp(rb.position.y, -screenHeight, screenHeight));
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void CursorPosition(InputAction.CallbackContext context)
    {
        cursorPosition = context.ReadValue<Vector2>();
    }
}
