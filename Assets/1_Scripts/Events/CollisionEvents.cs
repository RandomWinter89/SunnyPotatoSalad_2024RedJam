using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvents : MonoBehaviour
{
    [SerializeField] private List<string> tags;

    [SerializeField] private bool triggerOnce = false;
    private bool isTriggered = false;

    [SerializeField] private UnityEvent onCollisionEnter;
    [SerializeField] private UnityEvent onCollisionStay;
    [SerializeField] private UnityEvent onCollisionExit;

    private bool CanTrigger
    {
        get
        {
            if (triggerOnce)
            {
                if (isTriggered) return false;
                else return true;
            }
            else
            {
                return true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsValidTag(collision.collider.tag)) return;
        if (!CanTrigger) return;

        if (onCollisionEnter != null) onCollisionEnter.Invoke();
        isTriggered = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!IsValidTag(collision.collider.tag)) return;
        if (!CanTrigger) return;

        if (onCollisionStay != null) onCollisionStay.Invoke();
        isTriggered = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!IsValidTag(collision.collider.tag)) return;

        if (!CanTrigger) return;


        if (onCollisionExit != null) onCollisionExit.Invoke();
        isTriggered = true;
    }

    private bool IsValidTag(string tag)
    {
        return tags.Contains(tag);
    }
}
