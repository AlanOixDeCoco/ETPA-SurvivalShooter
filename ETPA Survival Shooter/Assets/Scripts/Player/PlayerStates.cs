using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState
{
    public void OnEnter()
    {
        return;
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

public class PlayerWalkState : IState
{
    private PlayerController _playerController;

    public PlayerWalkState(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void OnEnter()
    {
        return;
    }

    public void OnExit()
    {
        return;
    }

    public void Tick()
    {
        _playerController.Walk();
    }
}

public class PlayerRunState : IState
{
    private PlayerController _playerController;

    public PlayerRunState(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void OnEnter()
    {
        return;
    }

    public void OnExit()
    {
        return;
    }

    public void Tick()
    {
        _playerController.Run();
    }
}