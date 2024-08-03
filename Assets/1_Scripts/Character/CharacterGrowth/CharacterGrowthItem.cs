using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGrowthItem : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector2 dropForceMinMax = new Vector2(5, 10);

    [SerializeField] private float growthAmount = .1f;
    public float GrowthAmount => growthAmount;

    [SerializeField] private float canBeCollectedCooldown = 1f;
    [SerializeField, NaughtyAttributes.ReadOnly] private bool canBeCollected = true;
    private Coroutine resetCanBeCollectedCooldownRoutine = null;

    [SerializeField] private float disappearDelay = 5f;

    public static System.Action OnCollectedAction;


    private void OnDestroy()
    {
        OnCollectedAction = null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!canBeCollected) return;

            CharacterGrowth characterGrowth = other.transform.parent.GetComponent<CharacterGrowth>();
            OnCollected(characterGrowth);
        }
    }

    private void OnCollected(CharacterGrowth characterGrowth)
    {
        characterGrowth.IncreaseGrowth(this);
        ScoreSystem.Instance.IncrementScore(35);

        if (OnCollectedAction != null) OnCollectedAction.Invoke();

        gameObject.SetActive(false);
    }

    #region Drop
    public void Drop(Vector3 playerPos)
    {
        gameObject.transform.position = playerPos;
        gameObject.SetActive(true);

        canBeCollected = false;
        if (resetCanBeCollectedCooldownRoutine != null) StopCoroutine(resetCanBeCollectedCooldownRoutine);
        resetCanBeCollectedCooldownRoutine = StartCoroutine(ResetCanBeCollectedCooldown());

        Vector3 dir = new Vector3(GetRandomNormalizedValue(), 1, GetRandomNormalizedValue());
        rb.AddForce(dir * GetRandomForce(dropForceMinMax.x, dropForceMinMax.y), ForceMode.Impulse);

        StartCoroutine(DisappearRoutine());
    }

    private IEnumerator ResetCanBeCollectedCooldown()
    {
        yield return new WaitForSeconds(canBeCollectedCooldown);
        canBeCollected = true;
    }

    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(disappearDelay);

        gameObject.SetActive(false);
    }

    private float GetRandomNormalizedValue()
    {
        return Random.Range(-1f, 1f);
    }

    private float GetRandomForce(float min, float max)
    {
        return Random.Range(min, max);
    }
    #endregion
}
