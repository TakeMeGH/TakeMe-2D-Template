using UnityEngine;

namespace TKM
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected IState currentState;
        bool _onSwitchState = false;

        public void SwitchState(IState newState)
        {
            _onSwitchState = true;
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
            _onSwitchState = false;
        }

        protected virtual void Update()
        {
            if (!_onSwitchState) currentState?.Update();
        }

        protected virtual void FixedUpdate()
        {
            if (!_onSwitchState) currentState?.PhysicsUpdate();

        }

        public System.Type GetCurrentStateType()
        {
            return currentState.GetType();
        }

        public IState GetCurrentState()
        {
            return currentState;
        }
    }
}
