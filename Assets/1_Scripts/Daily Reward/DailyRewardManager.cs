using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using TMPro;
using static PlayFabKeys;

public class DailyRewardManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool alwaysCanClaim;

    [Header("References")]
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text dayStrekText;
    [SerializeField] Button claimBtn;
    
    [SerializeField] GameObject ticketSection;
    [SerializeField] GameObject pointSection;
    [SerializeField] TMP_Text ticketCountText;
    [SerializeField] TMP_Text pointCountText;

    [Header("Data")]
    [SerializeField] SerializedDictionary<int, Currency> dailyRewardStats = new();

    private const int ClaimHour = 08;
    private const int ClaimMinutes = 00;

    public void Awake()
    {
        var data = DataManager.main;

        bool canClaim = CanClaim();
        panel.SetActive(canClaim);

        if (canClaim)
        {
            claimBtn.onClick.AddListener(ClaimReward);
            PrepareReward();
        }
    }

    private bool CanClaim()
    {
        return WorldTimer.UtcNow > DataManager.main.dailyReward.GetNextClaimTime() || alwaysCanClaim;
    }

    private bool LoseStreak()
    {
        var data = DataManager.main;

        DateTime lastClaimTime = data.dailyReward.GetNextClaimTime().AddDays(-1);
        DateTime utcNow = WorldTimer.UtcNow;

        // Check if more than 24 hours have passed since the last claim
        if (utcNow > lastClaimTime.AddHours(24))
        {
            return true;
        }
        return false;
    }

    public static DateTime GetNextClaimTime()
    {
        DateTime utcNow = WorldTimer.UtcNow;
        DateTime checkInResetTime = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, ClaimHour, ClaimMinutes, 0, DateTimeKind.Utc);

        // make sure the claim time is not passed current time
        if (utcNow > checkInResetTime)
        {
            checkInResetTime = checkInResetTime.AddDays(1);
        }

        return checkInResetTime;
    }

    private void PrepareReward()
    {
        var data = DataManager.main;

        if (data.dailyReward.dayStreak >= 7 || LoseStreak())
        {
            // reset cycle
            data.dailyReward.dayStreak = 1;
        }
        else
        {
            // streak days
            data.dailyReward.dayStreak += 1;
        }

        int ticketReward = dailyRewardStats[data.dailyReward.dayStreak].ticketCount;
        int airAsiaPointsReward = dailyRewardStats[data.dailyReward.dayStreak].airAsiaPoint;

        ticketSection.SetActive(ticketReward > 0);
        pointSection.SetActive(airAsiaPointsReward > 0);

        ticketCountText.SetText($"x{ticketReward}");
        pointCountText.SetText($"x{airAsiaPointsReward}");

        data.dailyReward.SetNextClaimTime(GetNextClaimTime());

        PlayFabUtils.Save<DailyCheckIn>(P_DAILY_CHECK_IN, data.dailyReward);
    }

    private void ClaimReward()
    {
        var data = DataManager.main;

        int ticketReward = dailyRewardStats[data.dailyReward.dayStreak].ticketCount;
        int airAsiaPointsReward = dailyRewardStats[data.dailyReward.dayStreak].airAsiaPoint;

        data.playerData.AddTicket(ticketReward);
        data.playerData.AddAirAsiaPoint(airAsiaPointsReward);

        panel.SetActive(false);
    }
}
