using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RectTransform hud;

   // [SerializeField] private TimeSystem timeSystem;
  //  [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private PointsTallySystem pointsTallySystem;

    [SerializeField] private PauseMenu pauseMenu;





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

        PauseMenu.OnPausedStateChanged += OnPauseStateChanged;

    
    }

    public void OnGameEnded()
    {
        pointsTallySystem.gameObject.SetActive(true);

        pauseMenu.gameObject.SetActive(false);
    }

    private void OnPauseStateChanged(bool paused)
    {
        if (paused)
        {
            hud.gameObject.SetActive(false);
        }
        else
        {
            hud.gameObject.SetActive(true);
        }
    }
}
