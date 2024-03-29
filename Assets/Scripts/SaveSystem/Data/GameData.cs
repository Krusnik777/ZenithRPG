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
        public PlayerData PlayerDataAtStart;

        public SceneState(string id)
        {
            SceneId = id;
            SceneObjects = new List<SceneObject>();
        }
    }

    [System.Serializable]
    public class GameData
    {
        // params
        public long LastUpdated;
        public List<SceneState> SavedSceneStates;
        public PlayerData PlayerData;
        // PlayTime

        public SceneState ActiveSceneState { get; private set; }

        public GameData()
        {
            SavedSceneStates = new List<SceneState>();
            PlayerData = new PlayerData();
        }

        public void SetActiveSceneState(string sceneId)
        {
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
