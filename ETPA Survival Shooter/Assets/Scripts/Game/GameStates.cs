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

        _wave = new Wave(_gameManager.GameStats.waves)
    }

    public void OnEnter()
    {
        Debug.Log("Entering wave state!");
        _waveStartTime = Time.time;
    }

    public void OnExit()
    {
        _gameManager.GameStats.time += Time.time - _waveStartTime;
    }

    public void Tick()
    {
        return;
    }

    private Wave GetNextWave()
    {
        int difficulty = 0;
        List<EnemyStatsSO> enemiesStatsSOs = new List<EnemyStatsSO>();

        return new Wave(difficulty, enemiesStatsSOs);
    }
}