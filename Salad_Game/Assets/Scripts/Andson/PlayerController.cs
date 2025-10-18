using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;

    public float moveSpeed = 10f;
    public float rotationSpeed = 5f;
    public float jumpForce = 10f;
    public float gravity = -30f;

    private float rotationY;
    private float verticalVelocity;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    public void Move(Vector2 movementVector)
    {
        Vector3 thisMove = transform.forward * movementVector.y + transform.right * movementVector.x;
        thisMove = thisMove * moveSpeed * Time.deltaTime;
        characterController.Move(thisMove);

        verticalVelocity = verticalVelocity + gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    public void Rotate(Vector2 rotationVector)
    {
        rotationY += rotationVector.x * rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }
}
    