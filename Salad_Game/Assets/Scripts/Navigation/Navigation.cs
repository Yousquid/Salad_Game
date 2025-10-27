using UnityEngine;
using UnityEngine.InputSystem;

public class Navigation : MonoBehaviour
{
    [Header("Profile System")]
    public ProfileUI profileUI;
    public ProfileDataGenerator profileGenerator;
    public RandomFruit randomFruit;

    [Header("Mouse Swipe Settings")]
    [Range(0.05f, 0.5f)]
    public float swipeThreshold = 0.15f;
    public float rotationMultiplier = 10f;
    public float returnSpeed = 10f;
    public float maxTilt = 15f;

    private RectTransform panelRect;
    private float screenWidth;

    private float appliedAngleZ = 0f;
    private float targetAngleZ = 0f;

    private bool isDragging = false;
    private Vector2 pressPos;

    void Start()
    {
        panelRect = profileUI.GetComponent<RectTransform>();
        screenWidth = Mathf.Max(1, Screen.width);
        GenerateNewProfile();
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            isDragging = true;
            pressPos = mouse.position.ReadValue();
        }

        if (isDragging && mouse.leftButton.isPressed)
        {
            float deltaX = (mouse.position.ReadValue().x - pressPos.x) / screenWidth;
            targetAngleZ = Mathf.Clamp(-deltaX * rotationMultiplier, -maxTilt, maxTilt);
        }

        if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            float deltaX = (mouse.position.ReadValue().x - pressPos.x) / screenWidth;
            if (Mathf.Abs(deltaX) > swipeThreshold)
            {
                targetAngleZ = 0f;
                GenerateNewProfile();
            }
            isDragging = false;
        }

        if (!isDragging)
        {
            targetAngleZ = Mathf.MoveTowards(targetAngleZ, 0f, returnSpeed * Time.deltaTime);
        }

        float deltaAngle = targetAngleZ - appliedAngleZ;
        if (Mathf.Abs(deltaAngle) > 0.0001f)
        {
            RotateAroundBottomCenter(deltaAngle);
            appliedAngleZ += deltaAngle;
        }
    }

    private void RotateAroundBottomCenter(float deltaAngle)
    {
        Vector2 size = panelRect.rect.size;
        Vector2 pivot = panelRect.pivot;
        Vector3 localBottomCenter =
            new Vector3((0.5f - pivot.x) * size.x, (0f - pivot.y) * size.y, 0f);

        Vector3 worldBottomCenter = panelRect.TransformPoint(localBottomCenter);

        panelRect.RotateAround(worldBottomCenter, Vector3.forward, deltaAngle);
    }

    public void GenerateNewProfile()
    {
        if (profileGenerator == null || randomFruit == null || profileUI == null)
        {
            return;
        }

        if (Mathf.Abs(appliedAngleZ) > 0.0001f)
        {
            RotateAroundBottomCenter(-appliedAngleZ);
            appliedAngleZ = 0f;
            targetAngleZ = 0f;
        }

        var newProfile = profileGenerator.GenerateProfileData();
        profileUI.UpdateUI(newProfile);
        randomFruit.CombineRandom();
    }
}