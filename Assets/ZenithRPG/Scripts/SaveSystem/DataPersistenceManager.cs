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
        private GameData m_tempData;

        private string m_selectedProfileId = "";
        private string m_tempDataProfileId = "TempData";

        private bool m_loadingFromSaveFile;

        public GameData GetGameDataByProfile(string profileId) => m_dataHandler.Load(profileId);

        public bool CheckSaveForCurrentProfileExists() => m_dataHandler.CheckIfSaveFileForProfileExist(m_selectedProfileId);
        public bool CheckTempSaveExists() => m_dataHandler.CheckIfSaveFileForProfileExist(m_tempDataProfileId);

        public void ChangeSelectedProfileId(string newProfileId) => m_selectedProfileId = newProfileId;

        public void DeleteProfileData(string profileId) => m_dataHandler.Delete(profileId);

        public Dictionary<string, GameData> GetAllProfilesGameData() => m_dataHandler.LoadAllProfiles();

        public List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            var dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public bool CheckAnySaveFilesExists()
        {
            var profilesGameData = GetAllProfilesGameData();

            foreach (var profileData in profilesGameData)
            {
                if (profileData.Key != m_tempDataProfileId) return true;
            }

            return false;
        }

        public void SaveGame()
        {
            if (m_tempData == null)
            {
                Debug.LogError("No temp data was found");
                return;
            }

            if (m_gameData == null) m_gameData = new GameData();

            m_gameData.SavedSceneStates.Clear();
            m_gameData.SavedSceneStates.AddRange(m_tempData.SavedSceneStates);

            m_gameData.SetActiveSceneState(SceneManager.GetActiveScene().name);

            m_gameData.ActiveSceneState.SceneObjects.Clear();

            foreach (var dataPersistenceObject in FindAllDataPersistenceObjects())
            {
                if (!dataPersistenceObject.IsSerializable()) continue;

                if (dataPersistenceObject is Player)
                {
                    (dataPersistenceObject as Player).ResetCheckpointPosition();
                    var playerCharacter = (dataPersistenceObject as Player).Character as PlayerCharacter;
                    m_gameData.PlayerData = new PlayerData(playerCharacter.Stats as PlayerStats, playerCharacter.Inventory, playerCharacter.Money);
                }

                var sceneObject = new SceneObject(dataPersistenceObject.PrefabId, dataPersistenceObject.EntityId, dataPersistenceObject.SerializeState(), dataPersistenceObject.IsCreated);

                m_gameData.ActiveSceneState.SceneObjects.Add(sceneObject);

                if (dataPersistenceObject is MinimapIconCollector)
                {
                    var minimapIconCollector = dataPersistenceObject as MinimapIconCollector;
                    m_gameData.ActiveSceneState.MapCompletion = minimapIconCollector.GetMapCompletionPercent();
                }
            }

            m_gameData.LastUpdated = System.DateTime.Now.ToBinary();

            m_gameData.PlayTime = m_tempData.PlayTime + Time.timeSinceLevelLoadAsDouble;

            m_dataHandler.Save(m_gameData, m_selectedProfileId);
        }

        public void LoadGame()
        {
            m_gameData = m_dataHandler.Load(m_selectedProfileId);

            if (m_gameData == null) return;

            m_loadingFromSaveFile = true;

            SceneCommander.Instance.StartLevel(m_gameData.ActiveSceneName);
        }

        public void SaveTempData()
        {
            if (m_tempData == null)
            {
                Debug.LogWarning("No temp data was found");
                return;
            }

            m_tempData.SetActiveSceneState(SceneManager.GetActiveScene().name);

            //m_tempData.ActiveSceneState.SceneObjects.Clear();
            m_tempData.ActiveSceneState.SceneObjectsAtStart.Clear();

            foreach (var dataPersistenceObject in FindAllDataPersistenceObjects())
            {
                if (!dataPersistenceObject.IsSerializable()) continue;

                if (dataPersistenceObject is Player)
                {
                    (dataPersistenceObject as Player).ResetCheckpointPosition();
                    var playerCharacter = (dataPersistenceObject as Player).Character as PlayerCharacter;
                    m_tempData.PlayerData = new PlayerData(playerCharacter.Stats as PlayerStats, playerCharacter.Inventory, playerCharacter.Money);
                    m_tempData.ActiveSceneState.PlayerDataAtStart = new PlayerData(playerCharacter.Stats as PlayerStats, playerCharacter.Inventory, playerCharacter.Money);
                }

                var sceneObject = new SceneObject(dataPersistenceObject.PrefabId, dataPersistenceObject.EntityId, dataPersistenceObject.SerializeState(), dataPersistenceObject.IsCreated);

                //m_tempData.ActiveSceneState.SceneObjects.Add(sceneObject);
                m_tempData.ActiveSceneState.SceneObjectsAtStart.Add(sceneObject);

                if (dataPersistenceObject is MinimapIconCollector)
                {
                    var minimapIconCollector = dataPersistenceObject as MinimapIconCollector;
                    m_tempData.ActiveSceneState.MapCompletion = minimapIconCollector.GetMapCompletionPercent();
                }
            }

            m_tempData.LastUpdated = System.DateTime.Now.ToBinary();

            m_tempData.PlayTime += Time.timeSinceLevelLoadAsDouble;

            m_dataHandler.Save(m_tempData, m_tempDataProfileId);
        }

        public void LoadPlayerDataFromTempData()
        {
            var player = FindObjectOfType<Player>();
            (player.Character as PlayerCharacter).UpdatePlayerCharacter(m_tempData.PlayerData);
            m_tempData.ActiveSceneState.PlayerDataAtStart = new PlayerData(player.Character.Stats as PlayerStats, (player.Character as PlayerCharacter).Inventory, (player.Character as PlayerCharacter).Money);
            if (Exit.ChangedLevels) player.transform.forward = -player.transform.forward;
        }

        protected override void Awake()
        {
            base.Awake();

            m_dataHandler = new FileDataHandler<GameData>(Application.persistentDataPath, m_fileName, m_useEncryption);

            //InitializeSelectedProfileId(); // For Continue From Last Save
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
            if (scene.name == SceneCommander.MainMenuSceneName || scene.name == SceneCommander.Instance.TutorialLevel.SceneName)
            {
                m_dataHandler.Delete(m_tempDataProfileId); // delete tempData if it for some reason exist
                return;
            }

            if (SceneSerializer.LoadingFromCheckpoint) return;

            if (!m_loadingFromSaveFile)
            {
                if (CheckTempSaveExists())
                {
                    if (m_tempData.ContainsScene(SceneManager.GetActiveScene().name))
                    {
                        LoadSceneObjectsFromTempData();
                        if (Exit.ChangedLevels)
                        {
                            LoadPlayerDataFromTempData();
                            Exit.ChangedLevels = false;
                        }
                    }
                    else
                    {
                        if (Exit.ChangedLevels) Exit.ChangedLevels = false;
                        LoadPlayerDataFromTempData();
                        SaveTempData();
                    }
                }
                else
                {
                    m_tempData = new GameData();
                    SaveTempData();
                }
            }
            else
            {
                LoadSceneObjects();

                CreateTempDataFromLoadedFile();

                m_loadingFromSaveFile = false;
            }
        }

        private void OnApplicationQuit()
        {
            m_dataHandler.Delete(m_tempDataProfileId); // delete tempData if it for some reason exist
        }

        private void InitializeSelectedProfileId()
        {
            m_selectedProfileId = m_dataHandler.GetMostRecentlyUpdatedProfileId();
        }

        private void LoadSceneObjects()
        {
            m_gameData.SetActiveSceneState(SceneManager.GetActiveScene().name);

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
                            var playerCharacter = ((dataPersistenceObject as Player).Character as PlayerCharacter);
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

        private void LoadSceneObjectsFromTempData()
        {
            m_tempData.SetActiveSceneState(SceneManager.GetActiveScene().name);

            foreach (var dataPersistenceObject in FindAllDataPersistenceObjects())
            {
                if (!dataPersistenceObject.IsSerializable()) continue;

                bool isFound = false;

                foreach (var loadedObject in m_tempData.ActiveSceneState.SceneObjectsAtStart)
                {
                    if (dataPersistenceObject.EntityId == loadedObject.EntityId)
                    {
                        dataPersistenceObject.DeserializeState(loadedObject.State);
                        isFound = true;

                        if (dataPersistenceObject is Player)
                        {
                            var playerCharacter = (dataPersistenceObject as Player).Character as PlayerCharacter;
                            playerCharacter.UpdatePlayerCharacter(m_tempData.ActiveSceneState.PlayerDataAtStart);
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

            // Find saved objects which were created in last game session but hadn't existed at pure start

            foreach (var loadedObject in m_tempData.ActiveSceneState.SceneObjectsAtStart)
            {
                if (loadedObject.IsCreated)
                {
                    Debug.Log("Found created object => " + loadedObject.EntityId);
                    GameObject createdObject = m_prefabsDataBase.CreateEntityFromId(loadedObject.PrefabId);
                    createdObject.GetComponent<IDataPersistence>().SetupCreatedDataPersistenceObject(loadedObject.EntityId, loadedObject.IsCreated, loadedObject.State);
                }
            }
        }

        private void CreateTempDataFromLoadedFile()
        {
            m_tempData = new GameData();

            m_tempData.SavedSceneStates.Clear();
            m_tempData.SavedSceneStates.AddRange(m_gameData.SavedSceneStates);
            m_tempData.PlayerData = m_gameData.PlayerData;

            m_tempData.SetActiveSceneState(SceneManager.GetActiveScene().name);

            m_tempData.LastUpdated = System.DateTime.Now.ToBinary();

            m_tempData.PlayTime = m_gameData.PlayTime;

            m_dataHandler.Save(m_tempData, m_tempDataProfileId);
        }
    }
}
