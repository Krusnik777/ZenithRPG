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

        public void PlayGameOverMusic()
        {
            m_audioSource.clip = m_musicDataBase.GameOverTheme;
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
            PlayCurrentSceneBGM();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlaySceneBGM(scene.name);
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
