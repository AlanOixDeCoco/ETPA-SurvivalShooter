using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyBasePrefab;
    [SerializeField] private EnemyStatsSO[] _enemiesBaseStats;

    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private Transform _enemySpawnPoint;

    [SerializeField] private Transform _enemiesTarget;

    // Number of enemies stored per wave
    private int _spawnerSize = 10;
    // Enemies stored
    private List<GameObject> _enemiesToSpawn;

    private void Start()
    {
        FillSpawner();
    }

    float timer = 0;

    private void Update()
    {
        if (!IsSpawnerEmpty)
        {
            if (timer > 5f)
            {
                SpawnEnemy(1f);
                timer = 0;
            }
            timer += Time.deltaTime;
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
        _enemiesToSpawn = new List<GameObject>();
        for (int i = 0; i < _spawnerSize; i++)
        {
            int randomStatIndex = Random.Range(0, _enemiesBaseStats.Length);
            GameObject newEnemy = Instantiate(_enemyBasePrefab, _enemiesContainer);
            newEnemy.SetActive(false);
            EnemyManager newEnemyManager = newEnemy.GetComponent<EnemyManager>();
            newEnemyManager.Setup(ref _enemiesBaseStats[randomStatIndex].enemyStats, ref _enemiesTarget);
            _enemiesToSpawn.Add(newEnemy);
        }

        IsSpawnerEmpty = false;
    }

    private async void SpawnEnemy(float spawnDuration)
    {
        // Move enemy using linear interpolation
        var newEnemy = _enemiesToSpawn[0];
        newEnemy.SetActive(true);

        float startTime = Time.time;
        float lerpFactor = 0;
        Vector3 startPos = newEnemy.transform.position;
        Vector3 targetPos = _enemySpawnPoint.position;
        targetPos.y = startPos.y;
        while (lerpFactor < 1)
        {
            float t = Time.time - startTime;
            lerpFactor = Mathf.Clamp(t / spawnDuration, 0, 1);
            newEnemy.transform.position = Vector3.Lerp(startPos, targetPos, lerpFactor);
            await Task.Yield();
        }

        newEnemy.GetComponent<EnemyManager>().enabled = true;
        _enemiesToSpawn.Remove(newEnemy);

        IsSpawnerEmpty = (_enemiesToSpawn.Count == 0);
    }
}
