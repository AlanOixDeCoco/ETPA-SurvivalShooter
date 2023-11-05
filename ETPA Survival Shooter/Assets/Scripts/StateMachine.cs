using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IState
{
    public void Update();
    public void ExitState();
    public void EnterState();
}

public class StateMachine
{
    public IState _currentState { get; private set; }

    public void UpdateState()
    {
        _currentState.Update();
    }

    public void SwitchState(IState nextState)
    {
        _currentState.ExitState();
  
        _currentState = nextState;

        _currentState.EnterState();
    }
}