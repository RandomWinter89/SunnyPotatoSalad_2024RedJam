using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvents : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    [SerializeField] private UnityEvent callback;

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    public void Invoke()
    {
        StopAllCoroutines();
        StartCoroutine(DelayedEvent());
    }

    private IEnumerator DelayedEvent()
    {
        yield return new WaitForSeconds(delay);
        callback.Invoke();
    }
}
