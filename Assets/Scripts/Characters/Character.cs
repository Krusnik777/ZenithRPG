using UnityEngine;

namespace DC_ARPG
{
    public class Character : MonoBehaviour
    {
        private PlayerStats characterStats;
        [SerializeField] private PlayerStatsInfo m_playerStatsInfo;

        private void Start()
        {
            characterStats = new PlayerStats();
            characterStats.InitStats(m_playerStatsInfo);
            Debug.Log(characterStats.Level);
            Debug.Log(characterStats.Strength);
            Debug.Log(characterStats.Intelligence);
            Debug.Log(characterStats.MagicResist);
            Debug.Log(characterStats.Luck);
            Debug.Log(characterStats.Attack);
            Debug.Log(characterStats.Defense);
        }
    }
}
