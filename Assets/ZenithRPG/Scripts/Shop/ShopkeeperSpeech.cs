using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

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

            int speechIndex = -1;

            for (int i = 0; i < currentShopkeeper.TalkSpeeches.Count; i++)
            {
                if (!currentShopkeeper.TalkSpeeches[i].Listened)
                {
                    speechIndex = i;
                    break;
                }
            }

            while (speechIndex == -1)
            {
                speechIndex = Random.Range(0, currentShopkeeper.TalkSpeeches.Count);
                if (!currentShopkeeper.TalkSpeeches[speechIndex].Repeatable) speechIndex = -1;
            }

            currentSpeech = currentShopkeeper.TalkSpeeches[speechIndex];

            ContinueSpeech();
        }

        public void ContinueSpeech()
        {
            if (currentLineNumber < currentSpeech.Lines.Count)
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
            currentSpeech.Listened = true;

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
