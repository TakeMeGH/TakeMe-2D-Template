using System.Collections.Generic;
using UnityEngine;

namespace TKM
{
    public class InteractionsManager : MonoBehaviour
    {
        [SerializeField] InputReader _inputReader = default;
        List<CustomInteractable> _potentialInteractions = new List<CustomInteractable>();
        CustomInteractable _currentInteraction;
        private void OnEnable()
        {
            _inputReader.InteractPerformed += OnInteractionButtonPress;
        }

        private void OnDisable()
        {
            _inputReader.InteractPerformed -= OnInteractionButtonPress;
        }
        private void OnInteractionButtonPress()
        {
            if (_potentialInteractions.Count == 0)
                return;

            if (_currentInteraction.IsInteractable == false)
            {
                return;
            }
            _currentInteraction.Interact();

        }
        public void OnTriggerChangeDetected(bool entered, GameObject obj)
        {
            if (entered)
                AddPotentialInteraction(obj);
            else
                RemovePotentialInteraction(obj);
        }

        private void AddPotentialInteraction(GameObject obj)
        {
            if (obj.TryGetComponent<CustomInteractable>(out var currentInteraction))
            {
                if (currentInteraction.IsInteractable == false)
                {
                    return;
                }

                if (_potentialInteractions.Count == 0)
                {
                    _currentInteraction = currentInteraction;
                    currentInteraction.InteractIndicator.enabled = true;
                }
                _potentialInteractions.Add(currentInteraction); // insert last
            }
        }

        private void RemovePotentialInteraction(GameObject obj)
        {
            if (obj.TryGetComponent<CustomInteractable>(out var currentInteraction))
            {
                for (int i = 0; i < _potentialInteractions.Count; i++)
                {
                    if (_potentialInteractions[i] == currentInteraction)
                    {
                        _potentialInteractions.RemoveAt(i);
                        currentInteraction.InteractIndicator.enabled = false;
                        break;
                    }
                }
            }
        }
    }
}
