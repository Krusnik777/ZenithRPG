using UnityEngine;
using UnityEngine.SceneManagement;

namespace DC_ARPG
{
    public class SceneCommander : MonoSingleton<SceneCommander>
    {
        public static string MainMenuSceneName = "MainMenu";

        [SerializeField] private LevelsDataBase m_levelsDataBase;
        public LevelData TutorialLevel => m_levelsDataBase.TutorialLevel;
        public LevelData[] Levels => m_levelsDataBase.Levels;

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
            SceneManager.LoadSceneAsync(MainMenuSceneName);
        }

        public void ExitGame()
        {
            if (SceneSerializer.Instance != null)
                SceneSerializer.Instance.DeleteCheckpoints();
            Application.Quit();
        }

        public void StartTutorialLevel()
        {
            SceneManager.LoadSceneAsync(TutorialLevel.SceneName);
        }

        
    }
}
