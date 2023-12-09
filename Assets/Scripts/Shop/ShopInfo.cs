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
            [TextArea(1,5)] public string[] Lines;
            /// <summary>
            /// Speak only single time.
            /// </summary>
            public bool Removable;
        }

        public List<Speech> TalkSpeeches;
        [TextArea(1, 5)] public List<string> WelcomeLines;
        [TextArea(1, 5)] public List<string> FarewellLines;
        [TextArea(1, 5)] public List<string> PurchaseLines;
        [TextArea(1, 5)] public List<string> SellLines;
        [TextArea(1, 5)] public List<string> NotEnoughMoneyLines;
        [TextArea(1, 5)] public List<string> NoPlaceLines;
    }

    [CreateAssetMenu(fileName = "ShopInfo", menuName = "ScriptableObjects/ShopInfo")]
    public class ShopInfo : ScriptableObject
    {
        public string ShopName;
        public Sprite BackgroundImage;
        public Shopkeeper Shopkeeper;
        public ItemData[] ShopItemsData;
    }
}
