using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSystem : MonoBehaviour
{
    [SerializeField] private float _maximumSecond;
    [SerializeField] private float _currentSecond;
    [SerializeField] private TMP_Text _TBTimer; 

    public bool _hasEnded;
    
    private void Start()
    {
        _currentSecond = _maximumSecond;
    }

    private void FixedUpdate()
    {
        if (!_hasEnded)
            DecrementTime();
    }

    private void DecrementTime()
    {
        if (_maximumSecond < 0)
        {
            _hasEnded = true;
            return;
        }

        _maximumSecond -= Time.deltaTime;
        UpdateTimerVisual(_maximumSecond);
    }

    private void UpdateTimerVisual(float _time)
    {
        _TBTimer.text = _time.ToString("F0");
    }

}
