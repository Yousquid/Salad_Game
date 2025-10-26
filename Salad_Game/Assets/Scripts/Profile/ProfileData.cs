using UnityEngine;
[System.Serializable]
public class QAData
{
    public string question;
    [TextArea(2, 5)]
    public string answer;

    public QAData(string question, string answer)
    {
        this.question = question;
        this.answer = answer;
    }
}
[System.Serializable]
public class ProfileData
{
    public string Name;
    public int Age;
    public bool Verified;
    [TextArea(2, 5)]
    public string AboutMe;
    public string[] Interests;
    public string[] MoreAboutMe;
    public QAData[] QAs;
}


