using UnityEngine;
[System.Serializable]
public class Question
{
    public string question;
    [TextArea(2, 5)]
    public string answer;

    public Question(string question, string answer)
    {
        this.question = question;
        this.answer = answer;
    }
}
[CreateAssetMenu(fileName = "ProfileDataSO", menuName = "Scriptable Objects/ProfileDataSO")]
public class ProfileDataSO : ScriptableObject
{
    public string Name;
    public int Age;
    public bool Verified;
    [TextArea(2, 5)]
    public string AboutMe;
    public string[] Interests;
    public string[] MoreAboutMe;
    public Question[] Questions;
}
