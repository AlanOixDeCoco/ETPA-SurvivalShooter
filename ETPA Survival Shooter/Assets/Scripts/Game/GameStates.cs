using System.Collections;
using System.Collections.Generic;
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
        _gameManager._onWaveStart?.Invoke();
    }

    public void OnExit()
    {
        return;
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

    public WaveGameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnEnter()
    {
        _gameManager.GameStats.waves++;
        Debug.Log($"Starting wave {_gameManager.GameStats.waves}");

        // Create a new wave object based on the game progression
        _wave = GetNewWave();

        // Instantiate enemies accordingly
        InstanciateEnemies();

        // Start timer at the end of the generation
        _waveStartTime = Time.time;

        // Start every spawn
        
    }

    public void OnExit()
    {
        _gameManager.GameStats.time += Time.time - _waveStartTime;
        _gameManager._onWaveEnd?.Invoke();
    }

    public void Tick()
    {
        return;
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

    private void InstanciateEnemies()
    {
        var enemiesToSpawn = new List<GameObject>();
        foreach(var enemyStats in _wave.EnemiesStats)
        {
            var newEnemy = GameObject.Instantiate(_gameManager.EnemyBasePrefab, _gameManager.EnemiesContainer);
            newEnemy.SetActive(false);
            newEnemy.GetComponent<EnemyManager>().Setup(enemyStats, _gameManager.EnemiesPrimaryTarget);
            enemiesToSpawn.Add(newEnemy);
        }
    }
}