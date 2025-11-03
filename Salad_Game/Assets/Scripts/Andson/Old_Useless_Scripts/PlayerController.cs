using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CharacterController characterController;
    public Transform cameraTransform; 

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 10f;
    public float gravity = -30f;

    [Header("Camera Rotation Settings")]
    public float minPitch = -60f; // 最小俯角
    public float maxPitch = 60f;  // 最大仰角
    private float rotationX = 0f;

    private float verticalVelocity;
    private float rotationY;

    void Start()
    {
        if (!characterController)
            characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector2 movementVector)
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * movementVector.y + camRight * movementVector.x;

        characterController.Move(moveDir * moveSpeed * Time.deltaTime);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(camForward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    public void Rotate(Vector2 rotationVector)
    {
        rotationY += rotationVector.x * rotationSpeed * Time.deltaTime;
        rotationX -= rotationVector.y * rotationSpeed * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    
    public void Jump()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }
}
    