using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DC_ARPG
{
    public class FileDataHandler<T>
    {
        private string m_dataDirPath = "";
        private string m_dataFileName = "";

        private bool m_useEncryption = false;
        private readonly string encryptionCodeWord = "word";
        private readonly string backupExtension = ".bak";

        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            m_dataDirPath = dataDirPath;
            m_dataFileName = dataFileName;
            m_useEncryption = useEncryption;
        }

        public bool CheckIfSaveFileExist()
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);

            return File.Exists(fullPath);
        }

        public bool CheckIfSaveFileForProfileExist(string profileId)
        {
            if (profileId == null) return false;

            string fullPath = Path.Combine(m_dataDirPath, profileId, m_dataFileName);

            return File.Exists(fullPath);
        }

        public Dictionary<string, T> LoadAllProfiles()
        {
            Dictionary<string, T> profileDictionary = new Dictionary<string, T>();

            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(m_dataDirPath).EnumerateDirectories();
            foreach(var dirInfo in dirInfos)
            {
                string profileId = dirInfo.Name;

                string fullPath = Path.Combine(m_dataDirPath, profileId, m_dataFileName);
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning("Skipping directory when loading all profiles because it does not contain the data: " + profileId);
                    continue;
                }

                T profileData = Load(profileId);

                if (profileData != null)
                {
                    profileDictionary.Add(profileId, profileData);
                }
                else
                {
                    Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
                }
            }

            return profileDictionary;
        }

        public string GetMostRecentlyUpdatedProfileId()
        {
            string mostRecentProfileId = null;

            var profilesDictionary = LoadAllProfiles();

            foreach(var profileData in profilesDictionary)
            {
                string profileId = profileData.Key;
                T saveData = profileData.Value;

                if (saveData == null)
                {
                    continue;
                }

                if (mostRecentProfileId == null)
                {
                    mostRecentProfileId = profileId;
                }
                else
                {
                    if (saveData is GameData)
                    {
                        DateTime mostRecentDateTime = DateTime.FromBinary((profilesDictionary[mostRecentProfileId] as GameData).LastUpdated);
                        DateTime newDateTime = DateTime.FromBinary((saveData as GameData).LastUpdated);

                        if (newDateTime > mostRecentDateTime) mostRecentProfileId = profileId;
                    }
                }
            }

            return mostRecentProfileId;
        }

        public void Delete()
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);

            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                else
                {
                    Debug.Log("Tried to delete data, but data was not found at path: " + fullPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to delete data at path: " + fullPath + "\n" + e);
            }
        }

        public void Delete(string profileId)
        {
            if (profileId == null) return;

            string fullPath = Path.Combine(m_dataDirPath, profileId, m_dataFileName);

            try
            {
                if (File.Exists(fullPath))
                {
                    Directory.Delete(Path.GetDirectoryName(fullPath), true);
                }
                else
                {
                    Debug.Log("Tried to delete data, but data was not found at path: " + fullPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to delete data for profile id: " + profileId + " at path: " + fullPath + "\n" + e);
            }
        }

        public void Save(T saveData)
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);
            string backupFilePath = fullPath + backupExtension;

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(saveData, true);

                if (m_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }

                T verifiedSaveData = Load();

                if (verifiedSaveData != null)
                {
                    File.Copy(fullPath, backupFilePath, true);
                }
                else
                {
                    throw new Exception("Save file could not be verified and backup could not be created.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        public void Save(T saveData, string profileId)
        {
            if (profileId == null) return;

            string fullPath = Path.Combine(m_dataDirPath, profileId, m_dataFileName);
            string backupFilePath = fullPath + backupExtension;

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(saveData, true);

                if (m_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }

                T verifiedSaveData = Load(profileId);

                if (verifiedSaveData != null)
                {
                    File.Copy(fullPath, backupFilePath, true);
                }
                else
                {
                    throw new Exception("Save file could not be verified and backup could not be created.");
                }

            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        public T Load(bool allowRestoreFromBackup = true)
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);

            T loadedData = default(T);

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (m_useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<T>(dataToLoad);
                }
                catch (Exception e)
                {
                    if (allowRestoreFromBackup)
                    {
                        Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);

                        if (AttemptRollback(fullPath))
                        {
                            loadedData = Load(false);
                        }
                    }
                    else
                    {
                        Debug.LogError("Error occured when trying to load data at path: " + fullPath + " and backup dod not work.\n" + e);
                    }
                }
            }

            return loadedData;
        }

        public T Load(string profileId, bool allowRestoreFromBackup = true)
        {
            if (profileId == null) return default(T);

            string fullPath = Path.Combine(m_dataDirPath, profileId, m_dataFileName);

            T loadedData = default(T);

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (m_useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<T>(dataToLoad);
                }
                catch (Exception e)
                {
                    if (allowRestoreFromBackup)
                    {
                        Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);

                        if (AttemptRollback(fullPath))
                        {
                            loadedData = Load(profileId, false);
                        }
                    }
                    else
                    {
                        Debug.LogError("Error occured when trying to load data at path: " + fullPath + " and backup dod not work.\n" + e);
                    }

                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }

            return loadedData;
        }

        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";

            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }

            return modifiedData;
        }

        private bool AttemptRollback(string fullPath)
        {
            bool success = false;

            string backupFilePath = fullPath + backupExtension;

            try
            {
                if (File.Exists(backupFilePath))
                {
                    File.Copy(backupFilePath, fullPath, true);
                    success = true;
                    Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
                }
                else
                {
                    throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when attempting to roll back to backup file at: " + backupFilePath + "\n" + e);
            }

            return success;
        }


        #region OBSOLETE
        /*
        public GameData Load()
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);

            GameData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (m_useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }

            return loadedData;
        }

        public void Save(GameData gameData)
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(gameData, true);

                if (m_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        public SceneData LoadSceneData()
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);

            SceneData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (m_useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<SceneData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }

            return loadedData;
        }

        public void SaveSceneData(SceneData sceneData)
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(sceneData, true);

                if (m_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }*/

        #endregion
    }
}
