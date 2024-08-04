using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [field: SerializeField] public int Highscore { get; private set; }
    [field: SerializeField] public Currency Currency { get; private set; } = new();

    public static event Action OnPlayerInfoUpdated;

    public void SetHighscore(int highscore)
    {
        this.Highscore = highscore;
        OnPlayerInfoUpdated?.Invoke();
    }

    public void AddTicket(int count)
    {   
        Currency.ticketCount += count;
        OnPlayerInfoUpdated?.Invoke();
    }

    public void AddAirAsiaPoint(int point)
    {
        Currency.airAsiaPoint += point;
        OnPlayerInfoUpdated?.Invoke();
    }
}

[Serializable]
public class DailyCheckIn
{
    public int dayStreak = 1;
    public string nextClaimTime;

    public DateTime GetNextClaimTime()
    {
        return nextClaimTime.ParseToKLDateTime();
    }

    public void SetNextClaimTime(DateTime dateTime)
    {
        nextClaimTime = dateTime.ToKLTimeString();
    }
}

[Serializable]
public class Currency
{
    public int ticketCount;
    public int airAsiaPoint;
}

[Serializable]
public class ReferralCode
{
    public string _id; // playfabID
    public string code;
    public int count;
}