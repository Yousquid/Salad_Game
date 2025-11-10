using System;
using UnityEngine;
using UnityEngine.UI;

public class UnlockUI : MonoBehaviour
{
    public Image superLikeIcon;
    public Image reportIcon;
    public Image percentageIcon;
    public enum IconTypes
    {
        SuperLike,
        Report,
        Percentage,
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
        }
    }
}
