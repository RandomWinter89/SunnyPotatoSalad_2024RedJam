using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayFabKeys;

public class DataManager : MonoBehaviour
{
    public PlayerData playerData;
    public DailyCheckIn dailyReward;

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

    private void OnEnable()
    {
        LoginManager.OnLogin += InitPlayerData;
    }

    private void OnDisable()
    {
        LoginManager.OnLogin += InitPlayerData;
    }

    private void InitPlayerData()
    {
        StartCoroutine(LoadPlayerDataRoutine());

        IEnumerator LoadPlayerDataRoutine()
        {
            yield return PlayFabUtils.LoadData<PlayerData>(P_PLAYER_DATA,
            data =>
            {
                this.playerData = data;
            });

            yield return PlayFabUtils.LoadData<DailyCheckIn>(P_DAILY_CHECK_IN,
            data =>
            {
                this.dailyReward = data;
            });
        }
    }
}
