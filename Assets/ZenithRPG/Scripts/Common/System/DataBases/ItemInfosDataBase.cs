using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "ItemInfosDataBase", menuName = "ScriptableObjects/ItemInfosDataBase")]
    public class ItemInfosDataBase : ScriptableObject
    {
        private static ItemInfosDataBase _instance;

        public static ItemInfosDataBase Instance
        {
            get
            {
                if (_instance == null) _instance = Resources.Load<ItemInfosDataBase>("ItemInfosDataBase");

                return _instance;
            }
        }

        public List<ItemInfo> ItemInfos;

        public ItemInfo GetItemInfoFromId(string infoId)
        {
            foreach (var info in ItemInfos)
            {
               if (info.ID == infoId) return info;
            }

            return null;
        }
    }
}
