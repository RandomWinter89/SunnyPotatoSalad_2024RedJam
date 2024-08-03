using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] Transform entriesContainer;
    [SerializeField] LeaderboardEntryUI entryUIPrefab;

    public void GenerateLeaderboardEntryUI(List<PlayerLeaderboardEntry> leaderboard)
    {
        entriesContainer.DestroyAllChilds();

        for (int i = 0; i < leaderboard.Count; i++)
        {
            LeaderboardEntryUI entryUI = Instantiate(entryUIPrefab, entriesContainer);
            entryUI.Init(leaderboard[i]);
        }
    }
}
