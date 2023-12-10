using UnityEngine;
using UnityEngine.UI;

namespace DC_ARPG
{
    public class ScrollableList : MonoBehaviour
    {
        [SerializeField] private UISelectableButtonContainer m_buttonContrainer;
        [SerializeField] private int m_maxVisibleButtons = 6;
        [SerializeField] private float m_scrollStep = 150;
        [SerializeField] private Image m_upArrowActiveImage;
        [SerializeField] private Image m_downArrowActiveImage;

        private int currentMinIndex;
        private int currentMaxIndex;

        private RectTransform m_rectTransform;

        private void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();

            currentMinIndex = 0;
            currentMaxIndex = m_maxVisibleButtons;

            m_buttonContrainer.EventOnButtonsCollected += OnButtonsCollected;
        }

        private void OnButtonsCollected()
        {
            foreach (var button in m_buttonContrainer.Buttons)
            {
                button.OnSelect.AddListener(ChangePosition);
            }

            UpdateArrowsImages();
        }

        private void OnDestroy()
        {
            m_buttonContrainer.EventOnButtonsCollected -= OnButtonsCollected;

            foreach (var button in m_buttonContrainer.Buttons)
            {
                button.OnSelect.RemoveListener(ChangePosition);
            } 
        }

        private void ChangePosition()
        {
            if (m_buttonContrainer.SelectedButtonIndex > currentMaxIndex - 1)
            {
                m_rectTransform.offsetMax += new Vector2(0, m_scrollStep);
                currentMinIndex++;
                currentMaxIndex++;
            }

            if (m_buttonContrainer.SelectedButtonIndex < currentMinIndex)
            {
                m_rectTransform.offsetMax -= new Vector2(0, m_scrollStep);
                currentMinIndex--;
                currentMaxIndex--;
            }

            UpdateArrowsImages();
        }

        private void UpdateArrowsImages()
        {
            if (currentMinIndex > 0) m_upArrowActiveImage.enabled = true;
            else m_upArrowActiveImage.enabled = false;

            if (m_buttonContrainer.Buttons.Length > currentMaxIndex) m_downArrowActiveImage.enabled = true;
            else m_downArrowActiveImage.enabled = false;
        }

    }
}
