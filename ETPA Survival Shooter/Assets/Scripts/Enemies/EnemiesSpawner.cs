using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyBasePrefab;
    [SerializeField] private Transform _enemySpawnStart;
    [SerializeField] private Transform _enemySpawnDestination;
    [SerializeField] private float _spawnDuration = 1f;

    public async Task SpawnEnemy(GameObject newEnemy)
    {
        // Move enemy using linear interpolation
        newEnemy.SetActive(true);

        float startTime = Time.time;
        float lerpFactor = 0;
        newEnemy.transform.position = _enemySpawnStart.position;
        newEnemy.transform.LookAt(_enemySpawnDestination.position);
        while (lerpFactor < 1)
        {
            float t = Time.time - startTime;
            lerpFactor = Mathf.Clamp(t / _spawnDuration, 0, 1);
            newEnemy.transform.position = Vector3.Lerp(_enemySpawnStart.position, _enemySpawnDestination.position, lerpFactor);
            await Task.Yield();
        }

        newEnemy.GetComponent<EnemyManager>().enabled = true;
    }
}
