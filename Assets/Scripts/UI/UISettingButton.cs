using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UISettingButton : UISelectableButton, IScriptableObjectProperty
    {
        [SerializeField] private Setting m_setting;
        [SerializeField] private TextMeshProUGUI m_titleText;
        [SerializeField] private TextMeshProUGUI m_valueText;
        [SerializeField] private Image m_previousImage;
        [SerializeField] private Image m_nextImage;

        #region Public

        public void SetNextValueSetting()
        {
            m_setting?.SetNextValue();
            m_setting?.Apply();
            UpdateInfo();

            OnClick?.Invoke();
        }
        public void SetPreviousValueSetting()
        {
            m_setting?.SetPreviousValue();
            m_setting?.Apply();
            UpdateInfo();

            OnClick?.Invoke();
        }

        public void ApplyProperty(ScriptableObject property)
        {
            if (property == null || !(property is Setting)) return;

            m_setting = property as Setting;

            UpdateInfo();
        }

        #endregion

        #region Private

        private void Start()
        {
            ApplyProperty(m_setting);
        }

        private void UpdateInfo()
        {
            m_titleText.text = m_setting.Title;
            m_valueText.text = m_setting.GetStringValue();

            m_previousImage.enabled = !m_setting.isMinValue;
            m_nextImage.enabled = !m_setting.isMaxValue;
        }

        #endregion
    }
}
