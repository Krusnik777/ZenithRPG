using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "PrefabsDataBase", menuName = "ScriptableObjects/PrefabsDataBase")]
    public class PrefabsDataBase : ScriptableObject
    {
        public List<GameObject> Prefabs;

        public GameObject CreateEntityFromId(string prefabId)
        {
            foreach (var prefab in Prefabs)
            {
                if (prefab.TryGetComponent(out IDataPersistence dataPersistence))
                {
                    if (dataPersistence.PrefabId == prefabId)
                    {
                        return Instantiate(prefab);
                    }
                }
            }

            return null;
        }
    }
}
