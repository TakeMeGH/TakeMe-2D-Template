using System;
using UnityEngine;

namespace TKM
{
    public class MCMovementState : IState
    {
        protected MCController _MCController;
        protected bool _pressingKey;
        protected float _directionX;


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
            CheckPressingKey();
        }

        void CheckPressingKey()
        {
            _directionX = _MCController.RawDirection.x;

            //Used to flip the character's sprite when she changes direction
            //Also tells us that we are currently pressing a direction button
            if (_directionX != 0)
            {
                _MCController.transform.localScale = new Vector3(_directionX > 0 ? 1 : -1, 1, 1);
                _pressingKey = true;
            }
            else
            {
                _pressingKey = false;
            }

        }

    }
}
