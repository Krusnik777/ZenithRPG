using UnityEngine;
using UnityEngine.SceneManagement;

namespace DC_ARPG
{
    public class GlobalDependenciesContainer : Dependency
    {
        [SerializeField] private SettingLoader m_settingLoader;
        [SerializeField] private ControlsManager m_controlsManager;
        [SerializeField] private MusicCommander m_musicCommander;
        [SerializeField] private SceneCommander m_sceneCommander;
        [SerializeField] private DataPersistenceManager m_dataPersistenceManager;

        private static GlobalDependenciesContainer instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected override void BindAll(MonoBehaviour monoBehaviourInScene)
        {
            Bind<SettingLoader>(m_settingLoader, monoBehaviourInScene);
            Bind<ControlsManager>(m_controlsManager, monoBehaviourInScene);
            Bind<MusicCommander>(m_musicCommander, monoBehaviourInScene);
            Bind<SceneCommander>(m_sceneCommander, monoBehaviourInScene);
            Bind<DataPersistenceManager>(m_dataPersistenceManager, monoBehaviourInScene);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            FindAllObjectsToBind();
        }
    }
}
