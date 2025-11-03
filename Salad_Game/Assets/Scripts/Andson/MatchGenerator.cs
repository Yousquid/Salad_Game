using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using TMPro;
public class MatchGenerator : MonoBehaviour
{
    public Navigation navigation;
    public static List<ProfileData>  likesData;
    public static List<GameObject> likesGameObjects;
    public static ProfileData superlikeData;
    public static GameObject superlikeGameObject;

    public ProfileData currentMatchData;
    public GameObject currentMatachGameObject;

    public GameObject thisCanvas;

    private float randomPassiveMatchTimer;
    private bool hasRandomed = false;
    private bool hasShown = false;

    public TextMeshProUGUI matchText;

    private void Start()
    {
        navigation = GetComponent<Navigation>();


        thisCanvas.SetActive(false);
    }
    private void Update()
    {
        RunMatchTimer();
    }

    public int GetLikeNumbers()
    {
        return likesData.Count;
    }

    public void OnClickSendAText()
    {
        
    }

    public void OnClickKeepSwiping()
    {
        thisCanvas.SetActive(false);
        navigation.GenerateNewProfile();
        hasRandomed = false;
        hasShown = false;

    }

    private void RunMatchTimer()
    {
        if (!hasRandomed)
        {
            randomPassiveMatchTimer = Random.Range(20, 100);
            hasRandomed = true;

        }

        if (randomPassiveMatchTimer >= 0)
        {
            randomPassiveMatchTimer -= Time.deltaTime;
        }

        if (randomPassiveMatchTimer < 0 && hasRandomed && !hasShown)
        {
            if (likesData.Count >= 1)
            {
                //currentMatchData = likesData[Random.Range(0, likesData.Count)];
               // currentMatachGameObject = likesGameObjects[Random.Range(0, likesData.Count)];
                //ShowMatchUI();
                //Instantiate(currentMatachGameObject);
                hasShown = true;
            }
            else
            {
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
        thisCanvas.SetActive(true);
        matchText.text = $"You and {currentMatchData.Name} have liked each other!";
    }
}
