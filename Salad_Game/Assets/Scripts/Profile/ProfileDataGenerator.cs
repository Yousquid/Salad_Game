using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ProfileDataGenerator : MonoBehaviour
{
    public TemperamentDatabaseSO temperamentDatabase;
    public Vector2Int ageAmtRange;
    [Range(0, 1)] public float verifiedRate;
    public Vector2Int personalitiesAmtRange;
    public Vector2Int tagsAmtRange;
    public TagsDatabaseSO tagsDatabase;
    public Vector2Int interestsAmtRange;
    public InterestsDatabaseSO interestsDatabase;
    public Vector2Int qaAmtRange;
    public QADatabaseSO qaDatabase;
    
    public ProfileData GenerateProfileData(string[] temperaments)
    {
        var dataBases = new TemperamentDataBase[temperaments.Length];
        var allDatabases = temperamentDatabase.temperamentsDatabases;
        for (int i = 0; i < dataBases.Length; i++)
        {
            for (int j = 0; j < allDatabases.Length; j++)
            {
                if (allDatabases[j].temperament == temperaments[i])
                {
                    dataBases[i] = allDatabases[j];
                }
            }
        }

        ProfileData profileData = new ProfileData();
        profileData.Name = Random.Range(0, 10000).ToString();
        profileData.Age = Random.Range(ageAmtRange.x, ageAmtRange.y + 1);
        profileData.Verified = Random.Range(0f, 1f) < verifiedRate;
        profileData.AboutMe = GetAboutMe(dataBases);
        profileData.Interests = GetFromDatabase(interestsAmtRange, interestsDatabase.interests);
        profileData.MoreAboutMe = GetMoreAboutMe(dataBases);
        profileData.QAs = GetFromDatabase(qaAmtRange, qaDatabase.QaDatas);
        return profileData;
    }

    private string GetAboutMe(TemperamentDataBase[] dataBases)
    {
        var aboutMes = new List<string>();
        Debug.Log(dataBases.Length);
        for (int i = 0; i < dataBases.Length; i++)
        {
            aboutMes = aboutMes.Concat(dataBases[i].aboutMeTexts).ToList();
        }
        return aboutMes[Random.Range(0, aboutMes.Count)];
    }
    private const int MAX_TRIES = 100;
    private int _maxTriesCounter;

    private T[] GetFromDatabase<T>(Vector2Int amountRange, T[] database)
    {
        var results = new T[Random.Range(amountRange.x, amountRange.y + 1)];
        _maxTriesCounter = 0;
        for (int i = 0; i < results.Length; i++)
        {
            var r = Random.Range(0, database.Length);
            var selected = database[r];
            if (!results.Contains(selected) || _maxTriesCounter > MAX_TRIES)
            {
                results[i] = selected;
                _maxTriesCounter = 0;
            }
            else
            {
                i--;
                _maxTriesCounter++;
            }
        }

        return results;
    }
    private string[] GetMoreAboutMe(TemperamentDataBase[] dataBases)
    {
        var tags = GetFromDatabase(tagsAmtRange, tagsDatabase.tags);
        
        var personalitiesDatabase = new List<string>();
        for (int i = 0; i < dataBases.Length; i++)
        {
            personalitiesDatabase = personalitiesDatabase.Concat(dataBases[i].personalityTexts).ToList();
        }

        var personalities = GetFromDatabase(personalitiesAmtRange, personalitiesDatabase.ToArray());
        var list = personalities.Concat(tags).ToList();
        return list.ShuffleList().ToArray();
    }
}
