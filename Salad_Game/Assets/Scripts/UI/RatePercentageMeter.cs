using Sirenix.OdinInspector;
using Unity.UI.Shaders.Sample;
using UnityEngine;
using UnityEngine.UI;

public class RatePercentageMeter : MonoBehaviour
{
    private Meter _meter;

    private void Awake()
    {
        _meter = GetComponent<Meter>();
        if (_meter != null) _meter.Value = 0f;
    }

    public void SetPercentage(float normalized)
    {
        if (_meter == null) return;
        _meter.Value = Mathf.Clamp01(normalized);
    }
}
