using UnityEngine;

namespace TKM
{
    [CreateAssetMenu(fileName = "MCMovementData", menuName = "Scriptable Objects/MCMovementData")]
    public class MCMovementData : ScriptableObject
    {
        [Header("Components")]
        // [SerializeField] movementLimiter moveLimit;
        private Rigidbody2D body;
        // MCGroundChecker ground;

        [Header("Movement Stats")]
        [SerializeField, Range(0f, 20f)][Tooltip("Maximum movement speed")] public float maxSpeed = 10f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed")] public float maxAcceleration = 52f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop after letting go")] public float maxDecceleration = 52f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction")] public float maxTurnSpeed = 80f;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed when in mid-air")] public float maxAirAcceleration;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop in mid-air when no direction is used")] public float maxAirDeceleration;
        [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction when in mid-air")] public float maxAirTurnSpeed = 80f;
        [SerializeField][Tooltip("Friction to apply against movement on stick")] public float friction;

        [Header("Options")]
        [Tooltip("When false, the charcter will skip acceleration and deceleration and instantly move and stop")] public bool useAcceleration;
        public bool itsTheIntro = true;
    }
}
