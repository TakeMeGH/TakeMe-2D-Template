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
        public Rigidbody2D Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        #endregion

        #region SharedData
        [field: Header("Read Only")]
        public MCMovementData MovementData;
        public MCJumpData JumpData;
        public Vector2 RawDirection { get; private set; }
        #endregion

        #region State
        public MCIdlingState MCIdlingState { get; private set; }
        public MCWalkingState MCWalkingState { get; private set; }
        public MCJumpingState MCJumpingState { get; private set; }
        public MCFallingState MCFallingState { get; private set; }
        #endregion

        void OnEnable()
        {
            InputReader.MoveEvent += MoveMC;
        }

        void OnDisable()
        {
            InputReader.MoveEvent -= MoveMC;

        }

        void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();

            MCIdlingState = new MCIdlingState(this);
            MCWalkingState = new MCWalkingState(this);
            MCJumpingState = new MCJumpingState(this);
            MCFallingState = new MCFallingState(this);
        }
        void Start()
        {
            Initialize();
            SwitchState(MCIdlingState);
        }

        void MoveMC(Vector2 pos)
        {
            RawDirection = pos.normalized;
        }

        public Vector3 GetMoveDirection()
        {
            Vector3 horizontal = transform.right * RawDirection.x;
            Vector3 vertical = transform.forward * RawDirection.y;
            return horizontal + vertical;
        }
    }
}
