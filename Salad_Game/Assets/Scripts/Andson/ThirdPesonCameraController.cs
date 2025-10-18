using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPesonCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform target;             // 跟随目标（玩家）
    public Transform camPivot;           // 相机旋转中心，可为空

    [Header("Camera Settings")]
    public float distance = 4f;          // 距离
    public float height = 1.7f;          // 高度
    public Vector2 shoulderOffset = new Vector2(0.8f, 0); // 左右偏移
    public float rotationSpeed = 120f;

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
    private Camera cam;

    // Input System variables
    private Vector2 lookInput;
    private bool zoomPressed;

    private float yaw;
    private float pitch;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    void Start()
    {
        cam = GetComponent<Camera>();
        defaultFOV = cam.fieldOfView;

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
        yaw += lookInput.x * rotationSpeed * Time.deltaTime;
        pitch -= lookInput.y * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        camPivot.rotation = Quaternion.Euler(currentRotation);
    }

    void HandlePosition()
    {
        // 计算偏移（肩膀视角）
        Vector3 offset = camPivot.TransformDirection(new Vector3(
            rightShoulder ? shoulderOffset.x : -shoulderOffset.x,
            shoulderOffset.y,
            0));

        Vector3 desiredPosition = camPivot.position + offset - camPivot.forward * distance;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, positionSmoothTime);
        transform.position = smoothedPosition;

        transform.LookAt(camPivot.position + Vector3.up * height * 0.3f);
    }

    void HandleZoom()
    {
        float targetFOV = zoomPressed ? zoomedFOV : defaultFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    // ======== Input System Callbacks ========

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
