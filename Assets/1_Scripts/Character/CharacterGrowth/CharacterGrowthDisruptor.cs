using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGrowthDisruptor : MonoBehaviour
{
    [SerializeField] private float growthDecreaseAmount = .4f;
    [SerializeField] private bool dropsFood = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            CharacterGrowth characterGrowth = other.collider.transform.parent.GetComponent<CharacterGrowth>();
            characterGrowth.DecreaseGrowth(growthDecreaseAmount);

            if(dropsFood)
                characterGrowth.DropGrowthItems();
        }
    }
}
