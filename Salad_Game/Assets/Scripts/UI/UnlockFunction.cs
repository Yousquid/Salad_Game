using TMPro;
using UnityEngine;

public class UnlockFunction : MonoBehaviour
{
    [Header("References")]
    public UnlockInformation unlockInformation;
    public UnlockUI unlockUI;
    public RatePercentageMeter percentageMeter;
    public TMP_Text unlockText;

    [Header("Unlock Thresholds")]
    public int[] swipesPerStage = new int[] { 5, 7, 10, 12 };

    [Header("Each Stage Unlock Icon & Message")]
    public UnlockUI.IconTypes[] stageIcons = new UnlockUI.IconTypes[]
    {
        UnlockUI.IconTypes.SuperLike,
        UnlockUI.IconTypes.Report,
        UnlockUI.IconTypes.Percentage,
        UnlockUI.IconTypes.LikePassTags
    };
    public string[] stageMessages = new string[]
    {
        "Super Like unlocked!",
        "Report unlocked!",
        "Percentage unlocked!",
        "Like/Pass unlocked!"
    };

    private int stageIndex = 0;
    private int stageStartSwipeCount = 0;

    void Start()
    {
        if (unlockUI != null) unlockUI.ResetUnlockUI();
        stageIndex = 0;
        stageStartSwipeCount = MatchGenerator.totalSwipesNumber;
        if (percentageMeter != null) percentageMeter.SetPercentage(0f);
        UpdateUnlockText();
    }

    void Update()
    {
        int swipeCount = MatchGenerator.totalSwipesNumber;
        CheckUnlocks(swipeCount);
        UpdateProgress(swipeCount);
        UpdateUnlockText();
    }

    private void CheckUnlocks(int total)
    {
        if (!HasStage()) return;

        int need = swipesPerStage[stageIndex];
        int stageProgress = total - stageStartSwipeCount;

        if (stageProgress >= need)
        {
            if (stageIcons != null && stageIndex < stageIcons.Length &&
                stageMessages != null && stageIndex < stageMessages.Length &&
                unlockInformation != null)
            {
                unlockInformation.ShowUnlockInformation(stageIcons[stageIndex], stageMessages[stageIndex]);
            }
            if (unlockUI != null && stageIcons != null && stageIndex < stageIcons.Length)
            {
                unlockUI.UpdateUnlockUI(stageIcons[stageIndex]);
            }

            stageStartSwipeCount = total;
            stageIndex++;
            if (percentageMeter != null) percentageMeter.SetPercentage(0f);
        }
    }

    private void UpdateProgress(int total)
    {
        if (percentageMeter == null) return;
        if (!HasStage()) { percentageMeter.SetPercentage(0f); return; }

        int need = swipesPerStage[stageIndex];
        if (need <= 0) { percentageMeter.SetPercentage(0f); return; }

        int stageProgress = Mathf.Max(0, total - stageStartSwipeCount);
        float normalized = Mathf.Clamp01((float)stageProgress / need);
        percentageMeter.SetPercentage(normalized);
    }

    private bool HasStage()
    {
        return swipesPerStage != null && stageIndex >= 0 && stageIndex < swipesPerStage.Length;
    }

    public int GetCurrentStageRemaining()
    {
        if (!HasStage()) return 0;
        int need = swipesPerStage[stageIndex];
        int stageProgress = Mathf.Max(0, MatchGenerator.totalSwipesNumber - stageStartSwipeCount);
        return Mathf.Max(0, need - stageProgress);
    }

    private void UpdateUnlockText()
    {
        if (unlockText == null) return;

        if (!HasStage())
        {
            unlockText.text = "All features unlocked!";
            return;
        }

        int remaining = GetCurrentStageRemaining();
        unlockText.text = $"Rate <b><color=#00FF03>{remaining}</color></b> More Profiles to Unlock";
    }
}
