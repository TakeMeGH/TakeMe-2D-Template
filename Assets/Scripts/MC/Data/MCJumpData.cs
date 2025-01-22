using UnityEngine;

namespace TKM
{
    [CreateAssetMenu(fileName = "MCJumpData", menuName = "Scriptable Objects/MCJumpData")]
    public class MCJumpData : ScriptableObject
    {
        [Header("Jumping Stats")]
        [SerializeField, Range(2f, 5.5f)][Tooltip("Maximum jump height")] public float jumpHeight = 7.3f;

        //If you're using your stats from Platformer Toolkit with this character controller, please note that the number on the Jump Duration handle does not match this stat
        //It is re-scaled, from 0.2f - 1.25f, to 1 - 10.
        //You can transform the number on screen to the stat here, using the function at the bottom of this script

        [SerializeField, Range(0.3f, 1.25f)][Tooltip("How long it takes to reach that height before coming back down")] public float timeToJumpApex;
        [SerializeField, Range(0f, 5f)][Tooltip("Gravity multiplier to apply when going up")] public float upwardMovementMultiplier = 1f;
        [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier to apply when coming down")] public float downwardMovementMultiplier = 6.17f;
        [SerializeField, Range(0, 1)][Tooltip("How many times can you jump in the air?")] public int maxAirJumps = 0;

        [Header("Options")]
        [Tooltip("Should the character drop when you let go of jump?")] public bool variablejumpHeight;
        [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier when you let go of jump")] public float jumpCutOff;
        [SerializeField][Tooltip("The fastest speed the character can fall")] public float speedLimit;
        [SerializeField, Range(0f, 0.3f)][Tooltip("How long should coyote time last?")] public float coyoteTime = 0.15f;
        [SerializeField, Range(0f, 0.3f)][Tooltip("How far from ground should we cache your jump?")] public float jumpBuffer = 0.15f;
    }
}
