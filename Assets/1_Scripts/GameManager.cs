using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RectTransform hud;

    [SerializeField] private PointsTallySystem pointsTallySystem;

    [SerializeField] private PauseMenu pauseMenu;

    public static int CollectedTicketCount { get; private set; } = 0;

    public static bool IsDevMode = true;



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
        hud.gameObject.SetActive(false);

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

    public static void AddCollectedTicket(int amount)
    {
        CollectedTicketCount += amount;
    }
}
