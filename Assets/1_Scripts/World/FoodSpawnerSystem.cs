using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnerSystem : MonoBehaviour
{
    [SerializeField] private List<CharacterGrowthItem> positiveFoodPrefabs;
    [SerializeField] private List<CharacterGrowthItem> negativeFoodPrefabs;
    [SerializeField] private CharacterManager player;

    private const int MAX_ACTIVE_FOOD_COUNT = 3;
    private const float NEGATIVE_FOOD_PROBABILITY = .3f;

    private const float SPAWN_INTERVAL = 5f;
    private float _nextSpawnTime = 0f;

 
    private void Awake()
    {
        foreach(CharacterGrowthItem food in positiveFoodPrefabs)
        {
            food.gameObject.SetActive(false);
        }

        foreach (CharacterGrowthItem food in negativeFoodPrefabs)
        {
            food.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (TooManyActiveFoods()) return;

        if (_nextSpawnTime > 0)
        {
            _nextSpawnTime -= Time.fixedDeltaTime;
            return;
        }

        SpawnFood();
        _nextSpawnTime = SPAWN_INTERVAL;
    }

    private void SpawnFood()
    {
        // Determine if we should spawn a negative food item
        bool spawnNegativeFood = Random.value < NEGATIVE_FOOD_PROBABILITY;

        // Choose the appropriate list based on the spawn type
        List<CharacterGrowthItem> availableFoods = spawnNegativeFood ? negativeFoodPrefabs : positiveFoodPrefabs;

        // Find an inactive food item to activate
        CharacterGrowthItem foodToActivate = availableFoods.Find(food => !food.gameObject.activeInHierarchy);

        if (foodToActivate != null)
        {
            // Activate the food item and add it to the list of active foods
            foodToActivate.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No inactive food items available to spawn.");
        }
    }

    private bool TooManyActiveFoods()
    {
        int activeFoodCount = 0;

        foreach(CharacterGrowthItem food in positiveFoodPrefabs)
        {
            if (!food.gameObject.activeInHierarchy) continue;
            else activeFoodCount++;
        }

        foreach (CharacterGrowthItem food in negativeFoodPrefabs)
        {
            if (!food.gameObject.activeInHierarchy) continue;
            else activeFoodCount++;
        }

        if (activeFoodCount >= MAX_ACTIVE_FOOD_COUNT) return true;
        return false;
    }
}
