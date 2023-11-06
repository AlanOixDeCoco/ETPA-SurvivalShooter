using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerStates>
{
    public enum PlayerStates
    {
        Idle,
        Walk,
        Run
    }
}
