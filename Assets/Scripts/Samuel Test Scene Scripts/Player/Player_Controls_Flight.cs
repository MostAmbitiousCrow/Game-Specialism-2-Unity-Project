using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controls_Flight : MonoBehaviour // By Samuel White
{
    [Header("Player Controls")]
    [SerializeField] private float moveSpeed = 10f;
    public float screenWidth, screenHeight;
    public bool useCursorMovement = false;

    [SerializeField] float worldXLimit, worldYLimit;
    [SerializeField] Vector2 cursorPosition;

    [SerializeField] Vector2 inputDirection;

    void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // worldXLimit = screenWidth / 100; // Temporarily ingoring because this won't work properly with all devices.
        // worldYLimit = screenHeight / 100;
    }

    void FixedUpdate()
    {
        if (useCursorMovement)
        {
            Vector2 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
            inputDirection = cursorPosition - playerPosition;
            transform.position = inputDirection;
        }
        else
        {
            Vector3 direction = moveSpeed * Time.deltaTime * (Vector3)inputDirection;
            transform.position += direction;
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -worldXLimit, worldXLimit),
                Mathf.Clamp(transform.position.y, -worldYLimit, worldYLimit));
        }
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
