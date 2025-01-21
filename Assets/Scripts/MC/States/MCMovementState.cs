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

        }

        public virtual void Exit()
        {

        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void Update()
        {
            // Set Animator for Walk and Idle
            _MCController.Animator.SetFloat("Speed", Math.Abs(_MCController.Rigidbody.linearVelocityX));

            CheckPressingKey();
            SetPhysics();
            CheckJumpBuffer();
            CheckCoyoteBuffer();
        }

        void CheckPressingKey()
        {
            _MCController.SharedMovementData.DirectionX = _MCController.RawDirection.x;

            //Used to flip the character's sprite when she changes direction
            //Also tells us that we are currently pressing a direction button
            if (_MCController.SharedMovementData.DirectionX != 0)
            {
                _MCController.transform.localScale = new Vector3(_MCController.SharedMovementData.DirectionX > 0 ? 1 : -1, 1, 1);
                _MCController.SharedMovementData.PressingMove = true;
            }
            else
            {
                _MCController.SharedMovementData.PressingMove = false;
            }

        }

        private void SetPhysics()
        {
            //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
            Vector2 newGravity = new Vector2(0, -2 * _MCController.JumpData.JumpHeight / (_MCController.JumpData.TimeToJumpApex * _MCController.JumpData.TimeToJumpApex));
            _MCController.Rigidbody.gravityScale = newGravity.y / Physics2D.gravity.y * _MCController.SharedMovementData.GravMultiplier;
        }

        private void CheckJumpBuffer()
        {
            //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
            if (_MCController.JumpData.JumpBuffer > 0)
            {
                //Instead of immediately turning off "desireJump", start counting up...
                //All the while, the DoAJump function will repeatedly be fired off
                if (_MCController.SharedMovementData.DesiredJump)
                {
                    _MCController.SharedMovementData.JumpBufferCounter += Time.deltaTime;

                    if (_MCController.SharedMovementData.JumpBufferCounter > _MCController.JumpData.JumpBuffer)
                    {
                        //If time exceeds the jump buffer, turn off "desireJump"
                        _MCController.SharedMovementData.DesiredJump = false;
                        _MCController.SharedMovementData.JumpBufferCounter = 0;
                    }
                }
            }
        }

        private void CheckCoyoteBuffer()
        {
            //Check if we're on ground, using Kit's Ground script
            bool onGround = _MCController.GroundDetector.GetOnGround();

            //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
            //So, start the coyote time counter...
            if (!_MCController.SharedMovementData.CurrentlyJumping && !onGround)
            {
                _MCController.SharedMovementData.CoyoteTimeCounter += Time.deltaTime;
            }
            else
            {
                //Reset it when we touch the ground, or jump
                _MCController.SharedMovementData.CoyoteTimeCounter = 0;
            }
        }
    }
}
