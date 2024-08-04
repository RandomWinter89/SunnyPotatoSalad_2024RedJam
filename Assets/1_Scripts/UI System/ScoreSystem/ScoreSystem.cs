using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private int _personalScore = 0;
    [SerializeField] private int _currentScore = 0; 
    [SerializeField] private int _multiplier = 1;
    [SerializeField] private int _buffDuration = 10;
    [SerializeField] private TMP_Text _currentScorePoint;
    [SerializeField] private TMP_Text _personalScorePoint;


    public static ScoreSystem Instance;

    public int PersonalBestScore => _personalScore;
    public int Highscore => _currentScore;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScorePoint();

        if (GameManager.IsDevMode)
        {
            _personalScore = 0;
        }
        else
        {
            _personalScore = DataManager.main.playerData.Highscore;
        }
    }
    
    public void IncrementScore(int _value)
    {
        _currentScore += (_value * _multiplier);
        _currentScore = Mathf.Clamp(_currentScore, 0, int.MaxValue);
        UpdateScorePoint();

        Vector3 offset = new Vector3(0, -80, 0);

        PopupNumber.Create(PopupType.AddScore, _currentScorePoint.transform.position + offset , _value, _currentScorePoint.transform);
        AudioManager.instance.OnSimpleAction_SFXAudio("AddScore");
    }

    public void DecrementScore(int _value)
    {
        _currentScore -= _value;
        _currentScore = Mathf.Clamp(_currentScore, 0, int.MaxValue);
        UpdateScorePoint();

        Vector3 offset = new Vector3(0, -80, 0);

        PopupNumber.Create(PopupType.MinusScore, _currentScorePoint.transform.position + offset, _value, _currentScorePoint.transform);
        AudioManager.instance.OnSimpleAction_SFXAudio("MinusScore");
    }

    private void UpdateScorePoint()
    {
        _currentScorePoint.text = _currentScore.ToString();

        if (_currentScore > _personalScore)
        {
            _personalScore = _currentScore;
            _personalScorePoint.text = _personalScore.ToString(); 
        }
    }

    public void MultiplierOn(int _value)
    {
        _multiplier = _value;
        StartCoroutine(MultiplierDuration());
    }

    private IEnumerator MultiplierDuration()
    {
        yield return new WaitForSeconds (_buffDuration);
        _multiplier = 1;
    }
}
