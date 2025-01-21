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
        }


        public override void Exit()
        {

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (_MCController.GetMoveDirection() != Vector3.zero)
            {
                _MCController.SwitchState(_MCController.MCWalkingState);
                return;
            }

            _MCController.Rigidbody.linearVelocity = new Vector3(0, _MCController.Rigidbody.linearVelocity.y, 0);
        }
    }
}
