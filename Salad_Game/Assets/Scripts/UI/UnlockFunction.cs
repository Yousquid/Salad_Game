using TMPro;
using UnityEngine;

public class UnlockFunction : MonoBehaviour
{
    [Header("References")]
    public UnlockInformation unlockInformation;
    public UnlockUI unlockUI;
    public RatePercentageMeter percentageMeter;
    public TMP_Text unlockText;
    public ProfileUI profileUI;
    public ProfileDataGenerator profileDataGenerator;

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
        if (unlockUI != null)
            unlockUI.ResetUnlockUI();

        stageIndex = 0;
        stageStartSwipeCount = 0;

        if (percentageMeter != null)
            percentageMeter.SetPercentage(0f);

        if (unlockText != null)
            unlockText.text = $"Rate <b><color=#00FF03>{swipesPerStage[0]}</color></b> More Profiles to Unlock";
    }

    void Update()
    {
        int total = MatchGenerator.totalSwipesNumber;
        UpdateProgress(total);
        CheckUnlocks(total);
    }

    private void CheckUnlocks(int total)
    {
        if (!HasStage()) return;

        int need = swipesPerStage[stageIndex];
        int progress = total - stageStartSwipeCount;

        if (progress >= need)
        {
            UnlockCurrentStage();

            stageStartSwipeCount = total;
            stageIndex++;

            if (percentageMeter != null)
                percentageMeter.SetPercentage(0f);

            if (!HasStage() && unlockText != null)
            {
                unlockText.text = "All features unlocked!";
            }
        }
    }

    private void UpdateProgress(int total)
    {
        if (!HasStage() || percentageMeter == null) return;

        int need = swipesPerStage[stageIndex];
        int progress = Mathf.Max(0, total - stageStartSwipeCount);
        float normalized = Mathf.Clamp01((float)progress / need);
        percentageMeter.SetPercentage(normalized);

        int remaining = Mathf.Max(0, need - progress);
        if (unlockText != null)
            unlockText.text = $"Rate <b><color=#00FF03>{remaining}</color></b> More Profiles to Unlock";
    }

    private void UnlockCurrentStage()
    {
        if (stageIndex >= stageIcons.Length || stageIndex >= stageMessages.Length) return;

        UnlockUI.IconTypes icon = stageIcons[stageIndex];
        string msg = stageMessages[stageIndex];

        if (unlockInformation != null)
            unlockInformation.ShowUnlockInformation(icon, msg);

        if (unlockUI != null)
            unlockUI.UpdateUnlockUI(icon);

        if (profileUI != null)
        {
            if (icon == UnlockUI.IconTypes.SuperLike)
                profileUI.showSuperLike = true;

            if (icon == UnlockUI.IconTypes.Report)
                profileUI.showReport = true;
        }

        if (icon == UnlockUI.IconTypes.Percentage && profileDataGenerator != null)
        {
            ApplyThirdStageAdjustments(profileDataGenerator);
        }
    }

    private bool HasStage()
    {
        return swipesPerStage != null && stageIndex >= 0 && stageIndex < swipesPerStage.Length;
    }

    private void ApplyThirdStageAdjustments(ProfileDataGenerator gen)
    {
        gen.interestsAmtRange = new Vector2Int(1, 4);
        gen.qaAmtRange = new Vector2Int(1, 3);
        gen.personalitiesAmtRange = new Vector2Int(2, 6);
        gen.temperamentAmtRange = new Vector2Int(2, 6);
    }
}
