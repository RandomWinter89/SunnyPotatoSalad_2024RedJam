using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMainUI : MonoBehaviour
{
    [SerializeField] TMP_Text highscoreText;
    [SerializeField] TMP_Text airAsiaPointsText;
    [SerializeField] TMP_Text ticketText;

    private void Awake()
    {
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
        highscoreText.SetText($"Highscore\n{data.playerData.Highscore}");
        airAsiaPointsText.SetText(data.playerData.Currency.airAsiaPoint.ToString());
        ticketText.SetText($"Ticket\n{data.playerData.Currency.ticketCount}");
    }
}
