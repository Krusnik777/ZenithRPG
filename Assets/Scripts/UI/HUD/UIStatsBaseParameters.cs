using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIStatsBaseParameters : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter m_playerCharacter;
        [Header("HitPoints")]
        [SerializeField] private TextMeshProUGUI m_hitPointsValueText;
        [SerializeField] private Image m_hitPointsFillImage;
        [Header("MagicPoints")]
        [SerializeField] private TextMeshProUGUI m_magicPointsValueText;
        [SerializeField] private Image m_magicPointsFillImage;

        private PlayerStats stats => m_playerCharacter.Stats as PlayerStats;

        private void Start()
        {
            stats.EventOnHitPointsChange += SetHitPoints;
            stats.EventOnMagicPointsChange += SetMagicPoints;
            stats.EventOnIntelligenceUp += SetMagicPoints;

            stats.EventOnLevelUp += SetBothPoints;
            stats.EventOnStatsUpdated += SetBothPoints;

            SetBothPoints();
        }

        private void OnDestroy()
        {
            stats.EventOnHitPointsChange -= SetHitPoints;
            stats.EventOnMagicPointsChange -= SetMagicPoints;
            stats.EventOnIntelligenceUp -= SetMagicPoints;

            stats.EventOnLevelUp -= SetBothPoints;
            stats.EventOnStatsUpdated -= SetBothPoints;
        }

        private void SetHitPoints(int change = 0)
        {
            m_hitPointsValueText.text = $"{m_playerCharacter.Stats.CurrentHitPoints}/{m_playerCharacter.Stats.HitPoints}";
            m_hitPointsFillImage.fillAmount = (float) m_playerCharacter.Stats.CurrentHitPoints / (float) m_playerCharacter.Stats.HitPoints;
        }

        private void SetMagicPoints()
        {
            m_magicPointsValueText.text = $"{m_playerCharacter.Stats.CurrentMagicPoints}/{m_playerCharacter.Stats.MagicPoints}";
            m_magicPointsFillImage.fillAmount = (float) m_playerCharacter.Stats.CurrentMagicPoints / (float) m_playerCharacter.Stats.MagicPoints;
        }

        private void SetBothPoints()
        {
            SetHitPoints();
            SetMagicPoints();
        }
    }
}
