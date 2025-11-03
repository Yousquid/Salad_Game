using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct PaymentSuccessEvent
{
    public string PaymentDetail;

    public PaymentSuccessEvent(string paymentDetail)
    {
        this.PaymentDetail = paymentDetail;
    }
}
public class PaymentPanel : MonoBehaviour
{
    public float enableYPos;
    public float disableYPos;
    public float lerpTime;
    private RectTransform _rectTransform;
    public string cardNumber;
    public TMP_InputField cardNumberInput;
    public string cardMM;
    public TMP_InputField cardMMInput;
    public string cardYY;
    public TMP_InputField cardYYInput;
    public Image retryImage;
    
    private string _currentPaymentDetail;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = _rectTransform.anchoredPosition.SetY(disableYPos);
        retryImage.color = retryImage.color.SetAlpha(0);
        // EnablePaymentPanel("111");
    }
    public void ProcessPayment()
    {
        if (cardNumberInput.text == cardNumber && cardMMInput.text == cardMM && cardYYInput.text == cardYY)
        {
            EventBetter.Raise(new PaymentSuccessEvent(_currentPaymentDetail));
            DisablePaymentPanel();
        }
        else
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(retryImage.DOFade(1f, lerpTime * 0.5f)) 
                .AppendInterval(1f)
                .Append(retryImage.DOFade(0f, lerpTime * 0.5f));
        }
    }
    public void EnablePaymentPanel(string paymentDetail)
    {
        _currentPaymentDetail = paymentDetail;
        _rectTransform.DOAnchorPosY(enableYPos, lerpTime);
    }
    public void DisablePaymentPanel()
    {
        _rectTransform.DOAnchorPosY(disableYPos, lerpTime);
    }
}
