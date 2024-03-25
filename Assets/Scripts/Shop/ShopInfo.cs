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

            public bool Repeatable = true;
        }

        public List<Speech> TalkSpeeches;
        [TextArea(1, 5)] public List<string> WelcomeLines;
        [TextArea(1, 5)] public List<string> FarewellLines;
        [TextArea(1, 5)] public List<string> PurchaseLines;
        [TextArea(1, 5)] public List<string> SellLines;
        [TextArea(1, 5)] public List<string> SellFailureLines;
        [TextArea(1, 5)] public List<string> NotEnoughMoneyLines;
        [TextArea(1, 5)] public List<string> NoPlaceLines;

        public int CurrentSpeech { get; set; }

        public Shopkeeper()
        {

        }

        public Shopkeeper(Shopkeeper shopkeeper)
        {
            TalkSpeeches = new List<Speech>(shopkeeper.TalkSpeeches);
            WelcomeLines = new List<string>(shopkeeper.WelcomeLines);
            FarewellLines = new List<string>(shopkeeper.FarewellLines);
            PurchaseLines = new List<string>(shopkeeper.PurchaseLines);
            SellLines = new List<string>(shopkeeper.SellLines);
            SellFailureLines = new List<string>(shopkeeper.SellFailureLines);
            NotEnoughMoneyLines = new List<string>(shopkeeper.NotEnoughMoneyLines);
            NoPlaceLines = new List<string>(shopkeeper.NoPlaceLines);
        }
    }

    [CreateAssetMenu(fileName = "ShopInfo", menuName = "ScriptableObjects/ShopInfo")]
    public class ShopInfo : ScriptableObject
    {
        public string ShopName;
        public Sprite BackgroundImage;
        public Shopkeeper Shopkeeper;
        public ItemData[] ShopItemsData;
        public float Surcharge;
    }
}
