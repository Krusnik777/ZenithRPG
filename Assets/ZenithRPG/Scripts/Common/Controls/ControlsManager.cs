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

        [SerializeField] private SimpleMenuInputController m_simpleMenuInputController;

        private Controls _controls;
        public Controls Controls => _controls;

        public void TurnOffAllControls()
        {
            SetPlayerControlsActive(false);
            SetMainMenuControlsActive(false);
            SetMenuControlsActive(false);
            SetInventoryControlsActive(false);
            SetStoryEventControlsActive(false);
            SetShopControlsActive(false);
            SetSimpleMenuControlsActive(false);
        }

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

        public void SetSimpleMenuControlsActive(bool state)
        {
            if (m_simpleMenuInputController == null)
            {
                if (state == true) m_simpleMenuInputController = FindFirstObjectByType<SimpleMenuInputController>(FindObjectsInactive.Include);

                if (m_simpleMenuInputController == null) return;
            }

            if (state == true) GameState.State = GameState.GameplayState.NotActive;
            m_simpleMenuInputController.enabled = state;
        }

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _controls = new Controls();
            _controls.Enable();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            UpdateStartActiveControls(scene);
        }

        private void UpdateStartActiveControls(Scene scene)
        {
            SetMenuControlsActive(false);
            SetInventoryControlsActive(false);
            SetStoryEventControlsActive(false);
            SetShopControlsActive(false);
            SetSimpleMenuControlsActive(false);

            if (scene.name == SceneCommander.MainMenuSceneName)
            {
                m_mainMenuInputController = FindFirstObjectByType<MainMenuInputController>(FindObjectsInactive.Include);

                SetMainMenuControlsActive(true);
                SetPlayerControlsActive(false);
            }
            else
            {
                m_playerInputController = FindFirstObjectByType<PlayerInputController>(FindObjectsInactive.Include);

                SetMainMenuControlsActive(false);
                SetPlayerControlsActive(true);
            }
        }
    }
}
