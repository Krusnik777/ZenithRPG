using System.Collections;
using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class ShopkeeperSpeech : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_phraseText;
        [SerializeField] private float m_shortPhraseTime;
        public float ShortPhraseTime => m_shortPhraseTime;

        private Coroutine displayPhrase;

        private int currentLineNumber;
        private Shopkeeper currentShopkeeper;
        private Shopkeeper.Speech currentSpeech;

        public void StartSpeech(Shopkeeper shopkeeper)
        {
            if (displayPhrase != null) StopCoroutine(displayPhrase);

            currentLineNumber = 0;

            currentShopkeeper = shopkeeper;
            currentSpeech = currentShopkeeper.TalkSpeeches[shopkeeper.CurrentSpeech];

            ContinueSpeech();
        }

        public void ContinueSpeech()
        {
            if (currentLineNumber < currentSpeech.Lines.Length)
            {
                m_phraseText.text = currentSpeech.Lines[currentLineNumber];

                currentLineNumber++;
            }
            else
            {
                m_phraseText.text = "";
                EndCurrentSpeech();
            }
        }

        public void ShowShortPhrase(string phrase)
        {
            if (displayPhrase != null) StopCoroutine(displayPhrase);
            displayPhrase = StartCoroutine(DisplayShortPhrase(phrase));
        }

        private void EndCurrentSpeech()
        {
            if (!currentSpeech.Repeatable)
            {
                if (currentShopkeeper.TalkSpeeches.Count > currentShopkeeper.CurrentSpeech + 1)
                    currentShopkeeper.CurrentSpeech++;
            }

            currentShopkeeper = null;
            currentSpeech = null;
            currentLineNumber = 0;

            UIShop.Instance.EndTalk();
        }

        #region Coroutines

        private IEnumerator DisplayShortPhrase(string phrase)
        {
            m_phraseText.text = phrase;

            var timer = 0f;

            while (timer < m_shortPhraseTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            m_phraseText.text = "";
        }

        #endregion
    }
}
