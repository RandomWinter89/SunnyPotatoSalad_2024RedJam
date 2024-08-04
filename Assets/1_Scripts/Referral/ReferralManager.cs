using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.VersionControl;

public class ReferralManager : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject subPanel;

    [SerializeField] TMP_Text referralCodeText;
    [SerializeField] TMP_InputField referralCodeInput;
    [SerializeField] Button applyCodeBtn;
    [SerializeField] Button copyCodeBtn;

    public ReferralCode Referral => DataManager.main.referral;

    private void Awake()
    {
        applyCodeBtn.onClick.AddListener(ApplyCode);

        if (Referral != null)
        {
            referralCodeText.SetText(Referral.code);
        }

        if (Referral != null && Referral.count > 0)
        {
            int reward = Referral.count * 2;

            PromptData data = new();
            data.title = "Referral Success!";
            data.description = $"You’ve earned {Referral.count} referrals!\n\nClaim {reward} Tickets now!";
            data.action = OwnerClaimReward;
            data.confirmText = "Claim";
            data.disableBackButton = true;
            data.enablePanel = true;

            PromptManager.Prompt(data);
        }
    }

    private void OwnerClaimReward()
    {
        int reward = Referral.count * 2;
        DataManager.main.playerData.AddTicket(reward);
        Referral.count = 0;

        PromptManager.Prompt("Referral Claimed", $"You had claimed {reward} tickets!");
    }

    private void ClientClaimReward()
    {
        DataManager.main.playerData.AddAirAsiaPoint(1);
        PromptManager.Prompt("Referral Claimed", $"You had claimed {1} Air Asia Point!");
    }

    private void ApplyCode()
    {
        string code = referralCodeInput.text;

        if (string.IsNullOrEmpty(referralCodeInput.text))
        {
            PromptManager.Prompt("Failed Apply Code", "The code is empty, please enter you frens code and apply.");
            return;
        }

        StartCoroutine(MongoUtils.PostReferral(code,
            message =>
            {
                if (message == "success")
                {
                    mainPanel.SetActive(false);
                    subPanel.SetActive(false);
                    ClientClaimReward();
                }
                else
                {
                    PromptManager.Prompt("Failed Apply Code", $"Failed to apply code due to \n\n{message}, \n\nplease report to developer.");
                }
            }));
    }
}
