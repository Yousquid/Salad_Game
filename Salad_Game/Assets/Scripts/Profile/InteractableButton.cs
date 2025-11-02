using System;
using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    public GameObject selectedImage;
    private SelfProfileUI _selfProfileUI;
    private bool _isSelected;

    private void Awake()
    {
        _selfProfileUI = GameObject.FindAnyObjectByType<SelfProfileUI>();
    }

    public void OnButtonClicked()
    {
        if (_isSelected)
        {
            _selfProfileUI.RemoveInterest();
            selectedImage.SetActive(false);
            _isSelected = false;
        }
        else
        {
            if (_selfProfileUI.TryAddInterest())
            {
                selectedImage.SetActive(true);
                _isSelected = true;
            }
        }
    }
}