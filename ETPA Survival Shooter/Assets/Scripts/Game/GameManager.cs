using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wave
{
    public List<EnemyStats> EnemiesStats {  get; private set; }

    public Wave(int difficulty, List<EnemyStatsSO> enemiesStatsSOs)
    {
        EnemiesStats = new List<EnemyStats>();

        while(difficulty > 0)
        {
            int randomEnemyStats = Random.Range(0, enemiesStatsSOs.Count);

            EnemyStats newEnemyStats = (EnemyStats)enemiesStatsSOs[randomEnemyStats].enemyStats.Clone();
            difficulty -= newEnemyStats.difficulty;

            EnemiesStats.Add(newEnemyStats);
        }
    }
}

public class GameStats {
    public string name = "Anonymous";
    public int score = 0;
    public int enemyKilled = 0;
    public int waves = 0;
    public float time = 0;
}

[System.Serializable]
public class EnemyUnlock
{
    public int wave = 0;
    public EnemyStatsSO enemyStatsSO;
}

public class GameManager : MonoBehaviour
{
    [Header("Waves settings")]
    [SerializeField] private AnimationCurve _difficultyCurve;
    [SerializeField] private AnimationCurve _spawnRateCurve;
    [SerializeField] private int _difficultyPeriod = 10;
    [SerializeField] private int _startDifficulty = 10;
    [SerializeField] private List<EnemyUnlock> _enemiesUnlocks = new List<EnemyUnlock>();

    [Header("Standby settings")]
    [SerializeField] private float _standbyTime;

    [Header("Enemies settings")]
    [SerializeField] private List<EnemiesSpawner> _activeSpawners;
    [SerializeField] private GameObject _enemyBasePrefab;
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private Transform _enemiesPrimaryTarget;

    [Header("Events")]
    public UnityEvent _onWaveStart;
    public UnityEvent _onWaveEnd;

    // Public / private set properties
    public GameStats GameStats { get; private set; } = new GameStats();
    public bool RequestNextWave { get; private set; } = false;
    public float StandbyTime {get => _standbyTime; }
    public int DifficultyPeriod { get => _difficultyPeriod; }
    public int StartDifficulty { get => _startDifficulty; }
    public List<EnemyUnlock> EnemiesUnlocks { get => _enemiesUnlocks; }
    public AnimationCurve DifficultyCurve { get => _difficultyCurve; }
    public GameObject EnemyBasePrefab { get => _enemyBasePrefab; }
    public Transform EnemiesPrimaryTarget { get => _enemiesPrimaryTarget; }
    public Transform EnemiesContainer { get => _enemiesContainer; }
    public AnimationCurve SpawnRateCurve { get => _spawnRateCurve; }
    public List<EnemiesSpawner> ActiveSpawners { get => _activeSpawners; }

    // Private variables
    private StateMachine _stateMachine;

    // Unity methods
    private void Awake()
    {
        _stateMachine = new StateMachine();

        // Create states
        var standbyGameState = new StandbyGameState(this);
        var waveGameState = new WaveGameState(this);

        // Standby --> Wave
        _stateMachine.AddTransition(standbyGameState, waveGameState, () =>
        {
            if(RequestNextWave)
            {
                RequestNextWave = false;
                return true;
            }
            return false;
        });

        // Wave --> Standby
        _stateMachine.AddTransition(waveGameState, standbyGameState, () =>
        {
            return _enemiesContainer.childCount <= 0;
        });

        // Set the entry state
        _stateMachine.SetState(standbyGameState);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void StartNextWave()
    {
        RequestNextWave = true;
    }
}
