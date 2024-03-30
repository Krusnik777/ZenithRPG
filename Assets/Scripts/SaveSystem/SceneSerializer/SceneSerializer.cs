using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DC_ARPG
{
    public class SceneSerializer : MonoSingleton<SceneSerializer>
    {
        [Header("Prefabs DataBase")]
        [SerializeField] private PrefabsDataBase m_prefabsDataBase;
        [Header("File Storage Config")]
        [SerializeField] private string m_fileName;
        [SerializeField] private bool m_useEncryption;

        public event UnityAction EventOnSaved;

        public static bool LoadingFromCheckpoint { get; set; }

        private FileDataHandler<SceneData> m_dataHandler;

        private SceneData m_sceneData;

        public bool CheckSaveExists() => m_dataHandler.CheckIfSaveFileExist();

        public void DeleteCheckpoints()
        {
            m_dataHandler.Delete();
        }

        public void SaveSceneData()
        {
            if (m_sceneData == null) m_sceneData = new SceneData();

            m_sceneData.SceneObjects.Clear();

            foreach (var dataPersistenceObject in FindAllDataPersistenceObjects())
            {
                if (!dataPersistenceObject.IsSerializable()) continue;

                var sceneObject = new SceneObject(dataPersistenceObject.PrefabId, dataPersistenceObject.EntityId, dataPersistenceObject.SerializeState(), dataPersistenceObject.IsCreated);

                m_sceneData.SceneObjects.Add(sceneObject);

                if (dataPersistenceObject is Player)
                {
                    var playerCharacter = (dataPersistenceObject as Player).Character;
                    m_sceneData.PlayerData = new PlayerData(playerCharacter.PlayerStats, playerCharacter.Inventory, playerCharacter.Money);
                }
            }

            m_dataHandler.Save(m_sceneData);

            EventOnSaved?.Invoke();
        }

        public void LoadSceneData()
        {
            m_sceneData = m_dataHandler.Load();

            if (m_sceneData == null)
            {
                m_sceneData = new SceneData();
                return;
            }

            foreach (var dataPersistenceObject in FindAllDataPersistenceObjects())
            {
                if (!dataPersistenceObject.IsSerializable()) continue;

                bool isFound = false;

                foreach (var loadedObject in m_sceneData.SceneObjects)
                {
                    if (dataPersistenceObject.EntityId == loadedObject.EntityId)
                    {
                        dataPersistenceObject.DeserializeState(loadedObject.State);
                        isFound = true;

                        if (dataPersistenceObject is Player)
                        {
                            var playerCharacter = (dataPersistenceObject as Player).Character;
                            playerCharacter.UpdatePlayerCharacter(m_sceneData.PlayerData);
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

            foreach (var loadedObject in m_sceneData.SceneObjects)
            {
                if (loadedObject.IsCreated)
                {
                    Debug.Log("Found created object => " + loadedObject.EntityId);
                    GameObject createdObject = m_prefabsDataBase.CreateEntityFromId(loadedObject.PrefabId);
                    createdObject.GetComponent<IDataPersistence>().SetupCreatedDataPersistenceObject(loadedObject.EntityId, loadedObject.IsCreated, loadedObject.State);
                }
            }
        }

        public List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            var dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        protected override void Awake()
        {
            base.Awake();

            m_dataHandler = new FileDataHandler<SceneData>(Application.persistentDataPath, m_fileName, m_useEncryption);
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
            if (LoadingFromCheckpoint)
            {
                LoadSceneData();
                LoadingFromCheckpoint = false;
            }
        }

        private void OnApplicationQuit()
        {
            DeleteCheckpoints();
        }
    }
}
