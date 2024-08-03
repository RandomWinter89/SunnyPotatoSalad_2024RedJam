using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Event
{
    OnAwake,
    OnStart,
    OnEnable,
    OnDisable
}

public class UnityEvents : MonoBehaviour
{
    [SerializeField] private Event eventType;
    [SerializeField] private UnityEvent callback;

    private void Awake()
    {
        if (eventType != Event.OnAwake) return;
        callback.Invoke();
    }

    private void Start()
    {
        if (eventType != Event.OnStart) return;
        callback.Invoke();

    }

    private void OnEnable()
    {
        if (eventType != Event.OnEnable) return;
        callback.Invoke();
    }

    private void OnDisable()
    {
        if (eventType != Event.OnDisable) return;
        callback.Invoke();
    }
}
