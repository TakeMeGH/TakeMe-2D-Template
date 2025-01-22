using UnityEngine;

namespace TKM
{
    public class MCJump : IState
    {
        [Header("Components")]
        MCController _MCController;
        MCJumpData _jumpData;
        [Header("Calculations")]
        public float jumpSpeed;
        private float defaultGravityScale = 1f;
        public float gravMultiplier = 1f;
        [HideInInspector] public Vector2 velocity;


        [Header("Current State")]
        public bool canJumpAgain = false;
        private bool desiredJump;
        private float jumpBufferCounter;
        private float coyoteTimeCounter = 0;
        private bool pressingJump;
        public bool onGround;
        private bool currentlyJumping;

        public MCJump(MCController mCController, MCJumpData jumpData)
        {
            _MCController = mCController;
            _jumpData = jumpData;
        }
        public void Enter()
        {
            //Find the character's Rigidbody and ground detection and juice scripts
            defaultGravityScale = 1f;
            _MCController.InputReader.JumpStarted += OnJumpStarted;
            _MCController.InputReader.JumpCanceled += OnJumpCanceled;
        }

        public void OnJumpStarted()
        {
            desiredJump = true;
            pressingJump = true;

        }

        public void OnJumpCanceled()
        {
            pressingJump = false;
        }

        public void Update()
        {
            setPhysics();

            //Check if we're on ground, using Kit's Ground script
            onGround = _MCController.GroundDetector.GetOnGround();

            //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
            if (_jumpData.jumpBuffer > 0)
            {
                //Instead of immediately turning off "desireJump", start counting up...
                //All the while, the DoAJump function will repeatedly be fired off
                if (desiredJump)
                {
                    jumpBufferCounter += Time.deltaTime;

                    if (jumpBufferCounter > _jumpData.jumpBuffer)
                    {
                        //If time exceeds the jump buffer, turn off "desireJump"
                        desiredJump = false;
                        jumpBufferCounter = 0;
                    }
                }
            }

            //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
            //So, start the coyote time counter...
            if (!currentlyJumping && !onGround)
            {
                coyoteTimeCounter += Time.deltaTime;
            }
            else
            {
                //Reset it when we touch the ground, or jump
                coyoteTimeCounter = 0;
            }
        }

        private void setPhysics()
        {
            //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
            Vector2 newGravity = new Vector2(0, -2 * _jumpData.jumpHeight / (_jumpData.timeToJumpApex * _jumpData.timeToJumpApex));
            Debug.Log(newGravity.y + " " + Physics2D.gravity.y + " " + gravMultiplier);
            _MCController.Rigidbody.gravityScale = newGravity.y / Physics2D.gravity.y * gravMultiplier;
        }

        public void PhysicsUpdate()
        {
            //Get velocity from Kit's Rigidbody 
            velocity = _MCController.Rigidbody.linearVelocity;

            //Keep trying to do a jump, for as long as desiredJump is true
            if (desiredJump)
            {
                DoAJump();
                _MCController.Rigidbody.linearVelocity = velocity;

                //Skip gravity calculations this frame, so currentlyJumping doesn't turn off
                //This makes sure you can't do the coyote time double jump bug
                return;
            }

            calculateGravity();
        }

        private void calculateGravity()
        {
            //We change the character's gravity based on her Y direction

            //If Kit is going up...
            if (_MCController.Rigidbody.linearVelocity.y > 0.01f)
            {
                if (onGround)
                {
                    //Don't change it if Kit is stood on something (such as a moving platform)
                    gravMultiplier = defaultGravityScale;
                }
                else
                {
                    //If we're using variable jump height...)
                    if (_jumpData.variablejumpHeight)
                    {
                        //Apply upward multiplier if player is rising and holding jump
                        if (pressingJump && currentlyJumping)
                        {
                            gravMultiplier = _jumpData.upwardMovementMultiplier;
                        }
                        //But apply a special downward multiplier if the player lets go of jump
                        else
                        {
                            gravMultiplier = _jumpData.jumpCutOff;
                        }
                    }
                    else
                    {
                        gravMultiplier = _jumpData.upwardMovementMultiplier;
                    }
                }
            }

            //Else if going down...
            else if (_MCController.Rigidbody.linearVelocity.y < -0.01f)
            {

                if (onGround)
                //Don't change it if Kit is stood on something (such as a moving platform)
                {
                    gravMultiplier = defaultGravityScale;
                }
                else
                {
                    //Otherwise, apply the downward gravity multiplier as Kit comes back to Earth
                    gravMultiplier = _jumpData.downwardMovementMultiplier;
                }

            }
            //Else not moving vertically at all
            else
            {
                if (onGround)
                {
                    currentlyJumping = false;
                }

                gravMultiplier = defaultGravityScale;
            }

            //Set the character's Rigidbody's velocity
            //But clamp the Y variable within the bounds of the speed limit, for the terminal velocity assist option
            _MCController.Rigidbody.linearVelocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -_jumpData.speedLimit, 100));
        }

        private void DoAJump()
        {

            //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
            if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < _jumpData.coyoteTime) || canJumpAgain)
            {
                desiredJump = false;
                jumpBufferCounter = 0;
                coyoteTimeCounter = 0;

                //If we have double jump on, allow us to jump again (but only once)
                canJumpAgain = _jumpData.maxAirJumps == 1 && canJumpAgain == false;

                //Determine the power of the jump, based on our gravity and stats
                jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _MCController.Rigidbody.gravityScale * _jumpData.jumpHeight);

                //If Kit is moving up or down when she jumps (such as when doing a double jump), change the jumpSpeed;
                //This will ensure the jump is the exact same strength, no matter your velocity.
                if (velocity.y > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
                }
                else if (velocity.y < 0f)
                {
                    jumpSpeed += Mathf.Abs(_MCController.Rigidbody.linearVelocity.y);
                }

                //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
                velocity.y += jumpSpeed;
                currentlyJumping = true;

            }

            if (_jumpData.jumpBuffer == 0)
            {
                //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
                desiredJump = false;
            }
        }
        public void Exit()
        {
            _MCController.InputReader.JumpStarted -= OnJumpStarted;
            _MCController.InputReader.JumpCanceled -= OnJumpCanceled;
        }
    }
}
