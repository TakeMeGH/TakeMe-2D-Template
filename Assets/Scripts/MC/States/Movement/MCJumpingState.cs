using UnityEngine;

namespace TKM
{
    public class MCJumpingState : MCMovementState
    {
        [Header("Calculations")]
        float _jumpSpeed;
        float _defaultGravityScale;
        // float _gravMultiplier;

        [Header("Current State")]
        bool _canJumpAgain = false;
        bool _onGround;
        Vector2 _velocity;

        public MCJumpingState(MCController _MCController) : base(_MCController)
        {
        }


        public override void Enter()
        {
            _defaultGravityScale = 1f;
        }
        public override void PhysicsUpdate()
        {
            //Get velocity from Kit's Rigidbody 
            _velocity = _MCController.Rigidbody.linearVelocity;

            //Keep trying to do a jump, for as long as desiredJump is true
            if (_desiredJump)
            {
                DoAJump();
                _MCController.Rigidbody.linearVelocity = _velocity;

                //Skip gravity calculations this frame, so currentlyJumping doesn't turn off
                //This makes sure you can't do the coyote time double jump bug
                return;
            }

            CalculateGravity();
        }

        private void CalculateGravity()
        {
            //We change the character's gravity based on her Y direction

            //If Kit is going up...
            if (_MCController.Rigidbody.linearVelocity.y > 0.01f)
            {
                if (_onGround)
                {
                    //Don't change it if Kit is stood on something (such as a moving platform)
                    _gravMultiplier = _defaultGravityScale;
                }
                else
                {
                    //If we're using variable jump height...)
                    if (_MCController.JumpData.VariablejumpHeight)
                    {
                        //Apply upward multiplier if player is rising and holding jump
                        if (_pressingJump && _currentlyJumping)
                        {
                            _gravMultiplier = _MCController.JumpData.UpwardMovementMultiplier;
                        }
                        //But apply a special downward multiplier if the player lets go of jump
                        else
                        {
                            _gravMultiplier = _MCController.JumpData.JumpCutOff;
                        }
                    }
                    else
                    {
                        _gravMultiplier = _MCController.JumpData.UpwardMovementMultiplier;
                    }
                }
            }

            //Else if going down...
            else if (_MCController.Rigidbody.linearVelocity.y < -0.01f)
            {

                if (_onGround)
                //Don't change it if Kit is stood on something (such as a moving platform)
                {
                    _gravMultiplier = _defaultGravityScale;
                }
                else
                {
                    //Otherwise, apply the downward gravity multiplier as Kit comes back to Earth
                    _gravMultiplier = _MCController.JumpData.DownwardMovementMultiplier;
                }

            }
            //Else not moving vertically at all
            else
            {
                if (_onGround)
                {
                    _currentlyJumping = false;
                }

                _gravMultiplier = _defaultGravityScale;
            }

            //Set the character's Rigidbody's velocity
            //But clamp the Y variable within the bounds of the speed limit, for the terminal velocity assist option
            _MCController.Rigidbody.linearVelocity = new Vector3(_velocity.x, Mathf.Clamp(_velocity.y, -_MCController.JumpData.SpeedLimit, 100));
        }

        private void DoAJump()
        {

            //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
            if (_onGround || (_coyoteTimeCounter > 0.03f && _coyoteTimeCounter < _MCController.JumpData.CoyoteTime) || _canJumpAgain)
            {
                _desiredJump = false;
                _jumpBufferCounter = 0;
                _coyoteTimeCounter = 0;

                //If we have double jump on, allow us to jump again (but only once)
                _canJumpAgain = _MCController.JumpData.MaxAirJumps == 1 && _canJumpAgain == false;

                //Determine the power of the jump, based on our gravity and stats
                _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _MCController.Rigidbody.gravityScale * _MCController.JumpData.JumpHeight);

                //If Kit is moving up or down when she jumps (such as when doing a double jump), change the jumpSpeed;
                //This will ensure the jump is the exact same strength, no matter your velocity.
                if (_velocity.y > 0f)
                {
                    _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                }
                else if (_velocity.y < 0f)
                {
                    _jumpSpeed += Mathf.Abs(_MCController.Rigidbody.linearVelocity.y);
                }

                //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
                _velocity.y += _jumpSpeed;
                _currentlyJumping = true;
            }

            if (_MCController.JumpData.JumpBuffer == 0)
            {
                //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
                _desiredJump = false;
            }
        }

    }
}
