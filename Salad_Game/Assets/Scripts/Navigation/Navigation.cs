using UnityEngine;
using UnityEngine.InputSystem;

public class Navigation : MonoBehaviour
{
    [Header("Profile System")]
    public ProfileUI profileUI;
    public ProfileDataGenerator profileGenerator;

    [Header("Mouse Swipe Settings")]
    [Range(0.05f, 0.5f)]
    public float swipeThreshold = 0.15f;

    private Vector2 startPos;
    private bool isDragging;
    private float screenWidth;

    void Start()
    {
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
            startPos = mouse.position.ReadValue();
        }

        if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            Vector2 endPos = mouse.position.ReadValue();
            float deltaX = (endPos.x - startPos.x) / screenWidth;

            if (Mathf.Abs(deltaX) > swipeThreshold)
                GenerateNewProfile();

            isDragging = false;
        }
    }

    public void GenerateNewProfile()
    {
        if (profileGenerator == null || profileUI == null)
        {
            return;
        }

        var newProfile = profileGenerator.GenerateProfileData();
        profileUI.UpdateUI(newProfile);
    }
}