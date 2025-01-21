using UnityEngine;

namespace TKM
{
    public class MCWalkingState : MCMovementState
    {
        [Header("Calculations")]
        Vector2 _desiredVelocity;
        Vector2 _velocity;
        float _maxSpeedChange;
        float _acceleration;
        float _deceleration;
        float _turnSpeed;

        [Header("Current State")]
        bool _onGround;

        public MCWalkingState(MCController _MCController) : base(_MCController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _MCController.Animator.SetTrigger("Grounded");
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
            //Calculate's the character's desired velocity - which is the direction you are facing, multiplied by the character's maximum speed
            //Friction is not used in this game
            _desiredVelocity = new Vector2(_MCController.SharedMovementData.DirectionX, 0f) * Mathf.Max(_MCController.MovementData.MaxSpeed - _MCController.MovementData.Friction, 0f);

        }

        public override void PhysicsUpdate()
        {
            //Fixed update runs in sync with Unity's physics engine

            //Get Kit's current ground status from her ground script
            _onGround = _MCController.GroundDetector.GetOnGround();

            //Get the Rigidbody's current velocity
            _velocity = _MCController.Rigidbody.linearVelocity;

            //Calculate movement, depending on whether "Instant Movement" has been checked
            if (_MCController.MovementData.UseAcceleration)
            {
                RunWithAcceleration();
            }
            else
            {
                if (_onGround)
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

            _acceleration = _onGround ? _MCController.MovementData.MaxAcceleration : _MCController.MovementData.MaxAirAcceleration;
            _deceleration = _onGround ? _MCController.MovementData.MaxDecceleration : _MCController.MovementData.MaxAirDeceleration;
            _turnSpeed = _onGround ? _MCController.MovementData.MaxTurnSpeed : _MCController.MovementData.MaxAirTurnSpeed;

            if (_MCController.SharedMovementData.PressingMove)
            {
                //If the sign (i.e. positive or negative) of our input direction doesn't match our movement, it means we're turning around and so should use the turn speed stat.
                if (Mathf.Sign(_MCController.SharedMovementData.DirectionX) != Mathf.Sign(_velocity.x))
                {
                    _maxSpeedChange = _turnSpeed * Time.deltaTime;
                }
                else
                {
                    //If they match, it means we're simply running along and so should use the acceleration stat
                    _maxSpeedChange = _acceleration * Time.deltaTime;
                }
            }
            else
            {
                //And if we're not pressing a direction at all, use the deceleration stat
                _maxSpeedChange = _deceleration * Time.deltaTime;
            }

            //Move our velocity towards the desired velocity, at the rate of the number calculated above
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            //Update the Rigidbody with this new velocity
            _MCController.Rigidbody.linearVelocity = _velocity;

        }

        private void RunWithoutAcceleration()
        {
            //If we're not using acceleration and deceleration, just send our desired velocity (direction * max speed) to the Rigidbody
            _velocity.x = _desiredVelocity.x;
            _MCController.Rigidbody.linearVelocity = _velocity;
        }

        void CheckToIdle()
        {
            if (_MCController.Rigidbody.linearVelocity.x == 0f && !_MCController.SharedMovementData.PressingMove)
            {
                _MCController.SwitchState(_MCController.MCIdlingState);
            }
        }

    }
}
