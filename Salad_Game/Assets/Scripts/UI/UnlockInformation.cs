using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockInformation : MonoBehaviour
{
    public Sprite superLikeIcon;
    public Sprite reportIcon;
    public Sprite percentageIcon;
    public Sprite likePassTagsIcon;
    public Image iconImage;
    public TMP_Text informationText;
    public float startScale;
    public float endScale;
    public Vector2 informationPanelOpenMinMaxY;
    public float informationPanelDuration = 0.5f;
    public float informationPanelCloseY;
    private RectTransform _rectTransform;
    private UnlockUI _unlockUI;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _unlockUI = GameObject.FindAnyObjectByType<UnlockUI>();
    }

    private void Start()
    {
        ResetUnlockInformation();
    }
    [Button]
    public void ShowUnlockInformation(UnlockUI.IconTypes iconType, string information)
    {
        EventBetter.Raise(new PlaySFXEvent(PlaySFXEvent.SFXType.Unlock));
        ResetUnlockInformation();
        Sequence animation = DOTween.Sequence();
        animation.Append(_rectTransform.DOLocalMoveY(informationPanelOpenMinMaxY.y, informationPanelDuration).SetEase(Ease.InOutElastic));
        animation.Join(_rectTransform.DOScale(new Vector3(endScale, endScale, 1), informationPanelDuration).SetEase(Ease.InOutElastic));
        switch (iconType)
        {
            case UnlockUI.IconTypes.SuperLike:
                iconImage.sprite = superLikeIcon;
                break;
            case UnlockUI.IconTypes.Report:
                iconImage.sprite = reportIcon;
                break;
            case UnlockUI.IconTypes.Percentage:
                iconImage.sprite = percentageIcon;
                break;
        }
        informationText.text = information;
        if(_unlockUI != null) _unlockUI.UpdateUnlockUI(iconType);
    }

    public void HideUnlockInformation()
    {
        _rectTransform.DOLocalMoveY(informationPanelCloseY, informationPanelDuration).SetEase(Ease.InOutElastic);
        _rectTransform.DOScale(new Vector3(startScale, startScale, 1), informationPanelDuration).SetEase(Ease.InOutElastic);
    }

    public void ResetUnlockInformation()
    {
        _rectTransform.SetLocalY(informationPanelOpenMinMaxY.x);
        _rectTransform.localScale = new Vector3(startScale, startScale, 1);
    }
}
