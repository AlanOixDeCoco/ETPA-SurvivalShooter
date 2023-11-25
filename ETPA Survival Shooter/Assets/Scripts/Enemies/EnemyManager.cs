using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
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
    [SerializeField] private ParticleSystem _attackParticles;
    [SerializeField] private float _attackDistance = 4f;

    // Public properties
    public UnityAction OnEnemyDeath { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public EnemyStats EnemyStats { get; private set; }
    public Transform PrimaryTarget { get; private set; } = null;
    public Transform Target { get; private set; } = null;
    public bool AttackEnabled { get { return _attackParticles.emission.enabled; } set { _attackParticles.enableEmission = value; } }

    // Private variables
    private bool _isActive = false;
    private StateMachine _stateMachine;

    // Unity methods
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        // Create statemachine
        _stateMachine = new StateMachine();

        // Create states
        var inactiveState = new EnemyInactiveState(this);
        var movingState = new EnemyMovingState(this);
        var attackingState = new EnemyAttackingState(this);

        // Inactive --> Moving
        _stateMachine.AddTransition(inactiveState, movingState, () =>
        {
            return _isActive;
        });

        // Moving --> Inactive
        _stateMachine.AddTransition(movingState, inactiveState, () =>
        {
            return !_isActive;
        });

        // Moving --> Attacking
        _stateMachine.AddTransition(movingState, attackingState, () =>
        {
            return (transform.position - Target.position).magnitude < _attackDistance;
        });

        // Attacking --> Moving
        _stateMachine.AddTransition(attackingState, movingState, () =>
        {
            bool condition = (transform.position - Target.position).magnitude > _attackDistance;
            condition = condition && (Target != PrimaryTarget);
            return condition;
        });

        // Set the entry state
        _stateMachine.SetState(inactiveState);
    }

    private async void OnEnable()
    {
        float startTime = Time.time;
        float scaleDuration = 1f;
        while (transform.localScale.x < EnemyStats.scale)
        {
            float t = Time.time - startTime;
            float lerpFactor = Mathf.Clamp(t / scaleDuration, 0, 1);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * EnemyStats.scale, lerpFactor);
            await Task.Yield();
        }

        _isActive = true;
    }

    private void Update()
    {
        _stateMachine.Tick();

        transform.LookAt(Target);
    }

    // Class methods
    public void Setup(EnemyStats enemyStats, Transform target)
    {
        EnemyStats = (EnemyStats)enemyStats.Clone();
        PrimaryTarget = target;
        _detectionArea.radius = EnemyStats.detectionRadius;
        Agent.speed = EnemyStats.speed;
        gameObject.name = $"{EnemyStats.name}";
        GetComponent<HealthComponent>().SetHealth(enemyStats.health);
        foreach(var mesh in _enemyMeshes)
        {
            mesh.material = EnemyStats.material;
        }
        Target = PrimaryTarget;
    }
    public void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
        _detectionArea.radius = EnemyStats.followRadius / EnemyStats.scale;
        _detectionUI.gameObject.SetActive(true);
    }

    public void LooseTarget()
    {
        Target = PrimaryTarget;
        _detectionArea.radius = EnemyStats.detectionRadius / EnemyStats.scale;
        _detectionUI.gameObject.SetActive(false);
    }

    // Trigger events
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetTarget(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == Target)
        {
            LooseTarget();
        }
    }
}
