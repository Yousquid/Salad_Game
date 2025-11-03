using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string npcName;
    [TextArea(2, 5)]
    public string[] sentences;
}
