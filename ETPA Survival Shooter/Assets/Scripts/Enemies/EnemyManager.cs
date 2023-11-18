using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyManager : MonoBehaviour
{
    // Exposed private variables
    [SerializeField] private SphereCollider _detectionArea;
    [SerializeField] private MeshRenderer[] _enemyMeshes;
    [SerializeField] private Transform _detectionUI;

    // Public properties
    public UnityAction OnEnemyDeath { get; private set; }

    // Private variables
    private NavMeshAgent _agent;
    private EnemyStats _enemyStats;

    private Transform _primaryTarget;
    private Transform _target = null;

    // Unity methods
    private async void OnEnable()
    {
        _agent.enabled = true;

        float startTime = Time.time;
        float lerpFactor = 0;
        float scaleDuration = 1f;
        while (transform.localScale.x < _enemyStats.scale)
        {
            float t = Time.time - startTime;
            lerpFactor = Mathf.Clamp(t / scaleDuration, 0, 1);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * _enemyStats.scale, lerpFactor);
            await Task.Yield();
        }
    }

    private void Update()
    {
        if(_target != null) _agent.destination = _target.position;
        else _agent.destination = _primaryTarget.position;
    }

    // Class methods
    public void Setup(ref EnemyStats enemyStats, ref Transform target)
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyStats = (EnemyStats)enemyStats.Clone();
        _primaryTarget = target;
        _detectionArea.radius = _enemyStats.detectionRadius;
        _agent.speed = _enemyStats.speed;
        gameObject.name = $"{_enemyStats.name}";
        foreach(var mesh in _enemyMeshes)
        {
            mesh.material = _enemyStats.material;
        }
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

    private void SetTarget(Transform target)
    {
        _target = target;
        _detectionArea.radius = _enemyStats.followRadius / _enemyStats.scale;
        _detectionUI.gameObject.SetActive(true);
    }

    private void LooseTarget()
    {
        _target = null;
        _detectionArea.radius = _enemyStats.detectionRadius / _enemyStats.scale;
        _detectionUI.gameObject.SetActive(false);
    }

    // Collision events
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetTarget(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _target)
        {
            LooseTarget();
        }
    }
}
