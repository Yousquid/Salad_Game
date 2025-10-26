using UnityEngine;
using UnityEngine.EventSystems;

public class Navigation : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [Header("Profile System")]
    public ProfileUI profileUI;
    public ProfileDataGenerator profileGenerator;
    public string[] temperamentFilters;

    [Header("Swipe Settings")]
    public float swipeThreshold = 0.15f;

    private Vector2 startPos;
    private float screenWidth;

    void Start()
    {
        screenWidth = Screen.width;

        GenerateNewProfile();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float deltaX = (eventData.position.x - startPos.x) / screenWidth;

        if (Mathf.Abs(deltaX) > swipeThreshold)
        {
            GenerateNewProfile();
        }
    }

    public void GenerateNewProfile()
    {
        if (profileGenerator == null || profileUI == null)
        {
            return;
        }

        var newProfile = profileGenerator.GenerateProfileData(temperamentFilters);

        profileUI.UpdateUI(newProfile);
    }
}
