using NUnit.Framework.Internal;
using UnityEngine;

namespace TKM
{
    public class MCWalkingState : MCMovementState
    {
        [Header("Calculations")]
        public float directionX;
        private Vector2 desiredVelocity;
        public Vector2 velocity;
        private float maxSpeedChange;
        private float acceleration;
        private float deceleration;
        private float turnSpeed;

        [Header("Current State")]
        public bool onGround;
        public bool pressingKey;

        public MCWalkingState(MCController _MCController) : base(_MCController)
        {
        }

        public override void Update()
        {
            // //Used to stop movement when the character is playing her death animation
            // if (!moveLimit.characterCanMove && !itsTheIntro)
            // {
            //     directionX = 0;
            // }
            base.Update();

            CheckToIdle();

            directionX = _MCController.RawDirection.x;

            //Used to flip the character's sprite when she changes direction
            //Also tells us that we are currently pressing a direction button
            if (directionX != 0)
            {
                _MCController.transform.localScale = new Vector3(directionX > 0 ? 1 : -1, 1, 1);
                pressingKey = true;
            }
            else
            {
                pressingKey = false;
            }

            //Calculate's the character's desired velocity - which is the direction you are facing, multiplied by the character's maximum speed
            //Friction is not used in this game
            desiredVelocity = new Vector2(directionX, 0f) * Mathf.Max(_MCController.MovementData.maxSpeed - _MCController.MovementData.friction, 0f);

        }

        public override void PhysicsUpdate()
        {
            //Fixed update runs in sync with Unity's physics engine

            //Get Kit's current ground status from her ground script
            onGround = _MCController.GroundDetector.GetOnGround();

            //Get the Rigidbody's current velocity
            velocity = _MCController.Rigidbody.linearVelocity;

            //Calculate movement, depending on whether "Instant Movement" has been checked
            if (_MCController.MovementData.useAcceleration)
            {
                RunWithAcceleration();
            }
            else
            {
                if (onGround)
                {
                    RunWithoutAcceleration();
                }
                else
                {
                    RunWithAcceleration();
                }
            }
        }

        private void RunWithAcceleration()
        {
            //Set our acceleration, deceleration, and turn speed stats, based on whether we're on the ground on in the air

            acceleration = onGround ? _MCController.MovementData.maxAcceleration : _MCController.MovementData.maxAirAcceleration;
            deceleration = onGround ? _MCController.MovementData.maxDecceleration : _MCController.MovementData.maxAirDeceleration;
            turnSpeed = onGround ? _MCController.MovementData.maxTurnSpeed : _MCController.MovementData.maxAirTurnSpeed;

            if (pressingKey)
            {
                //If the sign (i.e. positive or negative) of our input direction doesn't match our movement, it means we're turning around and so should use the turn speed stat.
                if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x))
                {
                    maxSpeedChange = turnSpeed * Time.deltaTime;
                }
                else
                {
                    //If they match, it means we're simply running along and so should use the acceleration stat
                    maxSpeedChange = acceleration * Time.deltaTime;
                }
            }
            else
            {
                //And if we're not pressing a direction at all, use the deceleration stat
                maxSpeedChange = deceleration * Time.deltaTime;
            }

            //Move our velocity towards the desired velocity, at the rate of the number calculated above
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

            //Update the Rigidbody with this new velocity
            _MCController.Rigidbody.linearVelocity = velocity;

        }

        private void RunWithoutAcceleration()
        {
            //If we're not using acceleration and deceleration, just send our desired velocity (direction * max speed) to the Rigidbody
            velocity.x = desiredVelocity.x;

            _MCController.Rigidbody.linearVelocity = velocity;
        }

        void CheckToIdle()
        {
            if (_MCController.Rigidbody.linearVelocity.x == 0f && !pressingKey)
            {
                _MCController.SwitchState(_MCController.MCIdlingState);
            }
        }

    }
}
