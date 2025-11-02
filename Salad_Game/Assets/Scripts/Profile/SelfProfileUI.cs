using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelfProfileUI : ProfileUI
{
    [Header("Self Profile")]
    public GridLayoutGroup interestsGrid;
    public Vector2Int interestsMinMax;
    public int CurrentInterestsAmt { get; set; }
    public InterestsDatabaseSO interestsDatabase;

    public TMP_Text question;
    public QADatabaseSO qaDatabase;

    private void Start()
    {
        ScrollRect.verticalNormalizedPosition = 1;
        
        UpdateInterestsGrid();
        UpdateQuestion();
    }

    [Sirenix.OdinInspector.Button]
    public void UpdateInterestsGrid()
    {
        for (int i = interestsGrid.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(interestsGrid.transform.GetChild(i).gameObject);
        }
        
        foreach (string moreAboutMe in interestsDatabase.interests)
        {
            var moreAboutMeBubbleText = Instantiate(MoreAboutMeBubblePrefab, interestsGrid.transform)
                .GetComponentInChildren<TextMeshProUGUI>();
            moreAboutMeBubbleText.text = moreAboutMe;
        }
    }

    public void UpdateQuestion()
    {
        var questions = qaDatabase.QaDatas.Select(qa => qa.question).ToList();
        var select = questions[Random.Range(0, questions.Count)];
        question.text = select;
    }
    public bool TryAddInterest()
    {
        if (CurrentInterestsAmt < interestsMinMax.y)
        {
            CurrentInterestsAmt++;
            return true;
        }
        return false;
    }

    public void RemoveInterest()
    {
        CurrentInterestsAmt--;
    }
}
