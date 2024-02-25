using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DC_ARPG
{
    public class DataPersistenceManager : MonoSingleton<DataPersistenceManager>
    {
        [Header("File Storage Config")]
        [SerializeField] private string m_fileName;

        private FileDataHandler m_dataHandler;

        private GameData m_gameData;

        public List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            var dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public void NewGame()
        {
            m_gameData = new GameData();
        }

        public void LoadGame()
        {
            m_gameData = m_dataHandler.Load();

            if (m_gameData == null)
            {
                NewGame();
                return;
            }

            m_gameData.Load();
        }

        public void SaveGame()
        {
            m_gameData.Save(SceneManager.GetActiveScene().buildIndex, true); // TEMP

            m_dataHandler.Save(m_gameData);
        }

        private void Start()
        {
            m_dataHandler = new FileDataHandler(Application.persistentDataPath, m_fileName);
            // LoadGame();
        }
    }
}
