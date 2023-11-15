using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyBasePrefab;
    [SerializeField] EnemyStatsSO[] _enemiesBaseStats;

    // Number of enemies stored per wave
    private int _spawnerSize = 10;
    // Enemies stored
    private GameObject[] _enemies;

    public bool IsSpawnerEmpty { get; private set; } = true;

    public void FillSpawner()
    {
        _enemies = new GameObject[_spawnerSize];
        for (int i = 0; i < _spawnerSize; i++)
        {
            int randomStatIndex = Random.Range(0, _enemiesBaseStats.Length);
            GameObject newEnemy = Instantiate(_enemyBasePrefab);
            newEnemy.SetActive(false);
            EnemyManager newEnemyManager = newEnemy.GetComponent<EnemyManager>();
            newEnemyManager.Setup(ref _enemiesBaseStats[i].enemyStats);
        }

        IsSpawnerEmpty = false;
    }

    private void SpawnEnemy()
    {
        _enemies[_enemies.Length - 1].SetActive(true);

        IsSpawnerEmpty = _enemies.Length == 0;
    }
}
