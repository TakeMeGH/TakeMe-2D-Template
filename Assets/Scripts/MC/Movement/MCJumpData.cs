using UnityEngine;

namespace TKM
{
    [CreateAssetMenu(fileName = "MCJumpData", menuName = "Scriptable Objects/MCJumpData")]
    public class MCJumpData : ScriptableObject
    {
        [Header("Components")]
        // [HideInInspector] public Rigidbody2D body;
        // private MCGroundChecker ground;
        // [HideInInspector] public Vector2 velocity;
        // private characterJuice juice;
        // [SerializeField] movementLimiter moveLimit;

        [Header("Jumping Stats")]
        [SerializeField, Range(2f, 5.5f)][Tooltip("Maximum jump height")] public float JumpHeight = 7.3f;

        //If you're using your stats from Platformer Toolkit with this character controller, please note that the number on the Jump Duration handle does not match this stat
        //It is re-scaled, from 0.2f - 1.25f, to 1 - 10.
        //You can transform the number on screen to the stat here, using the function at the bottom of this script

        [SerializeField, Range(0.2f, 1.25f)][Tooltip("How long it takes to reach that height before coming back down")] public float TimeToJumpApex;
        [SerializeField, Range(0f, 5f)][Tooltip("Gravity multiplier to apply when going up")] public float UpwardMovementMultiplier = 1f;
        [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier to apply when coming down")] public float DownwardMovementMultiplier = 6.17f;
        [SerializeField, Range(0, 1)][Tooltip("How many times can you jump in the air?")] public int MaxAirJumps = 0;

        [Header("Options")]
        [Tooltip("Should the character drop when you let go of jump?")] public bool VariablejumpHeight;
        [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier when you let go of jump")] public float JumpCutOff;
        [SerializeField][Tooltip("The fastest speed the character can fall")] public float SpeedLimit;
        [SerializeField, Range(0f, 0.3f)][Tooltip("How long should coyote time last?")] public float CoyoteTime = 0.15f;
        [SerializeField, Range(0f, 0.3f)][Tooltip("How far from ground should we cache your jump?")] public float JumpBuffer = 0.15f;

    }
}
