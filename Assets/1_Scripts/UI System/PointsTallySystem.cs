using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PointsTallySystem : MonoBehaviour
{
    [SerializeField] private ScoreSystem scoreSystem;

    [Header("UI")]
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private TMP_Text personalBestText;
    [SerializeField] private TMP_Text airAsiaPointText;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    private const float airAsiaPointMultiplier = 1.5f;


    private void OnDisable()
    {
        StopAllCoroutines();

        restartButton.onClick.RemoveListener(Restart);
        exitButton.onClick.RemoveListener(Exit);
    }

    private void OnEnable()
    {
        restartButton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(Exit);


        SetHighscore(scoreSystem.Highscore);
        SetPersonalBest(scoreSystem.PersonalBestScore);

        int airAsiaPoint = Mathf.RoundToInt(scoreSystem.Highscore * airAsiaPointMultiplier);
        StartCoroutine(AirAsiaPointAnimation(airAsiaPoint));

        if (!GameManager.IsDevMode)
        {
            PlayerData playerData = DataManager.main.playerData;

            playerData.SetHighscore(scoreSystem.Highscore);
            playerData.AddAirAsiaPoint(airAsiaPoint);
            playerData.AddTicket(GameManager.CollectedTicketCount);

            UpdatePlayFab();
        }
    }


    private void SetHighscore(int highscore)
    {
        highscoreText.text = highscore.ToString();
    }

    private void SetPersonalBest(int personalBest)
    {
        personalBestText.text = personalBest.ToString();
    }

    private void SetAirAsiaPointText(int airAsiaPoint)
    {
        airAsiaPointText.text = airAsiaPoint.ToString();
    }

    private IEnumerator AirAsiaPointAnimation(int totalAirAsiaPoint, System.Action onComplete = null)
    {
        float elapsedTime = 0;
        float duration = 1.5f;

        int point = 0;
        SetAirAsiaPointText(point);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            point = Mathf.RoundToInt(Mathf.Lerp(0, totalAirAsiaPoint, t));
            SetAirAsiaPointText(point);

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        SetAirAsiaPointText(totalAirAsiaPoint);
        onComplete?.Invoke();
    }

    private void UpdatePlayFab()
    {
        PlayFabUtils.SetUserStatistic(PlayFabKeys.L_HIGHSCORE, scoreSystem.Highscore);
        PlayFabUtils.Save<PlayerData>(PlayFabKeys.P_PLAYER_DATA, DataManager.main.playerData);
    }

    private void Restart()
    {
        SceneLoader.instance.Load(Scene.Gameplay);
    }

    private void Exit()
    {
        SceneLoader.instance.Load(Scene.Menu);
    }
}
