using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnerSystem : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private List<CharacterGrowthItem> positiveFoodPrefabs;
    [SerializeField] private List<CharacterGrowthItem> negativeFoodPrefabs;
    [SerializeField] private CharacterManager player;
    [SerializeField] private Transform spawnPointsContainer;
    private List<Transform> spawnPoints = new();

    private const int MAX_ACTIVE_FOOD_COUNT = 3;
    private const float NEGATIVE_FOOD_PROBABILITY = .3f;

    private const float SPAWN_INTERVAL = 5f;
    private float _nextSpawnTime = 0f;

    private Dictionary<CharacterGrowthItem, Pool> poolDict = new();

    private static Dictionary<CharacterGrowthItem, Transform> occupiedPointsDict = new();

    private Pool CreatePool(CharacterGrowthItem food)
    {
        Pool pool = new Pool();

        for(int i = 0; i < pool.size; i++)
        {
            CharacterGrowthItem created = Instantiate(food, container);
            created.gameObject.SetActive(false);
            pool.items.Add(created);
        }

        return pool;
    }


    private void Awake()
    {
        spawnPoints.Clear();
        for(int i =0; i < spawnPointsContainer.childCount; i++)
        {
            spawnPoints.Add(spawnPointsContainer.GetChild(i));
        }

        spawnPointsContainer.gameObject.SetActive(false);


        occupiedPointsDict.Clear();
        CharacterGrowthItem.OnCollectedAction += OnFoodConsumed;

        foreach(CharacterGrowthItem food in positiveFoodPrefabs)
        {
            poolDict.Add(food, CreatePool(food));
        }

        foreach (CharacterGrowthItem food in negativeFoodPrefabs)
        {
            poolDict.Add(food, CreatePool(food));
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

        CharacterGrowthItem targetFoodItem;
        if (spawnNegativeFood)
        {
            targetFoodItem = GetRandomFoodItemFromList(negativeFoodPrefabs);

        }
        else
        {
            targetFoodItem = GetRandomFoodItemFromList(positiveFoodPrefabs);
        }

        CharacterGrowthItem spawned = poolDict[targetFoodItem].GetAvailable();
        spawned.gameObject.SetActive(true);

        Transform spawnPoint = GetRandomPointFromList(spawnPoints);
        spawned.transform.position = spawnPoint.position;

        occupiedPointsDict.Add(spawned, spawnPoint);


    }


    public static void OnFoodConsumed(CharacterGrowthItem food)
    {
        occupiedPointsDict.Remove(food);
    }

    private bool TooManyActiveFoods()
    {
        int activeFoodCount = 0;

        for(int i =0; i < container.childCount; i++)
        {
            if (!container.GetChild(i).gameObject.activeInHierarchy) continue;
            activeFoodCount++;
        }

        if (activeFoodCount >= MAX_ACTIVE_FOOD_COUNT) return true;
        return false;
    }

    private CharacterGrowthItem GetRandomFoodItemFromList(List<CharacterGrowthItem> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }

    private Transform GetRandomPointFromList(List<Transform> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }
}

public class Pool
{
    public List<CharacterGrowthItem> items =new();
    public int size = 10;

    public CharacterGrowthItem GetAvailable()
    {
        return items.Find((item) => !item.gameObject.activeInHierarchy);
    }
}
