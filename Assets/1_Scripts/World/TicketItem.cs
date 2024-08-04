using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketItem : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private float disappearDelay = 10f;
    [SerializeField] private ScoreItem scoreItem;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        StartCoroutine(DisappearRoutine());
    }

    public void GainTicket()
    {
        GameManager.AddCollectedTicket(amount);
        scoreItem.UpdateScore();
        gameObject.SetActive(false);
    }

    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(disappearDelay);
        gameObject.SetActive(false);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            GainTicket();
        }
    }
}
