using System;
using UnityEngine;

namespace TKM
{
    public class MCMovementState : IState
    {
        protected MCController _MCController;


        public MCMovementState(MCController _MCController)
        {
            this._MCController = _MCController;
        }

        public virtual void Enter()
        {
            _MCController.MCMovement.Enter();
            _MCController.MCJump.Enter();
        }

        public virtual void Exit()
        {
            _MCController.MCMovement.Exit();
            _MCController.MCJump.Exit();
        }

        public virtual void PhysicsUpdate()
        {
            _MCController.MCMovement.PhysicsUpdate();
            _MCController.MCJump.PhysicsUpdate();
        }

        public virtual void Update()
        {
            _MCController.Animator.SetFloat("Speed", Math.Abs(_MCController.Rigidbody.linearVelocityX));
            _MCController.MCMovement.Update();
            _MCController.MCJump.Update();
        }
    }
}
