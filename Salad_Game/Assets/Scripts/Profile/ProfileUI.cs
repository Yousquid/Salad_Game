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

    [Header("Super Like")] public RectTransform SuperLike;
    public Vector2 superLikeYRange;
    public Vector2 superLikeLocalScaleRange;
    public float superShowThresholdY = 0.5f;
    public float superShowThresholdYMin = 0.2f;
    public float superLikeMaxScale = 1.8f;

    [Header("Like & Nope")] public RectTransform Like;
    public Vector2 likeRangeX;
    public Vector2 likeRangeScale;
    public Vector2 likeRotationMapZ;
    public RectTransform Nope;
    public Vector2 nopeRangeX;
    public Vector2 nopeRotationMapZ;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        if (Like == null)
        {
            Like = GameObject.Find("Like").GetComponent<RectTransform>();
        }

        if (Nope == null)
        {
            Nope = GameObject.Find("Nope").GetComponent<RectTransform>();       
        }
    }

    private void Update()
    {
        // Handle super like
        if (ScrollRect.verticalNormalizedPosition < superShowThresholdY)
        {
            var y = ExtensionMethods.Map(ScrollRect.verticalNormalizedPosition, superShowThresholdYMin,
                superShowThresholdY,
                superLikeYRange.y, superLikeYRange.x);
            y = Mathf.Clamp(y, superLikeYRange.x, superLikeYRange.y);
            SuperLike.anchoredPosition = SuperLike.anchoredPosition.SetY(y);
            
            var s = ExtensionMethods.Map(ScrollRect.verticalNormalizedPosition, superShowThresholdYMin,
                superShowThresholdY,
                superLikeLocalScaleRange.y, superLikeLocalScaleRange.x);
            s = Mathf.Clamp(s, superLikeLocalScaleRange.x, superLikeMaxScale);
            SuperLike.localScale = new Vector3(s, s, s);
        }

        // Handle like
        if (Like != null)
        {
            var rotationZ = _rectTransform.eulerAngles.z;
            if (Mathf.Abs(rotationZ) > 180)
            {
                rotationZ = Mathf.Abs(rotationZ) - 360;
            }

            var x = ExtensionMethods.Map(rotationZ, likeRotationMapZ.x, likeRotationMapZ.y,
                likeRangeX.x, likeRangeX.y);
            Like.anchoredPosition = Like.anchoredPosition.SetX(x);
            
            var s = ExtensionMethods.Map(rotationZ, likeRotationMapZ.x, likeRotationMapZ.y,
                likeRangeScale.x, likeRangeScale.y);
            Like.localScale = new Vector3(s, s, s);
        }
        
        if (Nope != null)
        {
            var rotationZ = _rectTransform.eulerAngles.z;
            if (Mathf.Abs(rotationZ) > 180)
            {
                rotationZ = Mathf.Abs(rotationZ) - 360;
            }

            var x = ExtensionMethods.Map(rotationZ, nopeRotationMapZ.x, nopeRotationMapZ.y,
                nopeRangeX.x, nopeRangeX.y);
            Nope.anchoredPosition = Nope.anchoredPosition.SetX(x);
            
            var s = ExtensionMethods.Map(rotationZ, nopeRotationMapZ.x, nopeRotationMapZ.y,
                likeRangeScale.x, likeRangeScale.y);
            Nope.localScale = new Vector3(s, s, s);
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
        Age.text = profileData.Age.ToString() + " mos";
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