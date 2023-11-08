using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState
{
    private Animator _animator;

    public PlayerIdleState(Animator animator) { 
        _animator = animator;
    }

    public void OnEnter()
    {
        _animator.SetTrigger("idle");
    }

    public void OnExit()
    {
        _animator.ResetTrigger("idle");
    }

    public void Tick()
    {
        return;
    }
}

public class PlayerWalkState : IState
{
    private PlayerController _playerController;
    private Animator _animator;

    public PlayerWalkState(PlayerController playerController, Animator animator)
    {
        _playerController = playerController;
        _animator = animator;
    }

    public void OnEnter()
    {
        _animator.SetTrigger("walk");
    }

    public void OnExit()
    {
        _animator.ResetTrigger("walk");
        _animator.speed = 1f;
    }

    public void Tick()
    {
        _playerController.Walk();
        _animator.speed = _playerController.MoveInput.magnitude;
    }
}

public class PlayerRunState : IState
{
    private PlayerController _playerController;
    private Animator _animator;

    public PlayerRunState(PlayerController playerController, Animator animator)
    {
        _playerController = playerController;
        _animator = animator;
    }

    public void OnEnter()
    {
        _animator.SetTrigger("run");
    }

    public void OnExit()
    {
        _animator.ResetTrigger("run");
    }

    public void Tick()
    {
        _playerController.Run();
    }
}