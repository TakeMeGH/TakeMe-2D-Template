using UnityEngine;

namespace TKM
{
    [CreateAssetMenu(fileName = "MCMovementData", menuName = "Scriptable Objects/MCMovementData")]
    public class MCMovementData : ScriptableObject
    {
        // [Header("Components")]
        // [SerializeField] movementLimiter moveLimit;
        // private Rigidbody2D body;
        // MCGroundChecker ground;

        [Header("Movement Stats")]
        [SerializeField, Range(0f, 20f)][Tooltip("Maximum movement speed")] public float MaxSpeed = 10f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed")] public float MaxAcceleration = 52f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop after letting go")] public float MaxDecceleration = 52f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction")] public float MaxTurnSpeed = 80f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed when in mid-air")] public float MaxAirAcceleration;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop in mid-air when no direction is used")] public float MaxAirDeceleration;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction when in mid-air")] public float MaxAirTurnSpeed = 80f;
        [SerializeField][Tooltip("Friction to apply against movement on stick")] public float Friction;

        [Header("Options")]
        [Tooltip("When false, the charcter will skip acceleration and deceleration and instantly move and stop")] public bool UseAcceleration;
        public bool ItsTheIntro = true;
    }
}
