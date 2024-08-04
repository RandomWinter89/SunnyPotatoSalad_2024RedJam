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
    public ReferralCode referral = new();

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
        PlayerData.OnPlayerInfoUpdated += UpdatePlayerData;
    }

    private void OnDisable()
    {
        PlayerData.OnPlayerInfoUpdated -= UpdatePlayerData;
    }

    private void UpdatePlayerData()
    {
        //DataQueue.Queue(PlayFabUtils.Save<PlayerData>(P_PLAYER_DATA, playerData));
        PlayFabUtils.Save<PlayerData>(P_PLAYER_DATA, playerData);
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

        yield return MongoUtils.GetData<ReferralCode>("Referral", playFabID,
        data =>
        {
            if (data == null || string.IsNullOrEmpty(data.code))
            {
                data = new ReferralCode()
                {
                    _id = playFabID,
                    code = PromoCodeGenerator.GeneratePromoCode(playFabID),
                    count = 0
                };

                StartCoroutine(MongoUtils.InsertData("Referral", data));
            }

            referral = data;
        });
    }


    [NaughtyAttributes.Button]
    private void test_insert()
    {
        var data = new ReferralCode()
        {
            _id = DataManager.main.playFabID,
            code = PromoCodeGenerator.GeneratePromoCode(DataManager.main.playFabID),
            count = 0
        };

        StartCoroutine(MongoUtils.InsertData("Referral", data));
    }

    [NaughtyAttributes.Button]
    private void test_update()
    {
        var data = new ReferralCode()
        {
            _id = DataManager.main.playFabID,
            code = PromoCodeGenerator.GeneratePromoCode(DataManager.main.playFabID),
            count = 0
        };

        StartCoroutine(MongoUtils.UpdateData("Referral", data._id, data));
    }

}
