using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollidedWithBam : MonoBehaviour
{
    [SerializeField] private CharacterManager_AI characterManager;
    private CharacterGrowth characterGrowth;

    private const int GROWTH_STAGE_ROLL = 1;

    private void Start()
    {
        characterGrowth = CharacterManager.Instance.CharacterGrowth;
    }


    private void TrySquashEnemy(CharacterManager_AI enemy)
    {
        if (characterGrowth.GrowthStage < GROWTH_STAGE_ROLL)
        {
            Debug.Log("cannot squash");
            return;
        }

        Debug.Log("squash");
        characterGrowth.DecreaseGrowth(.4f);
        enemy.Die();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TrySquashEnemy(characterManager);
        }
    }
}
