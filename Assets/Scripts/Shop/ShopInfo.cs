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
            [TextArea(1,5)] public List<string> Lines;

            public bool Repeatable = true;
            public bool Listened { get; set; }

            public Speech() { }

            public Speech(Speech speech)
            {
                Lines = new List<string>(speech.Lines);
                Repeatable = speech.Repeatable;
                Listened = false;
            }
        }

        public List<Speech> TalkSpeeches;
        [TextArea(1, 5)] public List<string> WelcomeLines;
        [TextArea(1, 5)] public List<string> FarewellLines;
        [TextArea(1, 5)] public List<string> PurchaseLines;
        [TextArea(1, 5)] public List<string> SellLines;
        [TextArea(1, 5)] public List<string> SellFailureLines;
        [TextArea(1, 5)] public List<string> NotEnoughMoneyLines;
        [TextArea(1, 5)] public List<string> NoPlaceLines;

        public Shopkeeper() { }

        public Shopkeeper(Shopkeeper shopkeeper)
        {
            TalkSpeeches = new List<Speech>();

            foreach (var speech in shopkeeper.TalkSpeeches)
            {
                TalkSpeeches.Add(new Speech(speech));
            }

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
