using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int highscore;
    public Currency currency;
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