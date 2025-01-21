using UnityEngine;

namespace TKM
{
    public class MCJumpingState : MCMovementState
    {
        [Header("Calculations")]
        float _jumpSpeed;
        float _defaultGravityScale;
        float _gravMultiplier;

        [Header("Current State")]
        bool _canJumpAgain = false;
        bool _desiredJump;
        float _jumpBufferCounter;
        float _coyoteTimeCounter = 0;
        bool _pressingJump;
        bool _onGround;
        bool _currentlyJumping;
        Vector2 _velocity;

        public MCJumpingState(MCController _MCController) : base(_MCController)
        {
        }


        public override void Enter()
        {
            _defaultGravityScale = 1f;
        }

        // public void OnJump(InputAction.CallbackContext context)
        // {
        //     //This function is called when one of the jump buttons (like space or the A button) is pressed.

        //     if (moveLimit.characterCanMove)
        //     {
        //         //When we press the jump button, tell the script that we desire a jump.
        //         //Also, use the started and canceled contexts to know if we're currently holding the button
        //         if (context.started)
        //         {
        //             desiredJump = true;
        //             pressingJump = true;
        //         }

        //         if (context.canceled)
        //         {
        //             pressingJump = false;
        //         }
        //     }
        // }

        public override void Update()
        {
            base.Update();
            SetPhysics();

            //Check if we're on ground, using Kit's Ground script
            _onGround = _MCController.GroundDetector.GetOnGround();

            //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
            if (_MCController.JumpData.JumpBuffer > 0)
            {
                //Instead of immediately turning off "desireJump", start counting up...
                //All the while, the DoAJump function will repeatedly be fired off
                if (_desiredJump)
                {
                    _jumpBufferCounter += Time.deltaTime;

                    if (_jumpBufferCounter > _MCController.JumpData.JumpBuffer)
                    {
                        //If time exceeds the jump buffer, turn off "desireJump"
                        _desiredJump = false;
                        _jumpBufferCounter = 0;
                    }
                }
            }

            //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
            //So, start the coyote time counter...
            if (!_currentlyJumping && !_onGround)
            {
                _coyoteTimeCounter += Time.deltaTime;
            }
            else
            {
                //Reset it when we touch the ground, or jump
                _coyoteTimeCounter = 0;
            }
        }

        private void SetPhysics()
        {
            //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
            Vector2 newGravity = new Vector2(0, -2 * _MCController.JumpData.JumpHeight / (_MCController.JumpData.TimeToJumpApex * _MCController.JumpData.TimeToJumpApex));
            _MCController.Rigidbody.gravityScale = newGravity.y / Physics2D.gravity.y * _gravMultiplier;
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

                // if (juice != null)
                // {
                //     //Apply the jumping effects on the juice script
                //     juice.jumpEffects();
                // }
            }

            if (_MCController.JumpData.JumpBuffer == 0)
            {
                //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
                _desiredJump = false;
            }
        }

        // public void BounceUp(float bounceAmount)
        // {
        //     //Used by the springy pad
        //     _MCController.Rigidbody.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
        // }


        /*

        timeToApexStat = scale(1, 10, 0.2f, 2.5f, numberFromPlatformerToolkit)


          public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
            {

                float OldRange = (OldMax - OldMin);
                float NewRange = (NewMax - NewMin);
                float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

                return (NewValue);
            }

        */
    }
}
