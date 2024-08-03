using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;

    //Spawn delay
    [SerializeField] private float _nextSpawnTimer = 2.5f;
    private int _squashedEnemies; 

    //maximum enemies
    [SerializeField] private int _spawnedEnemies = 0;
    [SerializeField] private int _maximumSpawnCap = 5;

    private void Start()
    {
        StartCoroutine(OnHandle_Spawner());
    }

    private IEnumerator OnHandle_Spawner()
    {
        WaitForSeconds _wait = new WaitForSeconds(_nextSpawnTimer);

        while (_spawnedEnemies < _maximumSpawnCap)
        {
            Execute_SpawnEnemies();
            _spawnedEnemies++;

            yield return _wait;
        }
        
    }

    private void Execute_SpawnEnemies()
    {
        // = _enemyPrefab.GetComponent<>();

        UnityEngine.AI.NavMeshTriangulation _triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
        int _vertixIndex = Random.Range(0, _triangulation.vertices.Length);

        UnityEngine.AI.NavMeshHit _hit;
        if ( UnityEngine.AI.NavMesh.SamplePosition(_triangulation.vertices[_vertixIndex], out _hit, 2.0f, 0) )
        {
            GameObject _newbie = Instantiate(_enemyPrefab, _hit.position, Quaternion.identity);
            //.Agent.Warp(_hit.position);
            //.Agent.enabled = true;
        }
    }
}
