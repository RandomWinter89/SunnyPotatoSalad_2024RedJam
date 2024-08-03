using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;

public static class PlayFabKeys
{
    public const string TITLE_ID = "BCDB6";

    public const string P_PLAYER_DATA = "P_PlayerData";
    public const string P_DAILY_CHECK_IN = "P_DailyCheckIn";

    public const string L_HIGHSCORE = "Highscore";
}

public static class PlayFabUtils
{
    public static Action<string, List<PlayerLeaderboardEntry>> OnLeaderboardRetrieved;

    public static IEnumerator Login(string email, string password, Action onLogin)
    {
        bool done = false;
        PlayFabErrorCode errorCode = PlayFabErrorCode.Success;

        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest()
        {
            Email = email,
            Password = password,
            TitleId = PlayFabKeys.TITLE_ID,
        },
        result =>
        {
            Debug.Log($"Login success");

            var data = DataManager.main;
            data.sessionTicket = result.SessionTicket;
            data.playFabID = result.PlayFabId;

            onLogin?.Invoke();
            done = true;
        },
        error =>
        {
            done = true;
            errorCode = error.Error;
            Debug.LogError(error.GenerateErrorReport());
        });
        
        while (!done) yield return null;

        if (errorCode == PlayFabErrorCode.AccountNotFound)
        {
            yield return RegisterUser(email, password);
            yield return Login(email, password, onLogin);
        }
    }

    public static IEnumerator RegisterUser(string email, string password)
    {
        bool done = false;

        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest()
        {
            Email = email,
            Password = password,
            TitleId = PlayFabKeys.TITLE_ID,
            RequireBothUsernameAndEmail = false,
        },
        result =>
        {
            done = true;
        },
        error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });

        while (!done) yield return null;
    }

    public static IEnumerator GetPlayerProfile(string playFabId, Action<PlayerProfileModel> callback)
    {
        bool done = false;

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result =>
        {
            callback?.Invoke(result.PlayerProfile);
            done = true;
        },
        error => Debug.LogError(error.GenerateErrorReport()));

        while (!done) yield return null;
    }

    public static void SetPlayerName(string name, Action onSuccess = null, Action<string> onError = null)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = name,
        },
        result =>
        {
            onSuccess?.Invoke();
            Debug.Log($"Succesfully set player display name {name} to PlayFab");
        },
        err =>
        {
            onError?.Invoke(err.ErrorMessage);
            Debug.Log($"<color=red>Failed set player display name {name} to PlayFab");
        });
    }

    public static void Save<T>(string key, object data, Action onSuccess = null, Action onError = null)
    {
        string json = JsonConvert.SerializeObject(data);

        var updateRequest = new UpdateUserDataRequest();
        updateRequest.Data = new Dictionary<string, string>();
        updateRequest.Data[key] = json;

        PlayFabClientAPI.UpdateUserData(updateRequest,
        result =>
        {
            Debug.Log($"Succesfully save {key} to PlayFab");
            onSuccess?.Invoke();
        },
        error =>
        {
            onError?.Invoke();
            Debug.LogError($"<color=red>Failed save {key} to PlayFab: {error.ErrorMessage}</color>");
        });
    }

    public static IEnumerator LoadData<T>(string key, Action<T> success, Action<string> error = null, bool autoSave = true) where T : new()
    {
        bool done = false;

        Load<T>(key,
        data =>
        {
            success?.Invoke(data);
            done = true;
        },
        errMsg =>
        {
            error?.Invoke(errMsg);
        });

        while (!done) yield return null;
    }

    public static void Load<T>(string key, Action<T> success, Action<string> error = null, bool autoSave = true) where T : new()
    {
        T data = new T();

        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            AuthenticationContext = new PlayFabAuthenticationContext()
            {
                PlayFabId = DataManager.main.playFabID,
                ClientSessionTicket = DataManager.main.sessionTicket
            }
        },
        result =>
        {
            if (result.Data != null && result.Data.ContainsKey(key))
            {
                Debug.Log($"Load {key} from PlayFab");
                string jsonData = result.Data[key].Value;
                data = JsonConvert.DeserializeObject<T>(jsonData);
                success.Invoke(data);
            }
            else
            {
                //Player config doesn't exist, create from default and update it to PlayFab server
                Debug.Log($"No key {key} found from PlayFab, create a default value data");
                success.Invoke(data);

                if (autoSave)
                    Save<T>(key, data);
            }
        },
        err =>
        {
            string errorMsg = $"<color=red>Failed load data from PlayFab: {key} - {err.ErrorMessage}</color>";
            Debug.LogError(errorMsg);
            error?.Invoke(errorMsg);
        });
    }

    public static void GetUserStatistic(string key, Action<int> result = null, Action error = null)
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string>() { key },
        },
        response =>
        {
            Debug.Log($"Get Statistic {key} from PlayFab {response.Statistics}");

            if (response.Statistics.Count > 0)
                result?.Invoke(response.Statistics[0].Value);
            else
                result?.Invoke(-1);
        },
        err =>
        {
            string errorMsg = $"<color=red>Failed load statistic from PlayFab: {err.ErrorMessage}</color>";
            Debug.LogError(errorMsg);
            error?.Invoke();
        });
    }

    public static void SetUserStatistic(string key, int value, Action success = null, Action error = null)
    {
        var updateRequest = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = key,
                    Value = value,
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(updateRequest,
        result =>
        {
            Debug.Log($"Update Statistic {key} to PlayFab");
            success?.Invoke();
        },
        err =>
        {
            error?.Invoke();
            Debug.LogError($"<color=red>Failed update statistic to PlayFab: {err.ErrorMessage}</color>");
        });
    }

    public static void GetLeaderboard(string key, int maxResult = 10, Action<List<PlayerLeaderboardEntry>> success = null, Action error = null)
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = key,
            MaxResultsCount = maxResult,
        },
        result =>
        {
            Debug.Log($"Get Leaderboard {key} from PlayFab {result.Leaderboard.Count} - {result.ToJson()}");
            OnLeaderboardRetrieved?.Invoke(key, result.Leaderboard);
            success?.Invoke(result.Leaderboard);
        },
        err =>
        {
            error?.Invoke();
            Debug.LogError($"<color=red>Failed retrieve leaderboard from PlayFab: {err.ErrorMessage}</color>");
        });
    }
}
