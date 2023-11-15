using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyManager : MonoBehaviour
{
    // Every enemy have these exposed private variables
    [SerializeField] private SphereCollider _detectionArea;
    [SerializeField] private float _spawnTime = 3f;

    // Every enemy have these public properties
    public UnityAction<EnemyTypes> OnEnemyDeath { get; private set; }

    // Every enemy have these private variables
    private NavMeshAgent _agent;
    private EnemyStats _enemyStats;
    private bool _isReady;

    // Every enemy have these methods
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        gameObject.SetActive(false);
    }

    public void Setup(ref EnemyStats enemyStats)
    {
        _enemyStats = (EnemyStats)enemyStats.Clone();
    }

    public IEnumerator Spawn()
    {

        yield return new WaitForSeconds(_spawnTime);
        _isReady = true;
    }

    public void TakeDamage(int damage)
    {
        _enemyStats.health -= damage;
        if(_enemyStats.health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        OnEnemyDeath?.Invoke(_enemyStats.type);
        Destroy(gameObject);
    }
}
