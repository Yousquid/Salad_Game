using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class ThirdPesonCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform target;           
    public Transform camPivot;         

    [Header("Camera Settings")]
    public float distance = 4f;         
    public float height = 1.7f;        
    public float rotationSpeed = 120f;
    public float mouseSensitivity = 1f;

    [Header("Rotation Limits")]
    public float minPitch = -40f;
    public float maxPitch = 70f;

    [Header("Smoothing")]
    public float rotationSmoothTime = 0.1f;
    public float positionSmoothTime = 0.1f;

    [Header("Zoom Settings")]
    public float zoomedFOV = 35f;
    public float zoomSpeed = 5f;
    private float defaultFOV;

    private bool rightShoulder = true;
    private  CinemachineCamera cam;

    private Vector2 lookInput;
    private bool zoomPressed;

    private float yaw;
    private float pitch;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    void Start()
    {
        cam = GetComponent<CinemachineCamera>();

        if (!target)
        {
            Debug.LogError("Camera target not assigned!");
            enabled = false;
            return;
        }

        if (!camPivot)
        {
            GameObject pivotObj = new GameObject("CameraPivot");
            pivotObj.transform.position = target.position + Vector3.up * height;
            pivotObj.transform.parent = target;
            camPivot = pivotObj.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        HandleRotation();
        HandlePosition();
        HandleZoom();
    }


     void HandleRotation()
    {
        yaw += lookInput.x * rotationSpeed * mouseSensitivity * Time.deltaTime;
        pitch -= lookInput.y * rotationSpeed * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        camPivot.rotation = Quaternion.Euler(currentRotation);
    }

    void HandlePosition()
    {
      

        Vector3 desiredPosition = camPivot.position  - camPivot.forward * distance;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, positionSmoothTime);
        transform.position = smoothedPosition;

        transform.LookAt(camPivot.position + Vector3.up * height * 0.3f);
    }

    void HandleZoom()
    {
       
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnSwitchShoulder(InputAction.CallbackContext context)
    {
        if (context.performed)
            rightShoulder = !rightShoulder;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoomPressed = context.ReadValueAsButton();
    }
}
