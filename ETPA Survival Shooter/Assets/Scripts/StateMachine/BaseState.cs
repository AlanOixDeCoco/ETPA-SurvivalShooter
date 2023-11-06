using System;
using System.Runtime.Serialization;
using UnityEngine;

public abstract class BaseState
{
    protected GameObject gameObject;

    public BaseState(GameObject gameObject){
        this.gameObject = gameObject;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract Type GetNextState();
}
