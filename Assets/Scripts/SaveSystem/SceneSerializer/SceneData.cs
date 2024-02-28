using System.Collections.Generic;

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
    public class SceneData
    {
        public List<SceneObject> SceneObjects;
        public PlayerData PlayerData;

        public SceneData()
        {
            SceneObjects = new List<SceneObject>();
            PlayerData = new PlayerData();
        }
    }
}
