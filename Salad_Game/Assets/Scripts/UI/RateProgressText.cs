using System;
using TMPro;
using UnityEngine;

public class RateProgressText : MonoBehaviour
{
    private TMP_Text _rateProgressText;
    private string _initialText;

    private void Awake()
    {
        _rateProgressText = GetComponent<TMP_Text>();
        _initialText = _rateProgressText.text;
    }

    public void UpdateProgress(int remainingProfiles)
    {
        _rateProgressText.text = _initialText.Replace("X", remainingProfiles.ToString());
    }
}
