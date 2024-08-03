using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using AYellowpaper.SerializedCollections;
using static PlayFabKeys;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] int maxCount = 10;
    [SerializeField] GameObject leaderboardPanel;
    [SerializeField] SerializedDictionary<string, LeaderboardUI> leaderboardUIs = new();

    public void LoadLeaderboard(string key)
    {
        PlayFabUtils.GetLeaderboard(key, maxCount, (leaderboardData) =>
        {
            leaderboardUIs[key].GenerateLeaderboardEntryUI(leaderboardData);
            leaderboardPanel.SetActive(true);
        });
    }
}
