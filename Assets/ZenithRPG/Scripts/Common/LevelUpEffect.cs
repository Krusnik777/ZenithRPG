using UnityEngine;

namespace DC_ARPG
{
    public class LevelUpEffect : MonoBehaviour
    {
        [SerializeField] private GameObject m_levelUpEffect;

        private PlayerCharacter m_playerCharacter;

        private PlayerStats stats => m_playerCharacter.Stats as PlayerStats;

        private void Start()
        {
            m_playerCharacter = GetComponent<PlayerCharacter>();

            stats.EventOnLevelUp += OnLevelUp;
        }

        private void OnDestroy()
        {
            stats.EventOnLevelUp -= OnLevelUp;
        }

        private void OnLevelUp()
        {
            if (m_levelUpEffect == null) return;

            ShortMessage.Instance.ShowMessage("УРОВЕНЬ ПОВЫШЕН!");

            m_levelUpEffect.SetActive(true);

            CancelInvoke("TurnOffEffect");
            Invoke("TurnOffEffect", 2.0f);
        }

        private void TurnOffEffect()
        {
            if (m_levelUpEffect == null) return;

            m_levelUpEffect.SetActive(false);
        }
    }
}
