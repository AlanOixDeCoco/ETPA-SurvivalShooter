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

public class PlayerMovingState : IState
{
    private PlayerController _playerController;

    public PlayerMovingState(PlayerController playerController)
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

public class PlayerShootingState : IState
{
    private PlayerController _playerController;

    public PlayerShootingState(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void OnEnter()
    {
        _playerController.WeaponController.Shooting = true;
    }

    public void OnExit()
    {
        _playerController.WeaponController.Shooting = false;
    }

    public void Tick()
    {
        _playerController.Walk();
        _playerController.WeaponController.Shooting = _playerController.ShootInput;
    }
}