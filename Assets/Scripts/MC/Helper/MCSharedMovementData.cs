using UnityEngine;

namespace TKM
{
    public class MCSharedMovementData
    {
        [Header("Current State Movement")]
        public bool PressingMove;
        public float DirectionX;

        [Header("Current State Jump")]
        public float JumpBufferCounter = 0;
        public float CoyoteTimeCounter = 0;
        public bool PressingJump;
        public bool DesiredJump;
        public float GravMultiplier;
        public bool CurrentlyJumping;
    }
}
