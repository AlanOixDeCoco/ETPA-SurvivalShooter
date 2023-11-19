using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyBasePrefab;

    [SerializeField] private Transform _enemySpawnPoint;

    [SerializeField] private float _spawnDuration = 1f;

    // Number of enemies stored per wave
    private int _spawnerSize = 10;
    // Enemies stored
    private List<GameObject> _enemiesToSpawn;

    public bool IsSpawnerEmpty { get; private set; } = true;

    private async Task SpawnEnemy(GameObject newEnemy)
    {
        // Move enemy using linear interpolation
        newEnemy.SetActive(true);

        float startTime = Time.time;
        float lerpFactor = 0;
        Vector3 startPos = newEnemy.transform.position;
        Vector3 targetPos = _enemySpawnPoint.position;
        targetPos.y = startPos.y;
        while (lerpFactor < 1)
        {
            float t = Time.time - startTime;
            lerpFactor = Mathf.Clamp(t / _spawnDuration, 0, 1);
            newEnemy.transform.position = Vector3.Lerp(startPos, targetPos, lerpFactor);
            await Task.Yield();
        }

        newEnemy.GetComponent<EnemyManager>().enabled = true;
    }
}
