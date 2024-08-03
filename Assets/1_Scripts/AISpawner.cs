using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _enemyPrefab;
    private GameObject[] _enemyPool;
    private bool _hasfullySpawned;

    //Spawn delay
    [SerializeField] private float _nextSpawnTimer = 5.25f;
    private int _squashedEnemies; 

    //maximum enemies
    [SerializeField] private int _spawnedEnemies = 0;
    [SerializeField] private int _maximumSpawnCap = 5;

    public NavMeshTriangulation _triangulation;

    private void Awake()
    {
        _enemyPool = new GameObject[_maximumSpawnCap];
    }

    private void Start()
    {
        _triangulation = NavMesh.CalculateTriangulation();
        StartCoroutine(OnHandle_Spawner(_maximumSpawnCap));
    }

    private IEnumerator OnHandle_Spawner(int _spawnAmount)
    {
        _hasfullySpawned = false;
        WaitForSeconds _wait = new WaitForSeconds(_nextSpawnTimer);

        while (_spawnedEnemies < _spawnAmount)
        {
            Execute_SpawnEnemies();
            _spawnedEnemies++;

            yield return _wait;
        }

        _squashedEnemies = 0;
        _hasfullySpawned = true;
    }

    private void Execute_SpawnEnemies()
    {
        int _vertixIndex = Random.Range(0, _triangulation.vertices.Length);
        int _selectEnemies = Random.Range(0, _enemyPrefab.Length);

        UnityEngine.AI.NavMeshHit _hit;

        if ( UnityEngine.AI.NavMesh.SamplePosition(_triangulation.vertices[_vertixIndex], out _hit, 2.0f, -1) )
        {
            _enemyPool[_spawnedEnemies] = Instantiate(_enemyPrefab[_selectEnemies], _hit.position, Quaternion.identity);
            CharacterManager_AI _enemiesManager = _enemyPool[_spawnedEnemies].GetComponent<CharacterManager_AI>();
            CharacterMovement_AI _enemiesMovement = _enemyPool[_spawnedEnemies].GetComponent<CharacterMovement_AI>();
            
            _enemiesManager.OnDead += OnCountDeath;

            _enemiesMovement._getterAgent.Warp(_hit.position);
            _enemiesMovement._getterAgent.enabled = true;
        }
    }

    private void OnCountDeath()
    {
        _squashedEnemies++;
        StartCoroutine(RespawnEnemies());
    }

    private IEnumerator RespawnEnemies()
    {
        yield return new WaitForSeconds (15f);

        foreach(GameObject _enemy in _enemyPool)
        {
            if (_enemy.activeInHierarchy)
                continue;

            _enemy.SetActive(true);
            _squashedEnemies--;
        }
    }

}
