using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class SceneObject
    {
        public string PrefabId;
        public string EntityId;
        public bool IsCreated;
        public string State;

        public SceneObject(string prefabId, string entityId, string state, bool isCreated)
        {
            PrefabId = prefabId;
            EntityId = entityId;
            State = state;
            IsCreated = isCreated;
        }
    }

    [System.Serializable]
    public class SceneState
    {
        public int SceneId;
        // MAP_OPENED_STATE -> here OR to SceneObjects?
        public List<SceneObject> SceneObjects;

        public SceneState(int id)
        {
            SceneId = id;
            SceneObjects = new List<SceneObject>();
        }

        public void SaveSceneObjects()
        {
            SceneObjects.Clear(); // Just to be safe

            var dataPersistenceObjects = DataPersistenceManager.Instance.FindAllDataPersistenceObjects();

            foreach (var dataPersistenceObject in dataPersistenceObjects)
            {
                if (dataPersistenceObject.IsSerializable())
                {
                    var sceneObject = new SceneObject(dataPersistenceObject.PrefabId, dataPersistenceObject.EntityId, dataPersistenceObject.SerializeState(), dataPersistenceObject.IsCreated);

                    SceneObjects.Add(sceneObject);
                }
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
                        if (dataPersistenceObject.IsSerializable())
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
        // ALL TEMP

        // params
        public long LastUpdated;
        public SceneState ActiveScene;
        public List<SceneState> SavedScenes;
        // PlayerProgress : Stats, Inventory
        // PlayTime

        public GameData()
        {
            SavedScenes = new List<SceneState>();
        }

        public void Save(int currentSceneId)
        {
            bool notExist = true;

            foreach (var scene in SavedScenes)
            {
                if (scene.SceneId == currentSceneId)
                {
                    scene.SaveSceneObjects();
                    notExist = false;
                }
            }

            if (notExist)
            {
                var sceneForSave = new SceneState(currentSceneId);
                sceneForSave.SaveSceneObjects();
                SavedScenes.Add(sceneForSave);
            }

            // Save Player Progress
        }

        public void Load()
        {
            ActiveScene.LoadSceneObjects();

            // Load Player Progress
        }

        
    }
}