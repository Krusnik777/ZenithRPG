using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class StatusPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_levelValue;

        [SerializeField] private TextMeshProUGUI m_hitPointsValue;

        [SerializeField] private TextMeshProUGUI m_magicPointsValue;

        [SerializeField] private TextMeshProUGUI m_strengthValue;

        [SerializeField] private TextMeshProUGUI m_intelligenceValue;

        [SerializeField] private TextMeshProUGUI m_magicResistValue;

        [SerializeField] private TextMeshProUGUI m_luckValue;
        [SerializeField] private TextMeshProUGUI m_attackValue;
        [SerializeField] private TextMeshProUGUI m_defenseValue;

        public void UpdateStatus(PlayerCharacter playerCharacter)
        {
            var stats = playerCharacter.PlayerStats;

            m_levelValue.text = stats.Level.ToString();

            m_hitPointsValue.text = $"{stats.CurrentHitPoints}/{stats.HitPoints}";
            m_magicPointsValue.text = $"{stats.CurrentMagicPoints}/{stats.MagicPoints}";

            m_strengthValue.text = stats.Strength.ToString();

            m_intelligenceValue.text = stats.Intelligence.ToString();

            m_magicResistValue.text = stats.MagicResist.ToString();

            m_luckValue.text = stats.Luck.ToString();
            m_attackValue.text = stats.Attack.ToString();
            m_defenseValue.text = stats.Defense.ToString();
        }
    }
}
