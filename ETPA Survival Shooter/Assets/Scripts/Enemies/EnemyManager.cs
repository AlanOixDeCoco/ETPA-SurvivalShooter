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

    // Every enemy have these public properties
    public UnityAction OnEnemyDeath { get; private set; }

    // Every enemy have these private variables
    private NavMeshAgent _agent;
    private EnemyStats _enemyStats;

    // Every enemy have these methods
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        gameObject.SetActive(false);
    }

    public void Setup(ref EnemyStats enemyStats)
    {
        _enemyStats = (EnemyStats)enemyStats.Clone();
        gameObject.name = $"{_enemyStats.name} ID{gameObject.GetInstanceID()}";
        GetComponent<MeshRenderer>().material = _enemyStats.material;
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
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }
}
