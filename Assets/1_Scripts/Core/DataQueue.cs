using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataQueue : MonoBehaviour
{
    private Queue<IEnumerator> routines = new();

    private static event Action<IEnumerator> OnQueue;

    private void Awake()
    {
        StartCoroutine(UpdateData());
    }

    private void OnEnable()
    {
        OnQueue += QueueRoutine;
    }

    private void OnDisable()
    {
        OnQueue -= QueueRoutine;
    }

    public static void Queue(IEnumerator routine)
    {
        OnQueue?.Invoke(routine);
    }

    private void QueueRoutine(IEnumerator routine)
    {
        routines.Enqueue(routine);
    }

    private IEnumerator UpdateData()
    {
        while (true)
        {
            if (routines.Count > 0)
            {
                yield return routines.Dequeue();
            }

            yield return null;
        }
    }
}
