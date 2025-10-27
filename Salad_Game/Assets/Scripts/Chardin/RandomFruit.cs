using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RandomFruit : MonoBehaviour
{
    [Header("List 1: Eyes")]
    public List<GameObject> partListA;

    [Header("List 2: Body")]
    public List<GameObject> partListB;

    [Header("List 3: Decoration")]
    public List<GameObject> partListC;

    [Header("Position")]
    public Transform spawnPoint;

    private GameObject currentCombined;

    void Start()
    {
        if (spawnPoint == null)
        {
            GameObject go = new GameObject("SpawnPoint");
            go.transform.position = Vector3.zero;
            spawnPoint = go.transform;
        }

        CombineRandom();
    }

    /*void Update()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            CombineRandom();
        }
    }*/


    public void CombineRandom()
    {
        if (currentCombined != null)
        {
            Destroy(currentCombined);
        }

        currentCombined = new GameObject("Fruit");
        currentCombined.transform.position = spawnPoint.position;

        GameObject a = partListA[Random.Range(0, partListA.Count)];
        GameObject b = partListB[Random.Range(0, partListB.Count)];
        GameObject c = partListC[Random.Range(0, partListC.Count)];

        GameObject objA = Instantiate(a, spawnPoint.position, Quaternion.identity, currentCombined.transform);
        GameObject objB = Instantiate(b, spawnPoint.position, Quaternion.identity, currentCombined.transform);
        GameObject objC = Instantiate(c, spawnPoint.position, Quaternion.identity, currentCombined.transform);

        objA.transform.localPosition = new Vector3(0, 0, 0);
        objB.transform.localPosition = new Vector3(0, 0, 0);
        objC.transform.localPosition = new Vector3(0, 0, 0);
    }
}
