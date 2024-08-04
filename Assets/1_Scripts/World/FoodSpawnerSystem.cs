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

    [Header("Ticket")]
    [SerializeField] private TicketItem ticket;

    private const int MAX_ACTIVE_FOOD_COUNT = 10;
    private const float PROBABILITY_NEGATIVE_FOOD = .3f;
    private const float PROBABILITY_TICKET = .1f;

    private const float SPAWN_INTERVAL = 5f;
    private float _nextSpawnTime = 0f;

    private Pool<TicketItem> ticketPool = new();
    private Dictionary<CharacterGrowthItem, Pool<CharacterGrowthItem>> poolDict = new();

    private static Dictionary<CharacterGrowthItem, Transform> occupiedPointsDict = new();

    #region Pool

    private Pool<CharacterGrowthItem> CreatePool(CharacterGrowthItem food)
    {
        Pool<CharacterGrowthItem> pool = new Pool<CharacterGrowthItem>();

        for(int i = 0; i < pool.size; i++)
        {
            CharacterGrowthItem created = Instantiate(food, container);
            created.gameObject.SetActive(false);
            pool.items.Add(created);
        }

        return pool;
    }

    private Pool<TicketItem> CreatePool(TicketItem ticket)
    {
        Pool<TicketItem> pool = new Pool<TicketItem>();

        for (int i = 0; i < pool.size; i++)
        {
            TicketItem created = Instantiate(ticket, container);
            created.gameObject.SetActive(false);
            pool.items.Add(created);
        }

        return pool;
    }
    #endregion

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

        ticketPool = CreatePool(ticket);
    }

    private void FixedUpdate()
    {
        if (TooManyActiveFoods()) return;

        if (_nextSpawnTime > 0)
        {
            _nextSpawnTime -= Time.fixedDeltaTime;
            return;
        }

        bool spawnTicket = Random.value < PROBABILITY_TICKET;
        if (spawnTicket)
            SpawnTicket();
        else
            SpawnFood();

        _nextSpawnTime = SPAWN_INTERVAL;
    }

    private void SpawnFood()
    {
        // Determine if we should spawn a negative food item
        bool spawnNegativeFood = Random.value < PROBABILITY_NEGATIVE_FOOD;

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

    private void SpawnTicket()
    {
        TicketItem ticketItem = ticketPool.GetAvailable();

        Transform spawnPoint = GetRandomPointFromList(spawnPoints);
        ticketItem.transform.position = spawnPoint.position;

        ticketItem.gameObject.SetActive(true);
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


public class Pool<T> where T : MonoBehaviour
{
    public List<T> items = new();
    public int size = 10;

    public T GetAvailable()
    {
        return items.Find((item) => !item.gameObject.activeInHierarchy);
    }
}