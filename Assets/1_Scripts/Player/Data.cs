using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [field:SerializeField] public int Highscore { get; private set; }
    [field:SerializeField] public Currency Currency { get; private set; }

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
    public int dayStreak;
    public DateTime lastLoginTime;
}

[Serializable]
public class Currency
{
    public int ticketCount;
    public int airAsiaPoint;
}