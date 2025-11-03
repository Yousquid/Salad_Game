using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using TMPro;
public class MatchGenerator : MonoBehaviour
{
    public Navigation navigation;
    public  List<ProfileData>  likesData;
    public  List<GameObject> likesGameObjects;
    public  ProfileData superlikeData;
    public  GameObject superlikeGameObject;

    public ProfileData currentMatchData;
    public GameObject currentMatachGameObject;

    public GameObject thisCanvas;

    public bool isMatching = false;

    private float randomPassiveMatchTimer;
    private bool hasRandomed = false;
    private bool hasShown = false;

    public int likesNumber = 0;
    public TextMeshProUGUI matchText;


    void Awake()
    {
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
    }

    public int GetLikeNumbers()
    {
        return likesNumber;
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
                    hasRandomed = false;  // 下次重抽
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
                // 没有可用的 like，重置计时
                hasRandomed = false;
                hasShown = false;
            }
        }
    }

    public void DoMatch()
    {

        ShowMatchUI();
    }
    public void ShowMatchUI()
    {
        isMatching = true;
        thisCanvas.SetActive(true);
        matchText.text = $"You and {currentMatchData.Name} have liked each other!";
    }
}
