using UnityEngine;

namespace TKM
{
    public class MCIdlingState : MCMovementState
    {
        public MCIdlingState(MCController _MCController) : base(_MCController)
        {
        }

        public override void Enter()
        {
            _MCController.Animator.SetTrigger("Grounded");
            _MCController.InputReader.JumpEvent += OnJump;
        }


        public override void Exit()
        {
            _MCController.InputReader.JumpEvent -= OnJump;

        }

        public override void PhysicsUpdate()
        {
            if (_MCController.GetMoveDirection() != Vector3.zero)
            {
                _MCController.SwitchState(_MCController.MCWalkingState);
                return;
            }

            _MCController.Rigidbody.linearVelocity = new Vector3(0, _MCController.Rigidbody.linearVelocity.y, 0);
            // CheckFalling();
        }

        // void CheckFalling()
        // {
        //     if (_MCController.Rigidbody.linearVelocity.y < _MCController.FallingThreshold)
        //     {
        //         _MCController.SwitchState(_MCController.MCFallingState);
        //     }
        // }


        void OnJump()
        {
            _MCController.SwitchState(_MCController.MCJumpingState);
        }

    }
}
