using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class SceneObject
    {
        public long EntityId;
        public string State;

        public SceneObject(long id, string state)
        {
            EntityId = id;
            State = state;
        }
    }

    [System.Serializable]
    public class SceneState
    {
        public int SceneId;
        public bool IsActiveScene;
        // MAP_OPENED_STATE -> here OR to SceneObjects?
        public List<SceneObject> SceneObjects;

        public SceneState(int id, bool isActive)
        {
            SceneId = id;
            IsActiveScene = isActive;
            SceneObjects = new List<SceneObject>();
        }

        public void SaveSceneObjects()
        {
            SceneObjects.Clear(); // Just to be safe

            var dataPersistenceObjects = DataPersistenceManager.Instance.FindAllDataPersistenceObjects();

            foreach (var dataPersistenceObject in dataPersistenceObjects)
            {
                var sceneObject = new SceneObject(dataPersistenceObject.EntityId, dataPersistenceObject.SerializeState());

                SceneObjects.Add(sceneObject);
            }
        }

        public void LoadSceneObjects()
        {
            var dataPersistenceObjects = DataPersistenceManager.Instance.FindAllDataPersistenceObjects();

            foreach (var dataPersistenceObject in dataPersistenceObjects)
            {
                bool isFound = false;

                foreach (var sceneObject in SceneObjects)
                {
                    if (dataPersistenceObject.EntityId == sceneObject.EntityId)
                    {
                        dataPersistenceObject.DeserializeState(sceneObject.State);
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    Debug.Log("Not serialized object is found");
                    // Destroy dataPersistenceObject.gameObject SOMEHOW
                }
            }
        }
    }

    [System.Serializable]
    public class GameData
    {
        

        // params
        public List<SceneState> SavedScenes;
        // PlayerProgress : Stats, Inventory

        public GameData()
        {
            SavedScenes = new List<SceneState>();
        }

        public void Save(int currentSceneId, bool isActive)
        {
            bool notExist = true;

            foreach (var scene in SavedScenes)
            {
                if (scene.SceneId == currentSceneId)
                {
                    scene.IsActiveScene = isActive;
                    scene.SaveSceneObjects();
                    notExist = false;
                }
            }

            if (notExist)
            {
                var sceneForSave = new SceneState(currentSceneId, isActive);
                sceneForSave.SaveSceneObjects();
                SavedScenes.Add(sceneForSave);
            }

            // Save Player Progress
        }

        public void Load()
        {
            foreach(var scene in SavedScenes)
            {
                if (scene.IsActiveScene)
                {
                    scene.LoadSceneObjects();
                }
            }

            // Load Player Progress
        }

        
    }
}
