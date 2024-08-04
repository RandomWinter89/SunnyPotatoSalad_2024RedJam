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

        OnStart += StartLoading;
        OnStop += StopLoading;
    }

    public void OnEnable()
    {

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
            float totalTime = .5f;
            float initialSpeed = 1000;

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
