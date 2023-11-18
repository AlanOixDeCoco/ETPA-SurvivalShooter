using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyBasePrefab;
    [SerializeField] private EnemyStatsSO[] _enemiesBaseStats;

    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private Transform _newEnemyHolder;

    // Number of enemies stored per wave
    private int _spawnerSize = 10;
    // Enemies stored
    private List<GameObject> _enemies;

    private GameObject _newEnemy;

    private void Start()
    {
        FillSpawner();

        StartCoroutine(SpawnEvery5sec());
    }

    private void Update()
    {
        if (IsSpawnerEmpty)
        {
            Debug.Log("Spawned every enemy!");
            StopAllCoroutines();
        }
    }

    private IEnumerator SpawnEvery5sec()
    {
        while (true) { 
            SpawnEnemy();
            yield return new WaitForSeconds(5);
        }
    }

    public bool IsSpawnerEmpty { get; private set; } = true;

    public void UpdateSpawner(int spawnerSize, EnemyStatsSO[] enemiesBaseStats)
    {
        _spawnerSize = spawnerSize;
        _enemiesBaseStats = enemiesBaseStats;
    }

    public void FillSpawner()
    {
        _enemies = new List<GameObject>();
        for (int i = 0; i < _spawnerSize; i++)
        {
            int randomStatIndex = Random.Range(0, _enemiesBaseStats.Length);
            GameObject newEnemy = Instantiate(_enemyBasePrefab, _enemiesContainer);
            newEnemy.SetActive(false);
            EnemyManager newEnemyManager = newEnemy.GetComponent<EnemyManager>();
            newEnemyManager.Setup(ref _enemiesBaseStats[randomStatIndex].enemyStats);
            newEnemyManager.enabled = false;
            _enemies.Add(newEnemy);
        }

        IsSpawnerEmpty = false;
    }

    private void SpawnEnemy()
    {
        _newEnemy = _enemies.Last();
        _newEnemy.transform.parent = _newEnemyHolder;
        _newEnemy.SetActive(true);

        _newEnemy.transform.position += Vector3.right * 4;

        _newEnemy.transform.parent = _enemiesContainer;
        _enemies.Remove(_newEnemy);
        _newEnemy = null;

        IsSpawnerEmpty = (_enemies.Count == 0);
    }
}
