using System;
using UnityEngine;
using UnityEngine.UI;

public class UnlockUI : MonoBehaviour
{
    public Image superLikeIcon;
    public Image reportIcon;
    public Image percentageIcon;
    public Image likePassTagsIcon;
    public enum IconTypes
    {
        SuperLike,
        Report,
        Percentage,
        LikePassTags
    }

    private void Start()
    {
        ResetUnlockUI();
    }
    public void ResetUnlockUI()
    {
        superLikeIcon.gameObject.SetActive(false);
        reportIcon.gameObject.SetActive(false);
        percentageIcon.gameObject.SetActive(false);
        likePassTagsIcon.gameObject.SetActive(false);
    }
    public void UpdateUnlockUI(IconTypes iconType)
    {
        switch (iconType)
        {
            case IconTypes.SuperLike:
                superLikeIcon.gameObject.SetActive(true);
                break;
            case IconTypes.Report:
                reportIcon.gameObject.SetActive(true);
                break;
            case IconTypes.Percentage:
                percentageIcon.gameObject.SetActive(true);
                break;
            case IconTypes.LikePassTags:
                likePassTagsIcon.gameObject.SetActive(true);
                break;
        }
    }
}
