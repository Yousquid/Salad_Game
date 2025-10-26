using UnityEngine;

[System.Serializable]
public class TemperamentDataBase
{
    public string temperament;
    public string[] aboutMeTexts;
    public string[] personalityTexts;
    public string[] taglineTexts;
    public QAData[] questions;
}
[CreateAssetMenu(fileName = "TemperamentDatabaseSO", menuName = "Scriptable Objects/TemperamentDatabaseSO")]
public class TemperamentDatabaseSO : ScriptableObject
{
    public TemperamentDataBase[] temperamentsDatabases;
}
