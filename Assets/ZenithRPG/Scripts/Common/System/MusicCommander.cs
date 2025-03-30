using UnityEngine;
using UnityEngine.SceneManagement;

namespace DC_ARPG
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicCommander : MonoSingleton<MusicCommander>
    {
        [SerializeField] private MusicDataBase m_musicDataBase;

        public void PlayCurrentSceneBGM()
        {
            PlaySceneBGM(SceneManager.GetActiveScene().name);
        }

        public void PlayMainMenuTheme()
        {
            m_audioSource.clip = m_musicDataBase.MainMenuTheme;
            m_audioSource.Play();
        }

        public void PlayGameOverMusic()
        {
            m_audioSource.clip = m_musicDataBase.GameOverTheme;
            m_audioSource.Play();
        }

        public void PlayShopTheme()
        {
            m_audioSource.clip = m_musicDataBase.ShopTheme;
            m_audioSource.Play();
        }

        private AudioSource m_audioSource;

        protected override void Awake()
        {
            base.Awake();

            m_audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name != SceneCommander.MainMenuSceneName)
                PlayCurrentSceneBGM();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (SceneManager.GetActiveScene().name != SceneCommander.MainMenuSceneName)
                PlaySceneBGM(scene.name);
            else m_audioSource.Stop();
        }

        private void PlaySceneBGM(string sceneName)
        {
            foreach (var levelBGM in m_musicDataBase.LevelsBGM)
            {
                if (levelBGM.SceneName == sceneName)
                    m_audioSource.clip = levelBGM.BGM;
            }
            m_audioSource.Play();
        }
    }
}
