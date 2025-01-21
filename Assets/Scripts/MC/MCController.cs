using UnityEngine;

namespace TKM
{
    public class MCController : StateMachine
    {
        [field: Header("Input Reader")]
        [field: SerializeField] public InputReader InputReader;
        #region Component

        [field: Header("Component")]
        [field: SerializeField] public MCGroundChecker GroundDetector;
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        #endregion

        #region SharedData
        [field: Header("Read Only")]
        public MCMovementData MovementData;
        public MCJumpData JumpData;
        #endregion

        #region State
        #endregion


    }
}
