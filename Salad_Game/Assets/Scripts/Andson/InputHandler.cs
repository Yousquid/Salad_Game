using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{

    public PlayerController playerController;

    private InputAction moveAction, lookAction, jumpAction;
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");

        jumpAction.performed += OnJumpPerformed;

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerInput();
    }

    void HandlePlayerInput()
    {
        Vector2 movementVector = moveAction.ReadValue<Vector2>();
        playerController.Move(movementVector);
        Vector2 rotationVector = lookAction.ReadValue<Vector2>();
        playerController.Rotate(rotationVector);
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        playerController.Jump();
    }
}
