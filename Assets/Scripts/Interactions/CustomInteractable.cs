using UnityEngine;
using UnityEngine.Events;

namespace TKM
{
    public class CustomInteractable : MonoBehaviour
    {
        public UnityEvent<Component> FunctionTrigger;
        public SpriteRenderer InteractIndicator;
        public bool IsInteractable = true;

        private void Start()
        {
            SetInteractable(IsInteractable);
            InteractIndicator.enabled = false;
        }

        public void SetInteractable(bool state)
        {
            IsInteractable = state;
            if (state == false) InteractIndicator.enabled = state;
        }
        public void Interact()
        {
            FunctionTrigger?.Invoke(this);
        }
    }
}
