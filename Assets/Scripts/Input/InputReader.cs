using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TKM
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
    public class InputReader : ScriptableObject, MCInput.IGameplayActions, MCInput.IUIActions
    {
        public Action<Vector2> MoveEvent;
        public Action JumpEvent;


        MCInput _MCInput;
        void OnEnable()
        {
            if (_MCInput == null)
            {
                _MCInput = new MCInput();
                _MCInput.Gameplay.SetCallbacks(this);
                _MCInput.UI.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        void OnDisable()
        {
            if (_MCInput != null) _MCInput.Gameplay.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void EnableGameplayInput()
        {
            _MCInput.Gameplay.Enable();
            _MCInput.UI.Disable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void EnableUIInput()
        {
            _MCInput.Gameplay.Disable();
            _MCInput.UI.Enable();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                JumpEvent?.Invoke();
            }
        }
    }
}
