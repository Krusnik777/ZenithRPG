using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class ControlsManager : MonoSingleton<ControlsManager>
    {
        [SerializeField] private PlayerInputController m_playerInputController;
        [SerializeField] private MenuInputController m_menuInputController;
        [SerializeField] private InventoryInputController m_inventoryInputController;
        [SerializeField] private StoryEventInputController m_storyEventInputController;
        [SerializeField] private ShopInputController m_shopInputController;

        private Controls _controls;
        public Controls Controls => _controls;

        public void SetPlayerControlsActive(bool state)
        {
            m_playerInputController.enabled = state;
        }

        public void SetMenuControlsActive(bool state)
        {
            m_menuInputController.enabled = state;
        }

        public void SetInventoryControlsActive(bool state)
        {
            m_inventoryInputController.enabled = state;
        }

        public void SetStoryEventControlsActive(bool state)
        {
            m_storyEventInputController.enabled = state;
        }

        public void SetShopControlsActive(bool state)
        {
            m_shopInputController.enabled = state;
        }

        protected override void Awake()
        {
            base.Awake();

            _controls = new Controls();
            _controls.Enable();
            SetPlayerControlsActive(true);
            SetMenuControlsActive(false);
            SetInventoryControlsActive(false);
            SetStoryEventControlsActive(false);
            SetShopControlsActive(false);
        }

        private void OnDisable()
        {
            _controls.Disable();
        }
    }
}
