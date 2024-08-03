using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class DailyRewardManager : MonoBehaviour
{
    [SerializeField] SerializedDictionary<int, Currency> dailyRewardStats = new();

    public void Awake()
    {
        var data = DataManager.main;

        DateTime lastLoginTime = data.dailyReward.lastLoginTime;
        DateTime currentTime = WorldTimer.UtcNow;
        TimeSpan timeDifference = currentTime - lastLoginTime;

        if (timeDifference.TotalHours > 24 || data.dailyReward.dayStreak >= 7)
        {
            data.dailyReward.dayStreak = 1;
        }
        else
        {
            data.dailyReward.dayStreak += 1;
        }

        data.dailyReward.lastLoginTime = currentTime;

        int ticketReward = dailyRewardStats[data.dailyReward.dayStreak].ticketCount;
        int airAsiaPointsReward = dailyRewardStats[data.dailyReward.dayStreak].airAsiaPoint;

        data.playerData.AddTicket(ticketReward);
        data.playerData.AddAirAsiaPoint(airAsiaPointsReward);
    }
}
