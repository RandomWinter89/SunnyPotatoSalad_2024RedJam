using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private Button pauseButton;

    public static System.Action<bool> OnPausedStateChanged;

    public bool IsPaused { get; set; } = false;


    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(OnResumeClicked);
        exitButton.onClick.RemoveListener(OnExitClicked);

        pauseButton.onClick.RemoveListener(Pause);

        OnPausedStateChanged = null;
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(OnResumeClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        pauseButton.onClick.AddListener(Pause);

    }

    private void Pause()
    {
        Time.timeScale = 0f;
        panel.gameObject.SetActive(true);

        IsPaused = true;
        OnPausedStateChanged?.Invoke(IsPaused);
    }

    private void UnPause()
    {
        Time.timeScale = 1f;
        panel.gameObject.SetActive(false);

        IsPaused = false;
        OnPausedStateChanged?.Invoke(IsPaused);
    }

    private void OnResumeClicked()
    {
        UnPause();
    }

    private void OnExitClicked()
    {
        // implement scene loader function
    }

}
