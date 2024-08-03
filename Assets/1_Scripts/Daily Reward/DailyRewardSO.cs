using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Custom/DailyRewardSO", fileName = "Daily Reward 00")]
public class DailyRewardSO : ScriptableObject
{
    public int day;
    public Currency rewards = new();
}
