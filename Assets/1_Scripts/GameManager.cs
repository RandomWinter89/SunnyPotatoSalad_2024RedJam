using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private PointsTallySystem pointsTallySystem;

    // handle pause menu
    // handle player hud

    public static System.Action EndGame;

    private void OnDestroy()
    {
        EndGame -= OnGameEnded;
    }

    private void Awake()
    {
        pointsTallySystem.gameObject.SetActive(false);
        EndGame += OnGameEnded;
    }

    public void OnGameEnded()
    {
        pointsTallySystem.gameObject.SetActive(true);
        timeSystem.gameObject.SetActive(false);
        scoreSystem.gameObject.SetActive(false);
    }

  
}
