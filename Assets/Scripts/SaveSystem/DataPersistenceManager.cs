using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DC_ARPG
{
    public class DataPersistenceManager : MonoSingleton<DataPersistenceManager>
    {
        [Header("Prefabs DataBase")]
        [SerializeField] private PrefabsDataBase m_prefabsDataBase;
        [Header("File Storage Config")]
        [SerializeField] private string m_fileName;
        [SerializeField] private bool m_useEncryption;

        private FileDataHandler<GameData> m_dataHandler;

        private GameData m_gameData;

        private string m_selectedProfileId = "";

        public bool CheckSaveForCurrentProfileExists() => m_dataHandler.CheckIfSaveFileForProfileExist(m_selectedProfileId);

        public void ChangeSelectedProfileId(string newProfileId)
        {
            m_selectedProfileId = newProfileId;

            LoadGame();
        }

        public Dictionary<string, GameData> GetAllProfilesGameData() => m_dataHandler.LoadAllProfiles();

        public List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            var dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public void DeleteProfileData(string profileId)
        {
            m_dataHandler.Delete(profileId);

            InitializeSelectedProfileId();

            LoadGame();
        }

        public void NewGame()
        {
            m_gameData = new GameData();
        }
        public void SaveGame()
        {
            if (m_gameData == null)
            {
                Debug.LogWarning("No data was found");
                return;
            }

            m_gameData.SetActiveSceneState(SceneManager.GetActiveScene().name);

            m_gameData.ActiveSceneState.SceneObjects.Clear();

            foreach (var dataPersistenceObject in FindAllDataPersistenceObjects())
            {
                if (!dataPersistenceObject.IsSerializable()) continue;

                var sceneObject = new SceneObject(dataPersistenceObject.PrefabId, dataPersistenceObject.EntityId, dataPersistenceObject.SerializeState(), dataPersistenceObject.IsCreated);

                m_gameData.ActiveSceneState.SceneObjects.Add(sceneObject);

                if (dataPersistenceObject is Player)
                {
                    var playerCharacter = (dataPersistenceObject as Player).Character;
                    m_gameData.PlayerData = new PlayerData(playerCharacter.PlayerStats, playerCharacter.Inventory, playerCharacter.Money);
                }

                if (dataPersistenceObject is MinimapIconCollector)
                {
                    var minimapIconCollector = dataPersistenceObject as MinimapIconCollector;
                    m_gameData.ActiveSceneState.MapCompletion = minimapIconCollector.GetMapCompletionPercent();
                }
            }

            m_gameData.LastUpdated = System.DateTime.Now.ToBinary();

            m_dataHandler.Save(m_gameData, m_selectedProfileId);
        }

        public void LoadGame()
        {
            m_gameData = m_dataHandler.Load(m_selectedProfileId);

            if (m_gameData == null)
            {
                // NewGame();
                return;
            }

            SceneCommander.Instance.StartLevel(m_gameData.ActiveSceneState.SceneId);

            // Must be After Scene Loaded

            foreach (var dataPersistenceObject in FindAllDataPersistenceObjects())
            {
                if (!dataPersistenceObject.IsSerializable()) continue;

                bool isFound = false;

                foreach (var loadedObject in m_gameData.ActiveSceneState.SceneObjects)
                {
                    if (dataPersistenceObject.EntityId == loadedObject.EntityId)
                    {
                        dataPersistenceObject.DeserializeState(loadedObject.State);
                        isFound = true;

                        if (dataPersistenceObject is Player)
                        {
                            var playerCharacter = (dataPersistenceObject as Player).Character;
                            playerCharacter.UpdatePlayerCharacter(m_gameData.PlayerData);
                        }

                        break;
                    }
                }

                if (!isFound)
                {
                    //Debug.Log("Not serialized object is found: ID - " + dataPersistenceObject.EntityId + " -> Destroying Object");
                    Destroy((dataPersistenceObject as MonoBehaviour).gameObject);
                }
            }

            // Find saved objects which were created in last game session but hadn't existed at start

            foreach (var loadedObject in m_gameData.ActiveSceneState.SceneObjects)
            {
                if (loadedObject.IsCreated)
                {
                    Debug.Log("Found created object => " + loadedObject.EntityId);
                    GameObject createdObject = m_prefabsDataBase.CreateEntityFromId(loadedObject.PrefabId);
                    createdObject.GetComponent<IDataPersistence>().SetupCreatedDataPersistenceObject(loadedObject.EntityId, loadedObject.IsCreated, loadedObject.State);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();

            m_dataHandler = new FileDataHandler<GameData>(Application.persistentDataPath, m_fileName, m_useEncryption);

            InitializeSelectedProfileId();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Debug.Log("OnSceneLoaded");
            // LoadGame();
        }

        private void InitializeSelectedProfileId()
        {
            m_selectedProfileId = m_dataHandler.GetMostRecentlyUpdatedProfileId();
        }
    }
}
