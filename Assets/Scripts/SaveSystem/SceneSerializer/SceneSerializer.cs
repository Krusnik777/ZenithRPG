using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DC_ARPG
{
    [System.Serializable]
    public class SceneData
    {
        public List<SceneObject> SceneObjects;
        //public PlayerStats PlayerStats;
        //public Inventory PlayerInventory;

        public SceneData()
        {
            SceneObjects = new List<SceneObject>();
            //PlayerStats = playerStats;
            //PlayerInventory = inventory;
        }
    }

    public class SceneSerializer : MonoSingleton<SceneSerializer>
    {
        [Header("File Storage Config")]
        [SerializeField] private string m_fileName;
        [SerializeField] private bool m_useEncryption;

        private FileDataHandler<SceneData> m_dataHandler;

        private SceneData m_sceneData;

        public void SaveSceneData()
        {
            m_sceneData.SceneObjects.Clear();

            foreach (var dataPersistenceObject in FindAllDataPersistenceObjects())
            {
                var sceneObject = new SceneObject(dataPersistenceObject.EntityId, dataPersistenceObject.SerializeState());

                m_sceneData.SceneObjects.Add(sceneObject);
            }

            m_dataHandler.Save(m_sceneData);

            Debug.Log("Saved");
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
                bool isFound = false;

                foreach (var loadedObject in m_sceneData.SceneObjects)
                {
                    if (dataPersistenceObject.EntityId == loadedObject.EntityId)
                    {
                        dataPersistenceObject.DeserializeState(loadedObject.State);
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    Debug.Log("Not serialized object is found: ID - " + dataPersistenceObject.EntityId + " -> Destroying Object");
                    Destroy((dataPersistenceObject as MonoBehaviour).gameObject);
                }
            }
        }

        public List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            var dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        private void Start()
        {
            m_dataHandler = new FileDataHandler<SceneData>(Application.persistentDataPath, m_fileName, m_useEncryption);
            LoadSceneData();
        }

        // TEMP

        public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        public void Save() => SaveSceneData();
    }
}
