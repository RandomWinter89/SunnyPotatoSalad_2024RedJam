using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private RectTransform rect;

    private static event Action OnStart;
    private static event Action OnStop;

    public static void Load() => OnStart?.Invoke();
    public static void Stop() => OnStop?.Invoke();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnEnable()
    {
        OnStart += StartLoading;
        OnStop += StopLoading;
    }

    private void OnDisable()
    {
        OnStart -= StartLoading;
        OnStop -= StopLoading;
    }

    [Button]
    private void StartLoading()
    {
        StartCoroutine(RotateVisualRoutine(rect));
        loadingPanel.gameObject.SetActive(true);
    }

    [Button]
    private void StopLoading()
    {
        StopAllCoroutines();
        loadingPanel.gameObject.SetActive(false);
    }

    private IEnumerator RotateVisualRoutine(RectTransform rectTransform)
    {
        while (true)
        {
            float timer = 0f;
            float totalTime = 1f;
            float initialSpeed = 2500;

            while (timer < totalTime)
            {
                timer += Time.deltaTime;
                float speed = Mathf.Lerp(initialSpeed, 0f, timer / totalTime);
                rectTransform.Rotate(0f, 0f, speed * Time.unscaledDeltaTime);

                yield return null;
            }

            yield return null;
        }
    }

}
