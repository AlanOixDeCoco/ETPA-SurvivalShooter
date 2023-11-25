using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    public UnityEvent<int> _onWaveStart;
    public UnityEvent _onWaveEnd;
    public UnityEvent _onGameover;
    public UnityEvent<string> _updateScoreText;
    public UnityEvent<string> _updateWavesCount;
    public UnityEvent<string> _updateWavesText;
    public UnityEvent<float> _updateWaveRemainingEnemies;
    public UnityEvent<string> _updateTimeSurvivedText;

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
    public bool Gameover { get => _gameover; private set => _gameover = value; }
    public int WaveEnemiesCount { get; set; }
    public int EnemiesAlive { get; set; }

    // Private variables
    private StateMachine _stateMachine;
    private bool _gameover = false;

    // Unity methods
    private void Awake()
    {
        _stateMachine = new StateMachine();

        // Create states
        var standbyGameState = new StandbyGameState(this);
        var waveGameState = new WaveGameState(this);
        var gameoverState = new GameoverGameState(this);

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

        // Any --> Gameover
        _stateMachine.AddAnyTransition(gameoverState, () =>
        {
            return Gameover;
        });

        // Set the entry state
        _stateMachine.SetState(standbyGameState);
    }

    private void Start()
    {
        _updateWaveRemainingEnemies?.Invoke(0f);
        _updateScoreText?.Invoke("Score: 0");
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void StartNextWave()
    {
        RequestNextWave = true;
    }

    public void SetGameover()
    {
        Gameover = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void OnEnemyDeath(EnemyStats enemyStats)
    {
        EnemiesAlive--;

        int scoreIncrement = 0;
        scoreIncrement += enemyStats.difficulty * 10;

        GameStats.score += scoreIncrement;

        _updateWaveRemainingEnemies?.Invoke((float)EnemiesAlive / (float)WaveEnemiesCount);
        _updateScoreText?.Invoke("Score: " + GameStats.score.ToString());
    }
}
