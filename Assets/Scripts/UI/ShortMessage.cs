using System.Collections;
using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class ShortMessage : MonoBehaviour
    {
        [SerializeField] private GameObject m_shortMessagePanel;
        [SerializeField] private TextMeshProUGUI m_shortMessageText;
        [SerializeField] private float m_shortMessageTime;

        private Coroutine displayMessage;

        public void ShowMessage(string message)
        {
            if (message == "") return;

            if (displayMessage != null) StopCoroutine(displayMessage);
            displayMessage = StartCoroutine(DisplayMessage(message));
        }


        private void Start()
        {
            m_shortMessagePanel.SetActive(false);
        }

        #region Coroutines

        private IEnumerator DisplayMessage(string message)
        {
            m_shortMessagePanel.SetActive(true);
            m_shortMessageText.text = message;
            var timer = 0f;

            while(timer < m_shortMessageTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            m_shortMessagePanel.SetActive(false);
        }

        #endregion
    }
}
