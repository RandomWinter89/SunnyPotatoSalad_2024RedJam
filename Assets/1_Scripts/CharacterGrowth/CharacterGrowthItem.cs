using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGrowthItem : MonoBehaviour
{
    [SerializeField] private float growthAmount = .1f;
    public float GrowthAmount => growthAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterGrowth characterGrowth = other.transform.parent.GetComponent<CharacterGrowth>();
            OnCollected(characterGrowth);
        }
    }

    private void OnCollected(CharacterGrowth characterGrowth)
    {
        characterGrowth.IncreaseGrowth(this);
        gameObject.SetActive(false);
    }

    public void SetPositionAndEnable(Vector3 position)
    {
        gameObject.transform.position = position;
        gameObject.SetActive(true);
    }
}
