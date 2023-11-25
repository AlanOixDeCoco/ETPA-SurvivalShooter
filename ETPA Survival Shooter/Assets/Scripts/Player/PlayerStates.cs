using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerController _playerController;
    public PlayerIdleState(PlayerController playerController)
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
        // Rotate mesh
        _playerController.Mesh.LookAt(_playerController.AimLookTarget);
        Quaternion rotation = _playerController.Mesh.transform.rotation;
        rotation.x = 0;
        rotation.z = 0;
        _playerController.Mesh.transform.rotation = rotation;

        // Rotate weapon
        _playerController.WeaponController.transform.parent.LookAt(_playerController.AimLookTarget);
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

        // Rotate mesh
        _playerController.Mesh.LookAt(_playerController.AimLookTarget);
        Quaternion rotation = _playerController.Mesh.transform.rotation;
        rotation.x = 0;
        rotation.z = 0;
        _playerController.Mesh.transform.rotation = rotation;

        // Rotate weapon
        _playerController.WeaponController.transform.parent.LookAt(_playerController.AimLookTarget);
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

        // Rotate mesh
        _playerController.Mesh.LookAt(_playerController.AimLookTarget);
        Quaternion rotation = _playerController.Mesh.transform.rotation;
        rotation.x = 0;
        rotation.z = 0;
        _playerController.Mesh.transform.rotation = rotation;

        // Rotate weapon
        _playerController.WeaponController.transform.parent.LookAt(_playerController.AimLookTarget);

        // Handle shoot input
        _playerController.WeaponController.Shooting = _playerController.ShootInput;
    }
}