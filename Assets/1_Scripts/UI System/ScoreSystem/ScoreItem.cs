using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private bool isIncrement = true;

    [NaughtyAttributes.Button]
    public void UpdateScore()
    {
        if (isIncrement)
        {
            ScoreSystem.Instance.IncrementScore(score);
        }
 
        else
        {
            ScoreSystem.Instance.DecrementScore(score);

        }

    }
}
