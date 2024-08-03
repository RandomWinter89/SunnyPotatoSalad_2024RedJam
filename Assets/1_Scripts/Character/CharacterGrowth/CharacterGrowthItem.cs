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
    private Coroutine resetCooldownRoutine = null;


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
        gameObject.SetActive(false);
    }

    #region Drop
    public void Drop(Vector3 playerPos)
    {
        gameObject.transform.position = playerPos;
        gameObject.SetActive(true);

        canBeCollected = false;
        if (resetCooldownRoutine != null) StopCoroutine(resetCooldownRoutine);
        resetCooldownRoutine = StartCoroutine(ResetCooldown());

        Vector3 dir = new Vector3(GetRandomNormalizedValue(), 1, GetRandomNormalizedValue());
        rb.AddForce(dir * GetRandomForce(dropForceMinMax.x, dropForceMinMax.y), ForceMode.Impulse);
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(canBeCollectedCooldown);
        canBeCollected = true;
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
