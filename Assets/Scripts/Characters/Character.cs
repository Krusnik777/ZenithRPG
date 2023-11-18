using UnityEngine;

namespace DC_ARPG
{
    public class Character : MonoBehaviour
    {
        private PlayerStats playerStats;
        public PlayerStats PlayerStats => playerStats;
        [SerializeField] private PlayerStatsInfo m_playerStatsInfo;

        private void Awake()
        {
            playerStats = new PlayerStats();
            playerStats.InitStats(m_playerStatsInfo);
        }
    }
}
