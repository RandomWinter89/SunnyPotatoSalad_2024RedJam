using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGrowthDisruptor : MonoBehaviour
{
    [SerializeField] private float growthDecreaseAmount = .4f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            other.collider.transform.parent.GetComponent<CharacterGrowth>().DecreaseGrowth(growthDecreaseAmount);
        }
    }
}
