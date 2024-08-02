using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGrowthDisruptor : MonoBehaviour
{
    [SerializeField] private float growthDecreaseAmount = .4f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<CharacterGrowth>().DecreaseGrowth(growthDecreaseAmount);
        }
    }
}
