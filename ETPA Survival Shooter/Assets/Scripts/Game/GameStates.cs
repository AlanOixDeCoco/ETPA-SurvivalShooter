using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class StandbyGameState : IState
{
    private GameManager _gameManager;
    private float _endStandbyTime;

    public StandbyGameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnEnter()
    {
        Debug.Log("Entering standby state!");
        _endStandbyTime = Time.time + _gameManager.StandbyTime;
    }

    public void OnExit()
    {
        _gameManager.GameStats.waves++;
        _gameManager._onWaveStart?.Invoke(_gameManager.GameStats.waves);
    }

    public void Tick()
    {
        if(Time.time > _endStandbyTime)
        {
            _gameManager.StartNextWave();
        }
    }
}

public class WaveGameState : IState
{
    private GameManager _gameManager;
    private float _waveStartTime;
    private Wave _wave;
    private List<GameObject> _inactiveEnemies;
    private float _nextSpawnTime;
    private float _currentSpawnRate;
    private Task _instantiateEnemies;

    public WaveGameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnEnter()
    {
        // Create a new wave object based on the game progression
        _wave = GetNewWave();

        // Instantiate enemies accordingly
        _instantiateEnemies = InstanciateEnemies();

        // Start timer at the end of the generation
        _waveStartTime = Time.time;

        // Set the first enemy spawn time
        _currentSpawnRate = _gameManager.SpawnRateCurve.Evaluate(_gameManager.GameStats.waves);
        _nextSpawnTime = Time.time + _currentSpawnRate;
    }

    public void OnExit()
    {
        _gameManager.GameStats.time += Time.time - _waveStartTime;
        _gameManager._onWaveEnd?.Invoke();
    }

    public void Tick()
    {


        // If there are no enemies left
        if(_gameManager.EnemiesContainer.childCount == 0) _gameManager.StartNextWave();

        if ((Time.time >= _nextSpawnTime) && (_inactiveEnemies.Count > 0))
        {
            // Spawn next enemy
            int randomSpawnerIndex = Random.Range(0, _gameManager.ActiveSpawners.Count);
            var inactiveEnemy = _inactiveEnemies.First();
            Task enemySpawn = _gameManager.ActiveSpawners[randomSpawnerIndex].SpawnEnemy(inactiveEnemy);
            _inactiveEnemies.Remove(inactiveEnemy);
            _nextSpawnTime = Time.time + _currentSpawnRate;
        }
    }

    private Wave GetNewWave()
    {
        int difficulty = GetDifficulty();

        List<EnemyStatsSO> enemiesStatsSOs = new List<EnemyStatsSO>();
        foreach(var enemyUnlock in _gameManager.EnemiesUnlocks)
        {
            if(_gameManager.GameStats.waves >= enemyUnlock.wave)
            {
                enemiesStatsSOs.Add(enemyUnlock.enemyStatsSO);
            }
        }

        return new Wave(difficulty, enemiesStatsSOs);
    }

    private int GetDifficulty()
    {
        int difficulty = 0;
        difficulty += _gameManager.StartDifficulty;
        difficulty += (int)(_gameManager.GameStats.waves / _gameManager.DifficultyPeriod) * _gameManager.StartDifficulty;
        float multiplier = _gameManager.DifficultyCurve.Evaluate((_gameManager.GameStats.waves % _gameManager.DifficultyPeriod) / _gameManager.DifficultyPeriod);
        difficulty += (int)(_gameManager.StartDifficulty * multiplier);

        return difficulty;
    }

    private async Task InstanciateEnemies()
    {
        var enemiesGO = new List<GameObject>();
        foreach(var enemyStats in _wave.EnemiesStats)
        {
            var newEnemy = GameObject.Instantiate(_gameManager.EnemyBasePrefab, _gameManager.EnemiesContainer);
            newEnemy.SetActive(false);
            newEnemy.GetComponent<EnemyManager>().Setup(enemyStats, _gameManager.EnemiesPrimaryTarget);
            enemiesGO.Add(newEnemy);
        }
        _inactiveEnemies = enemiesGO;
    }
}