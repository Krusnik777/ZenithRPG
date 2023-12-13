using UnityEngine.SceneManagement;

namespace DC_ARPG
{
    public class SceneLoader
    {
        private const string MainMenuSceneName = "MainMenu";
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenuSceneName);
        }

        public static void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
