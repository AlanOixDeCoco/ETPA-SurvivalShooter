using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInactiveState : IState
{
    private EnemyManager _enemyManager;

    public EnemyInactiveState(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }

    public void OnEnter()
    {
        _enemyManager.Agent.enabled = false;
        _enemyManager.AttackEnabled = false;
    }

    public void OnExit()
    {
        return;
    }

    public void Tick()
    {
        return;
    }
}

public class EnemyMovingState : IState
{
    private EnemyManager _enemyManager;

    public EnemyMovingState(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }

    public void OnEnter()
    {
        _enemyManager.Agent.enabled = true;
        _enemyManager.AttackEnabled = false;
    }

    public void OnExit()
    {
        return;
    }

    public void Tick()
    {
        if(_enemyManager.Target != null)
        {
            _enemyManager.Agent.destination = _enemyManager.Target.position;
        }
    }
}

public class EnemyAttackingState : IState
{
    private EnemyManager _enemyManager;

    public EnemyAttackingState(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }

    public void OnEnter()
    {
        _enemyManager.Agent.enabled = false;
        _enemyManager.AttackEnabled = true;
    }

    public void OnExit()
    {
        return;
    }

    public void Tick()
    {
        return;
    }
}