using System;
using UnityEngine;

public abstract class PlayerState : BaseState
{
    protected PlayerState(GameObject gameObject) : base(gameObject)
    {
    }
}

namespace PlayerStates {
    public class IdleState : PlayerState
    {
        public IdleState(GameObject gameObject) : base(gameObject)
        {
        }

        public override void EnterState()
        {
            Debug.Log(gameObject.name);
        }

        public override void ExitState()
        {
            throw new NotImplementedException();
        }

        public override Type GetNextState()
        {
            throw new NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new NotImplementedException();
        }
    }
}