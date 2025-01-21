using System;

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

        }

        public virtual void Exit()
        {

        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void Update()
        {
            _MCController.Animator.SetFloat("Speed", Math.Abs(_MCController.Rigidbody.linearVelocityX));
        }
    }
}
