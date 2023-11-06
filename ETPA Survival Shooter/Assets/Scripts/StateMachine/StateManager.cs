using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<StateType> : MonoBehaviour where StateType : BaseState
{
    protected Dictionary<Type, StateType> _states = new Dictionary<Type, StateType>();
    protected StateType _currentState;

    protected bool _isTransitioning = false;

    private void Start() {
        _currentState.EnterState();
    }
    private void Update() {
        Type nextState = _currentState.GetNextState();
        if (!_isTransitioning && nextState != null)
            _currentState.UpdateState();
        else
            TransitionToState(nextState);
    }

    protected void AddState(StateType newState){
        if(!_states.ContainsKey(newState.GetType())){
            _states.Add(newState.GetType(), newState);
        }
    }

    public void TransitionToState(Type nextState)
    {
        _isTransitioning = true;
        _currentState.ExitState();
        _currentState = _states[nextState];
        _currentState.EnterState();
        _isTransitioning = false;
    }
}
