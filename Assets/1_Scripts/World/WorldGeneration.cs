using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// instantiate, save configuration
public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private bool generateOnStart = false;
    [SerializeField] private bool clearPreviousObstacles = true;
    [SerializeField] private Transform container;

    [Space]
    [SerializeField] private WorldObject treeObstaclesData;
    [SerializeField] private WorldObject rockObstaclesData;





    // already has an object here
    private List<Transform> occupiedPoints = new();


    public List<GameObject> GetAllWorldObjects()
    {
        List<GameObject> objs = new();

        for (int i = 0; i < container.childCount; i++)
        {
            objs.Add(container.GetChild(i).gameObject);
        }
        return objs;
    }


    private void Start()
    {
        if (generateOnStart)
        {
            Generate();
        }
    }


    [NaughtyAttributes.Button]
    private void Generate()
    {
        if(clearPreviousObstacles)
            Clear();


        treeObstaclesData.TryGetConfiguration();
        rockObstaclesData.TryGetConfiguration();

        Spawn(treeObstaclesData);
        Spawn(rockObstaclesData);

        HideSpawnPoints();
    }

    [NaughtyAttributes.Button]
    private void Clear()
    {
        // clear objects
        for (int i = 0; i < container.childCount; i++)
        {
            DestroyImmediate(container.GetChild(i).gameObject);
        }

        occupiedPoints.Clear();
    }

    [NaughtyAttributes.Button]
    private void ShowSpawnPoints()
    {
        treeObstaclesData.ShowSpawnPoints(true);
        rockObstaclesData.ShowSpawnPoints(true);
    }

    [NaughtyAttributes.Button]
    private void HideSpawnPoints()
    {
        treeObstaclesData.ShowSpawnPoints(false);
        rockObstaclesData.ShowSpawnPoints(false);
    }




    private void Spawn(WorldObject obstacleData)
    {
        foreach(KeyValuePair<Transform, GameObject> kvp in obstacleData.confirmedObstacleSpawns)
        {
            if (occupiedPoints.Contains(kvp.Key)) continue; // do not spawn if already occupied
            Instantiate(kvp.Value, kvp.Key.position, kvp.Value.transform.rotation, container);
            occupiedPoints.Add(kvp.Key);
        }
    }
}

[System.Serializable]
public class WorldObject
{
    public List<GameObject> objectPrefabs;
    public Transform spawnPointsContainer;
    [HideInInspector] public List<Transform> potentialSpawnPoints;
    [Range(0f, 1f)] public float likelihood;

    [HideInInspector] public List<KeyValuePair<Transform, GameObject>> confirmedObstacleSpawns = new();


    public void TryGetConfiguration()
    {
        confirmedObstacleSpawns.Clear();
        potentialSpawnPoints.Clear();
        for (int i = 0; i < spawnPointsContainer.childCount; i++)
        {
            potentialSpawnPoints.Add(spawnPointsContainer.GetChild(i));
        }

        foreach (Transform point in potentialSpawnPoints)
        {
            float rng = Random.value;

            // failed roll
            if (rng > likelihood) continue;

            confirmedObstacleSpawns.Add(new KeyValuePair<Transform, GameObject>(point, GetRandomPrefabReference()));
        }
    }

    public GameObject GetRandomPrefabReference()
    {
        int index = Random.Range(0, objectPrefabs.Count);
        return objectPrefabs[index];
    }

    public void ShowSpawnPoints(bool status)
    {
        spawnPointsContainer.gameObject.SetActive(status);
    }
}
