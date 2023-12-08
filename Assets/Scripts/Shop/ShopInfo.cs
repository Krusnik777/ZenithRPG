using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class Shopkeeper
    {
        [System.Serializable]
        public class Speech
        {
            public string[] Lines;
            /// <summary>
            /// Speak only single time.
            /// </summary>
            public bool Removable;
        }

        public List<Speech> TalkSpeeches;
        public List<string> WelcomeLines;
        public List<string> FarewellLines;
        public List<string> PurchaseLines;
        public List<string> NotEnoughMoneyLines;
        public List<string> NoPlaceLines;
    }

    [CreateAssetMenu(fileName = "ShopInfo", menuName = "ScriptableObjects/ShopInfo")]
    public class ShopInfo : ScriptableObject
    {
        public Shopkeeper Shopkeeper;
        public ItemData[] ShopItemsData;
    }
}
