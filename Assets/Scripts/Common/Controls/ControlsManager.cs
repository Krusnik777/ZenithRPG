using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public static class GameState
    {
        public enum GameplayState
        {
            Active,
            NotActive
        }

        public static GameplayState State;
    }

    public class ControlsManager : MonoBehaviour
    {
        [SerializeField] private MainMenuInputController m_mainMenuInputController;
        [SerializeField] private PlayerInputController m_playerInputController;

        [SerializeField] private MenuInputController m_menuInputController;
        [SerializeField] private InventoryInputController m_inventoryInputController;
        [SerializeField] private StoryEventInputController m_storyEventInputController;
        [SerializeField] private ShopInputController m_shopInputController;

        private Controls _controls;
        public Controls Controls => _controls;

        public void SetMainMenuControlsActive(bool state)
        {
            if (m_mainMenuInputController == null) return;

            m_mainMenuInputController.enabled = state;
        }

        public void SetPlayerControlsActive(bool state)
        {
            if (m_playerInputController == null) return;

            if (state == true) GameState.State = GameState.GameplayState.Active;
            m_playerInputController.enabled = state;
        }

        public void SetMenuControlsActive(bool state)
        {
            if (state == true) GameState.State = GameState.GameplayState.NotActive;
            m_menuInputController.enabled = state;
        }

        public void SetInventoryControlsActive(bool state)
        {
            if (state == true) GameState.State = GameState.GameplayState.NotActive;
            m_inventoryInputController.enabled = state;
        }

        public void SetStoryEventControlsActive(bool state)
        {
            if (state == true) GameState.State = GameState.GameplayState.NotActive;
            m_storyEventInputController.enabled = state;
        }

        public void SetShopControlsActive(bool state)
        {
            if (state == true) GameState.State = GameState.GameplayState.NotActive;
            m_shopInputController.enabled = state;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _controls = new Controls();
            _controls.Enable();

            if (SceneManager.GetActiveScene().name == SceneLoader.MainMenuSceneName)
            {
                m_mainMenuInputController = FindObjectOfType<MainMenuInputController>(true);

                SetMainMenuControlsActive(true);
                SetPlayerControlsActive(false);
            }
            else
            {
                m_playerInputController = FindObjectOfType<PlayerInputController>(true);

                SetMainMenuControlsActive(false);
                SetPlayerControlsActive(true);
            }

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
