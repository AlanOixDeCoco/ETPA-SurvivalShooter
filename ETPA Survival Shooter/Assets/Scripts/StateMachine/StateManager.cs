using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> _states = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> _currentState;

    protected bool _isTransitioning = false;

    private void Start() {
        _currentState.EnterState();
    }
    private void Update() {
        EState nextStateKey = _currentState.GetNextState();
        if (!_isTransitioning && nextStateKey.Equals(_currentState.StateKey))
            _currentState.UpdateState();
        else
            TransitionToState(nextStateKey);
    }

    public void TransitionToState(EState nextStateKey)
    {
        _isTransitioning = true;
        _currentState.ExitState();
        _currentState = _states[nextStateKey];
        _currentState.EnterState();
        _isTransitioning = false;
    }

    private void OnTriggerEnter(Collider other) {
        _currentState.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other) {
        _currentState.OnTriggerStay(other);
    }
    private void OnTriggerExit(Collider other) {
        _currentState.OnTriggerExit(other);
    }
}
