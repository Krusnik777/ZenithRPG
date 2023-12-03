using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class StoryEventInputController : MonoBehaviour, IDependency<ControlsManager>
    {
        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null) _controls = m_controlsManager.Controls;

            _controls.Event.Enable();

            _controls.Event.Confirm.performed += OnConfirm;
        }

        private void OnConfirm(InputAction.CallbackContext obj)
        {
            StoryEventManager.Instance.ContinueEvent();
        }

        private void OnDisable()
        {
            _controls.Event.Confirm.performed -= OnConfirm;

            _controls.Event.Disable();
        }

    }
}
