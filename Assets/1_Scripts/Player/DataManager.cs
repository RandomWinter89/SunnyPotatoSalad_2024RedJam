using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayFabKeys;

public class DataManager : MonoBehaviour
{
    public string sessionTicket;
    public string playFabID;
    public PlayerData playerData = new();
    public DailyCheckIn dailyReward = new();

    public static DataManager main;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadPlayerDataRoutine()
    {
        yield return PlayFabUtils.LoadData<PlayerData>(P_PLAYER_DATA,
        data =>
        {
            this.playerData = data;
        });

        yield return PlayFabUtils.LoadData<DailyCheckIn>(P_DAILY_CHECK_IN,
        data =>
        {
            if (data == null || string.IsNullOrEmpty(data.nextClaimTime))
            {
                data.SetNextClaimTime(DailyRewardManager.GetNextClaimTime());
                PlayFabUtils.Save<DailyCheckIn>(P_DAILY_CHECK_IN, data);
            }

            this.dailyReward = data;
        }, null, false);
    }

    [NaughtyAttributes.Button]
    private void Test_Push_Leaderboard_Statistic()
    {
        PlayFabUtils.SetUserStatistic(L_HIGHSCORE, 100);
    }
}
