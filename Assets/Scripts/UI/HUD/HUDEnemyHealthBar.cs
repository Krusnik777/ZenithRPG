using UnityEngine;
using UnityEngine.UI;

namespace DC_ARPG
{
    public class HUDEnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private GameObject m_enemyHealthBarPanel;
        [SerializeField] private Image m_fillImage;

        private void Update()
        {
            if (GameState.State != GameState.GameplayState.Active) return;

            UpdateEnemyHealthBar(m_player.CheckForwardGridForEnemy());
        }

        private void UpdateEnemyHealthBar(Enemy enemy)
        {
            if (enemy != null)
            {
                m_enemyHealthBarPanel.SetActive(true);
                m_fillImage.fillAmount = (float) enemy.Character.EnemyStats.CurrentHitPoints / (float) enemy.Character.EnemyStats.HitPoints;
            }
            else
            {
                m_enemyHealthBarPanel.SetActive(false);
            }
        }
    }
}
