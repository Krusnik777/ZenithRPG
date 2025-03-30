using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIStatsProgress : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter m_playerCharacter;
        [SerializeField] private GameObject m_statsProgressPanel;
        [Header("Level")]
        [SerializeField] private TextMeshProUGUI m_levelValueText;
        [SerializeField] private Image m_levelFillImage;
        [Header("Strength")]
        [SerializeField] private TextMeshProUGUI m_strengthValueText;
        [SerializeField] private Image m_strengthFillImage;
        [Header("Intelligence")]
        [SerializeField] private TextMeshProUGUI m_intelligenceValueText;
        [SerializeField] private Image m_intelligenceFillImage;
        [Header("MagicResist")]
        [SerializeField] private TextMeshProUGUI m_magicResistValueText;
        [SerializeField] private Image m_magicResistFillImage;

        private PlayerStats stats => m_playerCharacter.Stats as PlayerStats;

        private void Start()
        {
            SetLevelValue();
            SetStrengthValue();
            SetIntelligenceValue();
            SetMagicResistValue();

            FillLevelBar();
            FillStrengthBar();
            FillIntelligenceBar();
            FillMagicResistBar();

            stats.EventOnExperienceChange += FillLevelBar;
            stats.EventOnStrengthExperienceChange += FillStrengthBar;
            stats.EventOnIntelligenceExperienceChange += FillIntelligenceBar;
            stats.EventOnMagicResistExperienceChange += FillMagicResistBar;

            stats.EventOnLevelUp += SetLevelValue;
            stats.EventOnStrengthUp += SetStrengthValue;
            stats.EventOnIntelligenceUp += SetIntelligenceValue;
            stats.EventOnMagicResistUp += SetMagicResistValue;

            stats.EventOnStatsUpdated += UpdateAll;
        }

        private void OnDestroy()
        {
            stats.EventOnExperienceChange -= FillLevelBar;
            stats.EventOnStrengthExperienceChange -= FillStrengthBar;
            stats.EventOnIntelligenceExperienceChange -= FillIntelligenceBar;
            stats.EventOnMagicResistExperienceChange -= FillMagicResistBar;

            stats.EventOnLevelUp -= SetLevelValue;
            stats.EventOnStrengthUp -= SetStrengthValue;
            stats.EventOnIntelligenceUp -= SetIntelligenceValue;
            stats.EventOnMagicResistUp -= SetMagicResistValue;

            stats.EventOnStatsUpdated -= UpdateAll;
        }

        public void FillLevelBar() => m_levelFillImage.fillAmount = stats.Level >= stats.MaxLevel ? 1 : (float) stats.CurrentExperiencePoints / (float) stats.GetExperienceForLevelUp();
        public void FillStrengthBar() => m_strengthFillImage.fillAmount = stats.Strength >= stats.MaxStatLevel ? 1 : (float) stats.CurrentStrengthExperiencePoints / (float) stats.GetExperienceForStrengthUp();
        public void FillIntelligenceBar() => m_intelligenceFillImage.fillAmount = stats.Intelligence >= stats.MaxStatLevel ? 1 : (float) stats.CurrentIntelligenceExperiencePoints / (float) stats.GetExperienceForIntelligenceUp();
        public void FillMagicResistBar() => m_magicResistFillImage.fillAmount = stats.MagicResist >= stats.MaxStatLevel ? 1 : (float) stats.CurrentMagicResistExperiencePoints / (float) stats.GetExperienceForMagicResistUp();

        public void SetLevelValue() => m_levelValueText.text = m_playerCharacter.Stats.Level.ToString();
        public void SetStrengthValue() => m_strengthValueText.text = m_playerCharacter.Stats.Strength.ToString();
        public void SetIntelligenceValue() => m_intelligenceValueText.text = m_playerCharacter.Stats.Intelligence.ToString();
        public void SetMagicResistValue() => m_magicResistValueText.text = m_playerCharacter.Stats.MagicResist.ToString();

        private void UpdateAll()
        {
            SetLevelValue();
            SetStrengthValue();
            SetIntelligenceValue();
            SetMagicResistValue();

            FillLevelBar();
            FillStrengthBar();
            FillIntelligenceBar();
            FillMagicResistBar();
        }
    }
}
