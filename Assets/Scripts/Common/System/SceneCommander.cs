using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace DC_ARPG
{
    public class SceneCommander : MonoSingleton<SceneCommander>, IDependency<ControlsManager>
    {
        public static string MainMenuSceneName = "MainMenu";

        [SerializeField] private LevelsDataBase m_levelsDataBase;
        [Header("Loading")]
        [SerializeField] private GameObject m_loadingCanvas;
        [SerializeField] private Image m_loadingBarFillImage;
        public LevelData TutorialLevel => m_levelsDataBase.TutorialLevel;
        public LevelData[] Levels => m_levelsDataBase.Levels;

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private Coroutine loadingCoroutine;

        public string GetCurrentLevelTitle()
        {
            if (SceneManager.GetActiveScene().name == TutorialLevel.SceneName)
                return TutorialLevel.LevelTitle;

            foreach (var level in m_levelsDataBase.Levels)
            {
                if (level.SceneName == SceneManager.GetActiveScene().name)
                    return level.LevelTitle;
            }

            return string.Empty;
        }

        public void ReturnToMainMenu()
        {
            if (SceneSerializer.Instance != null)
                SceneSerializer.Instance.DeleteCheckpoints();

            LoadScene(MainMenuSceneName);

            //SceneManager.LoadSceneAsync(MainMenuSceneName);
        }

        public void ExitGame()
        {
            if (SceneSerializer.Instance != null)
                SceneSerializer.Instance.DeleteCheckpoints();
            Application.Quit();
        }

        public void StartTutorialLevel()
        {
            LoadScene(TutorialLevel.SceneName);

            //SceneManager.LoadSceneAsync(TutorialLevel.SceneName);
        }

        public void StartLevel(string sceneName)
        {
            foreach (var level in m_levelsDataBase.Levels)
            {
                if (level.SceneName == sceneName)
                {
                    LoadScene(sceneName);
                    return;
                }
            }

            Debug.LogWarning("Not Founded This Scene");
        }

        public void RestartCurrentLevelFromStart()
        {
            if (SceneSerializer.Instance != null)
                SceneSerializer.Instance.DeleteCheckpoints();

            LoadScene(SceneManager.GetActiveScene().name);

            //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

        public void RestartCurrentLevelFromCheckpoint()
        {
            LoadScene(SceneManager.GetActiveScene().name);

            //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

        private void LoadScene(string sceneName)
        {
            if (loadingCoroutine != null) StopAllCoroutines();

            loadingCoroutine = StartCoroutine(LoadSceneAsync(sceneName));
        }

        #region Coroutines

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);

            m_loadingCanvas.SetActive(true);

            m_controlsManager.TurnOffAllControls();

            while(!loadingOperation.isDone)
            {
                var progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);

                m_loadingBarFillImage.fillAmount = progressValue;

                yield return null;
            }

            m_loadingCanvas.SetActive(false);
        }

        #endregion
    }
}
