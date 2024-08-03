using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _enemyPrefab;

    //Spawn delay
    [SerializeField] private float _nextSpawnTimer = 2.5f;
    private int _squashedEnemies; 

    //maximum enemies
    [SerializeField] private int _spawnedEnemies = 0;
    [SerializeField] private int _maximumSpawnCap = 5;

    public NavMeshTriangulation _triangulation;

    private void Start()
    {
        _triangulation = NavMesh.CalculateTriangulation();

        StartCoroutine(OnHandle_Spawner(_maximumSpawnCap));
    }

    private IEnumerator OnHandle_Spawner(int _spawnAmount)
    {
        WaitForSeconds _wait = new WaitForSeconds(_nextSpawnTimer);

        while (_spawnedEnemies < _spawnAmount)
        {
            Execute_SpawnEnemies();
            _spawnedEnemies++;

            yield return _wait;
        }

        _squashedEnemies = 0;
    }

    private void Execute_SpawnEnemies()
    {
        int _vertixIndex = Random.Range(0, _triangulation.vertices.Length);
        int _selectEnemies = Random.Range(0, _enemyPrefab.Length);

        UnityEngine.AI.NavMeshHit _hit;

        Debug.Log("Spawning: " + _triangulation);
        if ( UnityEngine.AI.NavMesh.SamplePosition(_triangulation.vertices[_vertixIndex], out _hit, 2.0f, -1) )
        {
            Debug.Log("Found");
            GameObject _newbie = Instantiate(_enemyPrefab[_selectEnemies], _hit.position, Quaternion.identity);
            CharacterManager_AI _enemiesManager = _newbie.GetComponent<CharacterManager_AI>();
            CharacterMovement_AI _enemiesMovement = _newbie.GetComponent<CharacterMovement_AI>();

            _enemiesManager.OnDead += OnCountDeath;

            _enemiesMovement._getterAgent.Warp(_hit.position);
            _enemiesMovement._getterAgent.enabled = true;
        }
    }

    private void OnCountDeath()
    {
        _squashedEnemies += 1;

        if (_squashedEnemies > 1)
            StartCoroutine(RespawnEnemies());
    }

    private IEnumerator RespawnEnemies()
    {
        yield return new WaitForSeconds (2.5f);

        StartCoroutine(OnHandle_Spawner(_squashedEnemies));
    }
}
