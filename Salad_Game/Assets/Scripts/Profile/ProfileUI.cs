using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ProfileUI : MonoBehaviour
{
    public ProfileDataGenerator profileDataGenerator;
    public ScrollRect ScrollRect;
    public TMP_Text Name;
    public TMP_Text Age;
    public Image VerifiedIcon;
    public TMP_Text TagLineText;
    public TMP_Text AboutMeText;
    public GameObject InterestBubblePrefab;
    public Transform InterestsBubblesParent;
    public GameObject MoreAboutMeBubblePrefab;
    public Transform MoreAboutMeBubblesParent;
    public GameObject QuestionPrefab;
    public Transform QuestionsParent;
    public RectTransform SuperLike;
    public Vector2 superLikeYRange;
    public Vector2 superLikeLocalScaleRange;
    public float superShowThresholdY = 0.5f;

    private void Update()
    {
        // Debug.Log(ScrollRect.verticalNormalizedPosition);

        if (ScrollRect.verticalNormalizedPosition < superShowThresholdY)
        {
            var y = ExtensionMethods.Map(ScrollRect.verticalNormalizedPosition, 0, superShowThresholdY,
                superLikeYRange.y, superLikeYRange.x);
            y = Mathf.Clamp(y, superLikeYRange.x, superLikeYRange.y);
            var s = ExtensionMethods.Map(ScrollRect.verticalNormalizedPosition, 0, superShowThresholdY,
                superLikeLocalScaleRange.y, superLikeLocalScaleRange.x);
            SuperLike.anchoredPosition = SuperLike.anchoredPosition.SetY(y);
            SuperLike.localScale = new Vector3(s, s, s);
        }
        else
        {
            SuperLike.anchoredPosition = SuperLike.anchoredPosition.SetY(superLikeYRange.x);
        }
    }

    [Button]
    public void TestUpdateUI()
    {
        UpdateUI(profileDataGenerator.GenerateProfileData());
    }

    public void UpdateUI(ProfileData profileData)
    {
        Name.text = profileData.Name;
        Age.text = profileData.Age.ToString();
        VerifiedIcon.enabled = profileData.Verified;
        TagLineText.text = profileData.TagLine;
        AboutMeText.text = profileData.AboutMe;

        for (int i = InterestsBubblesParent.childCount - 1; i >= 0; i--)
        {
            Destroy(InterestsBubblesParent.GetChild(i).gameObject);
        }

        foreach (string interest in profileData.Interests)
        {
            var bubbleText = Instantiate(InterestBubblePrefab, InterestsBubblesParent)
                .GetComponentInChildren<TextMeshProUGUI>();
            bubbleText.text = interest;
        }

        for (int i = MoreAboutMeBubblesParent.childCount - 1; i >= 0; i--)
        {
            Destroy(MoreAboutMeBubblesParent.GetChild(i).gameObject);
        }

        foreach (string moreAboutMe in profileData.MoreAboutMe)
        {
            var moreAboutMeBubbleText = Instantiate(MoreAboutMeBubblePrefab, MoreAboutMeBubblesParent)
                .GetComponentInChildren<TextMeshProUGUI>();
            moreAboutMeBubbleText.text = moreAboutMe;
        }

        for (int i = QuestionsParent.childCount - 1; i >= 0; i--)
        {
            Destroy(QuestionsParent.GetChild(i).gameObject);
        }

        foreach (QAData question in profileData.QAs)
        {
            var questionObject = Instantiate(QuestionPrefab, QuestionsParent).GetComponent<TextMeshProUGUI>();
            questionObject.text = question.question;
            var answerObject = questionObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            answerObject.text = question.answer;
        }
    }
}