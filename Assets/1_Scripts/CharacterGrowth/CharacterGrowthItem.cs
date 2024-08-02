using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGrowthItem : MonoBehaviour
{
    [SerializeField] private float growthAmount = .1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected(other.GetComponent<CharacterGrowth>());
        }
    }

    private void OnCollected(CharacterGrowth characterGrowth)
    {
        characterGrowth.IncreaseGrowth(growthAmount);
    }
}
