using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class PlayerInChaseRangeDecision : EnemyDecision
    {
        [SerializeField] private float m_maxRange = 6.0f;

        private float currentRange;

        public override void OnStart(EnemyAIController controller)
        {
            currentRange = m_maxRange;
        }

        public override bool Decide(EnemyAIController controller)
        {
            bool playerNear = Vector3.Distance(controller.transform.position, LevelState.Instance.Player.transform.position) <= currentRange;

            if (playerNear) currentRange--;

            return playerNear;
        }
    }
}
