using UnityEngine;

public class UnlockFunction : MonoBehaviour
{
    [Header("References")]
    public UnlockInformation unlockInformation;
    public UnlockUI unlockUI;

    [Header("Unlock Thresholds")]
    public int unlockSuperLikeAt = 3;
    public int unlockReportAt = 6;
    public int unlockPercentageAt = 10;
    public int unlockLikePassTagsAt = 15;

    private bool superLikeUnlocked = false;
    private bool reportUnlocked = false;
    private bool percentageUnlocked = false;
    private bool likePassTagsUnlocked = false;

    void Start()
    {
        if (unlockUI != null)
            unlockUI.ResetUnlockUI();
    }

    void Update()
    {
        int swipeCount = MatchGenerator.totalSwipesNumber;
        CheckUnlocks(swipeCount);
    }

    private void CheckUnlocks(int swipeCount)
    {
        // Super Like
        if (!superLikeUnlocked && swipeCount >= unlockSuperLikeAt)
        {
            superLikeUnlocked = true;
            unlockInformation.ShowUnlockInformation(
                UnlockUI.IconTypes.SuperLike,
                "Super Like unlocked!");
        }

        // Report
        if (!reportUnlocked && swipeCount >= unlockReportAt)
        {
            reportUnlocked = true;
            unlockInformation.ShowUnlockInformation(
                UnlockUI.IconTypes.Report,
                "Report unlocked!");
        }

        // Percentage
        if (!percentageUnlocked && swipeCount >= unlockPercentageAt)
        {
            percentageUnlocked = true;
            unlockInformation.ShowUnlockInformation(
                UnlockUI.IconTypes.Percentage,
                "Percentage Unlocked!");
        }

        // Like/Pass Tags
        if (!likePassTagsUnlocked && swipeCount >= unlockLikePassTagsAt)
        {
            likePassTagsUnlocked = true;
            unlockInformation.ShowUnlockInformation(
                UnlockUI.IconTypes.LikePassTags,
                "Like/Pase Unlocked!");
        }
    }
}
