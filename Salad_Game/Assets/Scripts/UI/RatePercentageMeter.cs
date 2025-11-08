using Sirenix.OdinInspector;
using Unity.UI.Shaders.Sample;
using UnityEngine;

public class RatePercentageMeter : MonoBehaviour
{
    private Meter _meter;
    private void Awake()
    {
        _meter = GetComponent<Meter>();
        SetPercentage(0);
    }
    [Button]
    public void SetPercentage(float percentageT)
    {
        _meter.Value = percentageT;
    }
}
