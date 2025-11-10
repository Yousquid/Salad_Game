using System;
using System.Collections;
using System.Collections.Generic;
using Unity.UI.Shaders.Sample;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Navigation : MonoBehaviour
{
    private bool _started;
    public bool Started
    {
        get => _started;
        set => _started = value;
    }
    public UnlockFunction unlockFunction;
    public PercentageReport percentageReport;
    [Range(0, 1)] public float showPercentageReportChance = 0.5f;
    public float showPercentageReportSeconds = 2f;
    [Header("Profile System")] public ProfileUI profileUI;
    public ProfileDataGenerator profileGenerator;
    public RandomFruit randomFruit;

    [Header("Mouse Swipe Settings")] [Range(0.05f, 0.5f)]
    public float swipeThreshold = 0.15f;

    public float rotationMultiplier = 10f;
    public float returnSpeed = 10f;
    public float maxTilt = 15f;

    [Header("Camera Settings")] public Camera mainCamera;
    public float cameraDistance = 5f;
    public float horizontalAngleRange = 10f;
    public float verticalAngleRange = 6f;
    public float cameraMoveSpeed = 3f;

    private Vector3 targetCamPos;
    private Quaternion targetCamRot;

    private RectTransform panelRect;
    private Image panelImage;

    private float screenWidth;
    private float appliedAngleZ = 0f;
    private float targetAngleZ = 0f;
    private bool isDragging = false;
    private Vector2 pressPos;

    private Color baseColor = Color.white;
    private Color rightColor = new Color(1f, 0.6f, 0.6f);
    private Color leftColor = new Color(0.6f, 1f, 0.6f);

    public ProfileData currentProfile;
    public GameObject currentObject;
    public MatchGenerator matchGenerator;
    
    private Coroutine _showPercentageReportCoroutine;

    void Start()
    {
        matchGenerator = GetComponent<MatchGenerator>();
        panelRect = profileUI.GetComponent<RectTransform>();
        panelImage = panelRect.GetComponent<Image>();
        if (panelImage != null)
            baseColor = panelImage.color;

        screenWidth = Mathf.Max(1, Screen.width);

        if (mainCamera == null)
            mainCamera = Camera.main;

        GenerateNewProfile();
    }

    void Update()
    {
        if(!_started) return;
        
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame && !matchGenerator.isMatching)
        {
            isDragging = true;
            pressPos = mouse.position.ReadValue();
        }

        if (isDragging && mouse.leftButton.isPressed)
        {
            float deltaX = (mouse.position.ReadValue().x - pressPos.x) / screenWidth;
            targetAngleZ = Mathf.Clamp(-deltaX * rotationMultiplier, -maxTilt, maxTilt);

            if (panelImage != null)
            {
                float t = Mathf.Abs(targetAngleZ) / maxTilt;
                if (targetAngleZ > 0)
                    panelImage.color = Color.Lerp(baseColor, rightColor, t);
                else
                    panelImage.color = Color.Lerp(baseColor, leftColor, t);
            }
        }

        if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            float deltaX = (mouse.position.ReadValue().x - pressPos.x) / screenWidth;
            if (Mathf.Abs(deltaX) > swipeThreshold)
            {
                targetAngleZ = 0f;

                Vector3 mousePos = Input.mousePosition;

                float halfWidth = Screen.width / 2f;
                float r = Random.Range(0, 1);
                bool showPercentageReport = unlockFunction.PercentageReportUnlocked && r < showPercentageReportChance;
                if (mousePos.x < halfWidth)
                {
                    MatchGenerator.Instance.RecordUnlike();
                    EventBetter.Raise(new PlaySFXEvent(PlaySFXEvent.SFXType.SwipeLeft));
                    if (showPercentageReport)
                    {
                        if (_showPercentageReportCoroutine != null)
                        {
                            StopCoroutine(_showPercentageReportCoroutine);
                        }
                        StartCoroutine(ShowPercentageReportRoutine(GenerateNewProfile));
                    }
                    else
                    {
                        GenerateNewProfile();   
                    }
                }
                else
                {
                    MatchGenerator.Instance.RecordLike();
                    EventBetter.Raise(new PlaySFXEvent(PlaySFXEvent.SFXType.SwipeRight));
                    if (showPercentageReport)
                    {
                        if (_showPercentageReportCoroutine != null)
                        {
                            StopCoroutine(_showPercentageReportCoroutine);
                        }
                        StartCoroutine(ShowPercentageReportRoutine(PossibleInstantMatch));
                    }
                    else
                    {
                        PossibleInstantMatch();   
                    }
                }
            }

            isDragging = false;
        }

        if (!isDragging)
        {
            targetAngleZ = Mathf.MoveTowards(targetAngleZ, 0f, returnSpeed * Time.deltaTime);
            if (panelImage != null)
                panelImage.color = Color.Lerp(panelImage.color, baseColor, Time.deltaTime * returnSpeed);
        }

        float deltaAngle = targetAngleZ - appliedAngleZ;
        if (Mathf.Abs(deltaAngle) > 0.0001f)
        {
            RotateAroundBottomCenter(deltaAngle);
            appliedAngleZ += deltaAngle;
        }

        if (mainCamera != null)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCamPos,
                Time.deltaTime * cameraMoveSpeed);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetCamRot,
                Time.deltaTime * cameraMoveSpeed);
        }
    }

    private IEnumerator ShowPercentageReportRoutine(Action actionToPerform)
    {
        percentageReport.FadeInPercentageReport();
        yield return new WaitForSeconds(showPercentageReportSeconds);
        percentageReport.FadeOutPercentageReport();
        actionToPerform.Invoke();
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

    public void AddToSuperLike()
    {
        matchGenerator.superlikeData = currentProfile;
        matchGenerator.superlikeGameObject = currentObject;
    }

    private void AddCurrentNPCToLikes()
    {
        matchGenerator.likesData.Add(currentProfile);
        matchGenerator.likesGameObjects.Add(currentObject);
        MatchGenerator.Instance.likesNumber++;
    }

    private void PossibleInstantMatch()
    {
        float randomer = Random.Range(0, 100);
        if (randomer >= 12)
        {
            AddCurrentNPCToLikes();
            GenerateNewProfile();
        }
        else
        {
            AddCurrentNPCToLikes();
            GenerateNewProfile();
        }
    }

    public void GenerateNewProfile()
    {
        if (profileGenerator == null || randomFruit == null || profileUI == null)
            return;

        if (Mathf.Abs(appliedAngleZ) > 0.0001f)
        {
            RotateAroundBottomCenter(-appliedAngleZ);
            appliedAngleZ = 0f;
            targetAngleZ = 0f;
        }

        var newProfile = profileGenerator.GenerateProfileData();
        currentProfile = newProfile;
        currentObject = GameObject.Find("Fruit");
        profileUI.UpdateUI(newProfile);
        randomFruit.CombineRandom();

        if (panelImage != null)
            panelImage.color = baseColor;

        UpdateCameraTarget();
    }

    private void UpdateCameraTarget()
    {
        if (mainCamera == null || randomFruit == null) return;

        Vector3 fruitPos = randomFruit.transform.position;

        float yaw = Random.Range(-horizontalAngleRange, horizontalAngleRange);
        float pitch = Random.Range(-verticalAngleRange, verticalAngleRange);

        Quaternion randomRot = Quaternion.Euler(pitch, yaw, 0f);

        targetCamPos = fruitPos + randomRot * (Vector3.back * cameraDistance);
        targetCamRot = Quaternion.LookRotation(fruitPos - targetCamPos, Vector3.up);
    }
}