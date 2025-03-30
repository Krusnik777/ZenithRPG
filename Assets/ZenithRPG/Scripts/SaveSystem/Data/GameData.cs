using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class SceneState
    {
        public string SceneId;
        public float MapCompletion;
        public List<SceneObject> SceneObjects;
        public List<SceneObject> SceneObjectsAtStart;
        public PlayerData PlayerDataAtStart;

        public SceneState(string id)
        {
            SceneId = id;
            SceneObjects = new List<SceneObject>();
            SceneObjectsAtStart = new List<SceneObject>();
            PlayerDataAtStart = new PlayerData();
        }
    }

    [System.Serializable]
    public class GameData
    {
        // params
        public long LastUpdated;
        public string ActiveSceneName;
        public List<SceneState> SavedSceneStates;
        public PlayerData PlayerData;
        public double PlayTime;

        public SceneState ActiveSceneState { get; private set; }

        public GameData()
        {
            SavedSceneStates = new List<SceneState>();
            PlayerData = new PlayerData();
            PlayTime = 0;
        }

        public bool ContainsScene(string sceneId)
        {
            foreach (var sceneState in SavedSceneStates)
            {
                if (sceneState.SceneId == sceneId) return true;
            }

            return false;
        }

        public void SetActiveSceneState(string sceneId)
        {
            ActiveSceneName = sceneId;

            foreach (var sceneState in SavedSceneStates)
            {
                if (sceneState.SceneId == sceneId)
                {
                    ActiveSceneState = sceneState;
                    return;
                }
            }

            ActiveSceneState = new SceneState(sceneId);

            SavedSceneStates.Add(ActiveSceneState);
        }
    }
}
