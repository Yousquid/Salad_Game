using System.Collections;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using TMPro;
public class MatchGenerator : Singleton<MatchGenerator>
{
    public Navigation navigation;
    public  List<ProfileData>  likesData;
    public  List<GameObject> likesGameObjects;
    public  ProfileData superlikeData;
    public  GameObject superlikeGameObject;
    public GameObject matchEffect;

    public ProfileData currentMatchData;
    public GameObject currentMatachGameObject;

    public GameObject thisCanvas;

    public bool isMatching = false;

    private float randomPassiveMatchTimer;
    private bool hasRandomed = false;
    private bool hasShown = false;

    public int likesNumber = 0;
    public int unlikesNumber = 0;
    public int totalSwipesNumber = 0;

    public TextMeshProUGUI matchText;


    protected override void Awake()
    {
        base.Awake();
        
        if (likesData == null) likesData = new List<ProfileData>();
        if (likesGameObjects == null) likesGameObjects = new List<GameObject>();
    }
    private void Start()
    {
        navigation = GetComponent<Navigation>();


        thisCanvas.SetActive(false);
    }
    private void Update()
    {
        //RunMatchTimer();

        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlayClick();
        }
    }

    public int GetLikeNumbers()
    {
        return likesNumber;
    }

    public int GetUnlikeNumbers()
    {
        return unlikesNumber;
    }

    public int GetTotalSwipes()
    {
        return totalSwipesNumber;
    }

    public void RecordLike()
    {
        likesNumber++;
        totalSwipesNumber++;
    }

    public void RecordUnlike()
    {
        unlikesNumber++;
        totalSwipesNumber++;
    }

    public void RecordSuperLike()
    {
        totalSwipesNumber++;

    }
    public void RecordReport()
    {
        totalSwipesNumber++;

    }
    public void OnClickSendAText()
    {
        
    }

    public void OnClickKeepSwiping()
    {
        thisCanvas.SetActive(false);
        navigation.GenerateNewProfile();
        hasRandomed = false;
        isMatching = false;
        hasShown = false;
        GameObject oldObject = GameObject.Find("Fruit");
        oldObject.SetActive(true);
    }

    private void RunMatchTimer()
    {
        if (!hasRandomed)
        {
            randomPassiveMatchTimer = Random.Range(2f, 10f);
            hasRandomed = true;
        }

        if (randomPassiveMatchTimer >= 0f)
            randomPassiveMatchTimer -= Time.deltaTime;

        if (randomPassiveMatchTimer < 0f && hasRandomed && !hasShown)
        {
            if (likesData == null) likesData = new List<ProfileData>();
            if (likesGameObjects == null) likesGameObjects = new List<GameObject>();

            for (int i = likesGameObjects.Count - 1; i >= 0; i--)
            {
                if (likesGameObjects[i] == null)
                {
                    if (i < likesData.Count) likesData.RemoveAt(i);
                    likesGameObjects.RemoveAt(i);
                }
            }

            if (likesData.Count >= 1 && likesGameObjects.Count == likesData.Count)
            {
                int randomMatchNPC = Random.Range(0, likesData.Count);
                currentMatchData = likesData[randomMatchNPC];
                var currentMatchGameObject = likesGameObjects[randomMatchNPC];

                if (currentMatchData == null || currentMatchGameObject == null)
                {
                    hasRandomed = false;  // �´��س�
                    return;
                }

                ShowMatchUI();

                GameObject newMatchIcon = Instantiate(currentMatchGameObject);
                newMatchIcon.name = "NewIcon";

                var oldObject = GameObject.Find("Fruit");
                if (oldObject != null) oldObject.SetActive(false);

                hasShown = true;
            }
            else
            {
                // û�п��õ� like�����ü�ʱ
                hasRandomed = false;
                hasShown = false;
            }
        }
    }

    public void DoMatch()
    {
        ShowMatchUI();
        if(matchEffect != null)
        {
            var e = Instantiate(matchEffect);
            StartCoroutine(DelayedDestroy(e, 10f));
        }
    }
    IEnumerator DelayedDestroy(GameObject objectToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(objectToDestroy);
    }
    public void ShowMatchUI()
    {
        isMatching = true;
        thisCanvas.SetActive(true);
        EventBetter.Raise(new PlaySFXEvent(PlaySFXEvent.SFXType.Match));
        matchText.text = $"You and {currentMatchData.Name} have liked each other!";
    }
}
