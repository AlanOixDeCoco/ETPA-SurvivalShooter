using System;
using System.Collections;
using System.Collections.Generic;
using PlayerStates;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerState>
{
    private void Awake(){
        AddState(new IdleState(gameObject));

        _currentState = _states[typeof(IdleState)];
    }
}
