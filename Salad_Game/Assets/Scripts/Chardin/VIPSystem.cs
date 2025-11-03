using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VipSystemTMP : MonoBehaviour
{
    [Header("UI References")]
    public GameObject vipPanel;
    public TextMeshProUGUI vipText;
    public TMP_InputField chargeInput;
    public Button viewVipButton;
    public Button chargeButton;
    public Button closeButton;
    public PaymentPanel paymentPanel;


    [Header("VIP Data")]
    public int currentVip = 0;
    public float totalMoney = 0f;

    private void Start()
    {
        vipPanel.SetActive(false);

        viewVipButton.onClick.AddListener(OpenVipPanel);
        chargeButton.onClick.AddListener(Charge);
        closeButton.onClick.AddListener(CloseVipPanel);

        UpdateVipText();
    }

    void OpenVipPanel()
    {
        vipPanel.SetActive(true);
    }

    void CloseVipPanel()
    {
        vipPanel.SetActive(false);
    }

    void Charge()
    {
        if (float.TryParse(chargeInput.text, out float money) && money > 0)
        {
            totalMoney += money;

            currentVip = Mathf.Clamp((int)(totalMoney / 1000000f), 0,100000000);

            paymentPanel.EnablePaymentPanel("XXX");

            UpdateVipText();
            chargeInput.text = "";
        }
        else
        {
            Debug.LogWarning("Please enter a valid number");
        }
    }

    void UpdateVipText()
    {
        vipText.text = $"<b><color=#000000>Current VIP level: </color></b><color=#C77DFF>VIP{currentVip}</color>\n" +
        $"<b><color=#000000>Money Invested: </color></b><color=#C77DFF>${totalMoney:F2}</color>";
    }
}
