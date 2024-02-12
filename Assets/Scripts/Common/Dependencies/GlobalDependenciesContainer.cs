using UnityEngine;
using UnityEngine.SceneManagement;

namespace DC_ARPG
{
    public class GlobalDependenciesContainer : Dependency
    {
        //[SerializeField] private Pauser m_pauser;
        //[SerializeField] private GameCompletion m_gameCompletion;
        [SerializeField] private SettingLoader m_settingLoader;
        [SerializeField] private ControlsManager m_controlsManager;

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
            //Bind<Pauser>(m_pauser, monoBehaviourInScene);
            //Bind<GameCompletion>(m_gameCompletion, monoBehaviourInScene);
            Bind<SettingLoader>(m_settingLoader, monoBehaviourInScene);
            Bind<ControlsManager>(m_controlsManager, monoBehaviourInScene);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            FindAllObjectsToBind();
        }
    }
}
