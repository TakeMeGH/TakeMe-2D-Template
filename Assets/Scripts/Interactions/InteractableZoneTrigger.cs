using UnityEngine;
using UnityEngine.Events;

namespace TKM
{
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool, GameObject> { }

    public class InteractableZoneTrigger : MonoBehaviour
    {
        [SerializeField] private BoolEvent _enterZone = default;
        private void OnTriggerEnter2D(Collider2D other)
        {
            _enterZone.Invoke(true, other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _enterZone.Invoke(false, other.gameObject);
        }
    }
}
