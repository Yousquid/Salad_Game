using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class PercentageReport : MonoBehaviour
{
    public float animationDurationInSeconds = 1f;
    public Vector2 percentageReportLeftMinMaxX;
    public Image percentageReportLeft;
    public TMP_Text percentageReportTextLeft;
    public Vector2 percentageReportRightMinMaxX;
    public Image percentageReportRight;
    public TMP_Text percentageReportTextRight;

    private void Start()
    {
        ResetPercentageReport();
    }

    public void FadeInPercentageReport()
    {
        percentageReportLeft.rectTransform.DOAnchorPosX(percentageReportLeftMinMaxX.y, animationDurationInSeconds);
        percentageReportRight.rectTransform.DOAnchorPosX(percentageReportRightMinMaxX.y, animationDurationInSeconds);
        Debug.Log(ExtensionMethods.GaussianRandom());
        var rLeft = GetGuassianRandom();
        var rRight = 100 - rLeft;
        percentageReportTextLeft.text = rLeft.ToString() + "%";
        percentageReportTextRight.text = rRight.ToString()+ "%";
    }

    private float GetGuassianRandom()
    {
        var result = Mathf.FloorToInt(ExtensionMethods.GaussianRandom() * 15) + 50;
        while (result < 0 || result > 100)
        {
            result = Mathf.FloorToInt(ExtensionMethods.GaussianRandom() * 15) + 50;
        }
        return result;
    }
    public void FadeOutPercentageReport()
    {
        percentageReportLeft.rectTransform.DOAnchorPosX(percentageReportLeftMinMaxX.x, animationDurationInSeconds);
        percentageReportRight.rectTransform.DOAnchorPosX(percentageReportRightMinMaxX.x, animationDurationInSeconds);
    }
    private void ResetPercentageReport()
    {
        percentageReportLeft.rectTransform.DOAnchorPosX(percentageReportLeftMinMaxX.x, 0);
        percentageReportRight.rectTransform.DOAnchorPosX(percentageReportRightMinMaxX.x, 0);
    }
}
