using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
public class MatchGenerator : MonoBehaviour
{
    public Navigation navigation;
    public static List<ProfileData>  likesData;
    public static List<GameObject> likesGameObjects;


    private void Start()
    {
        navigation = GetComponent<Navigation>();
    }
    private void Update()
    {
        
    }

    public int GetLikeNumbers()
    {
        return likesData.Count;
    }
}
