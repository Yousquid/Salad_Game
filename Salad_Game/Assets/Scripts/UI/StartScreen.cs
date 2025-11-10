using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartScreen : Singleton<StartScreen>
{
    public TMP_Text vText;
    public RectTransform rightPanel;
    public float rightPanelMoveTarget = 200f;
    public RectTransform leftPanel;
    public float leftPanelMoveTarget = -200f;
    public Navigation navigation;
    public RectTransform textRect;
    public float textRectMoveTarget = 200f;
    public float textRectMoveDurationInSecond = 1f;
    public float swipeThreshold = 0.2f;
    private float screenWidth;
    private bool isDragging = false;
    private Vector2 pressPos;
    protected override void Awake()
    {
        base.Awake();

        if (navigation == null)
        {
            navigation = FindObjectOfType<Navigation>();
        }
    }

    private void Start()
    {
        screenWidth = Screen.width;
        // textRect.DOAnchorPosX(textRectMoveTarget, textRectMoveDurationInSecond)
        //     .SetEase(Ease.InOutBack)
        //     .SetLoops(-1, LoopType.Yoyo);
        Sequence s = DOTween.Sequence();
        s.Append(textRect.DOAnchorPosX(textRectMoveTarget, textRectMoveDurationInSecond).SetEase(Ease.InOutBack));
        s.Append(textRect.DOAnchorPosX(-300, textRectMoveDurationInSecond).SetEase(Ease.Linear));
        s.SetLoops(-1, LoopType.Restart);
    }

    private void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            isDragging = true;
            pressPos = mouse.position.ReadValue();
        }

        if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            float deltaX = (mouse.position.ReadValue().x - pressPos.x) / screenWidth;
            if (Mathf.Abs(deltaX) > swipeThreshold)
            {
                if (deltaX > 0 && !navigation.Started)
                {
                    rightPanel.DOAnchorPosX(rightPanelMoveTarget, textRectMoveDurationInSecond).SetEase(Ease.InOutBack);
                    leftPanel.DOAnchorPosX(leftPanelMoveTarget, textRectMoveDurationInSecond).SetEase(Ease.InOutBack)
                        .OnComplete((() =>
                        {
                            navigation.Started = true;
                            gameObject.SetActive(false);
                        }));
                    Color c = vText.color;
                    DOTween.To(() => c.a, x => {
                        c.a = x;
                        vText.color = c;
                    }, 0f, textRectMoveDurationInSecond).SetEase(Ease.InOutQuad);
                }
                else
                {
                }
            }
        }
    }
}