using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayerMainUI : MonoBehaviour
{
    [SerializeField] TMP_Text highscoreText;
    [SerializeField] TMP_Text airAsiaPointsText;
    [SerializeField] TMP_Text ticketText;

    [SerializeField] Button startGameBtn;
    [SerializeField] Button logoutBtn;

    private void Awake()
    {
        AudioManager.instance.OnAction_MusicAudio("MainMenu", true);

        startGameBtn.onClick.AddListener(StartGame);
        logoutBtn.onClick.AddListener(Logout);

        RepaintUI();
    }

    private void OnEnable()
    {
        PlayerData.OnPlayerInfoUpdated += RepaintUI;
    }

    private void OnDisable()
    {
        PlayerData.OnPlayerInfoUpdated -= RepaintUI;
    }

    private void RepaintUI()
    {
        var data = DataManager.main;
        highscoreText.SetText($"{data.playerData.Highscore}");
        airAsiaPointsText.SetText(data.playerData.Currency.airAsiaPoint.ToString());
        ticketText.SetText($"{data.playerData.Currency.ticketCount}");
    }
    
    private void StartGame()
    {
        var data = DataManager.main;

        if (data.playerData.Currency.ticketCount <= 0)
        {
            PromptManager.Prompt("Ticket Finished", "You do not have any ticket left");
            return;
        }

        data.playerData.Currency.ticketCount--;
        SceneLoader.instance.Load(Scene.Gameplay);
    }

    private void Logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        LoginManager.ForgetAllCredentials();
        SceneLoader.instance.Load(Scene.Login);
    }
}
