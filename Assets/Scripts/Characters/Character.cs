using UnityEngine;

namespace DC_ARPG
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private PlayerStatsInfo m_playerStatsInfo;
        private PlayerStats playerStats;
        public PlayerStats PlayerStats => playerStats;

        private Inventory inventory;
        public Inventory Inventory => inventory;

        private void Awake()
        {
            playerStats = new PlayerStats();
            playerStats.InitStats(m_playerStatsInfo);

            inventory = new Inventory(3, 9, 3);
        }
    }
}
