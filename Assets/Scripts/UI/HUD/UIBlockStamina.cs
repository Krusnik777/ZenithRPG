using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DC_ARPG
{
    public class UIBlockStamina : MonoBehaviour
    {
        [SerializeField] private BlockStamina m_blockStamina;
        [SerializeField] private GameObject m_panel;
        [SerializeField] private Image m_iconImage;
        [SerializeField] private Image m_fillImage;
        [SerializeField] private Color m_defaultColor;
        [SerializeField] private Color m_breakColor;

        private Coroutine updateViewRoutine;

        private void Start()
        {
            m_blockStamina.EventOnStaminaSpended += UpdateView;
            m_blockStamina.EventOnDefenseBreaked += UpdateViewToBreaked;

            ChangePanelColor(m_defaultColor);

            if (m_panel.activeInHierarchy) SetPanelActive(false);
        }

        private void OnDestroy()
        {
            m_blockStamina.EventOnStaminaSpended -= UpdateView;
            m_blockStamina.EventOnDefenseBreaked -= UpdateViewToBreaked;
        }

        private void UpdateView()
        {
            if (updateViewRoutine != null) return;

            updateViewRoutine = StartCoroutine(UpdateViewRoutine());
        }

        private void UpdateViewToBreaked()
        {
            ChangePanelColor(m_breakColor);

            if (updateViewRoutine != null) return;

            updateViewRoutine = StartCoroutine(UpdateViewRoutine());
        }

        private void SetPanelActive(bool state)
        {
            m_panel.SetActive(state);
        }

        private void ChangePanelColor(Color color)
        {
            m_iconImage.color = color;
            m_fillImage.color = color;
        }

        private IEnumerator UpdateViewRoutine()
        {
            SetPanelActive(true);

            while (!m_blockStamina.Recovered)
            {
                m_fillImage.fillAmount = m_blockStamina.CurrentStamina / m_blockStamina.MaxStamina;

                yield return new WaitForFixedUpdate();
            }

            SetPanelActive(false);
            ChangePanelColor(m_defaultColor);

            updateViewRoutine = null;
        }
    }
}
