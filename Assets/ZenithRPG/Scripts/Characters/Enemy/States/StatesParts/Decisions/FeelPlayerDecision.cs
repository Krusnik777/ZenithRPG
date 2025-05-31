using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class FeelPlayerDecision : EnemyDecision
    {
        [SerializeField] private float m_range = 1.5f;

        public override void OnStart(EnemyAIController controller) { }

        public override bool Decide(EnemyAIController controller)
        {
            bool playerNear = Vector3.Distance(controller.transform.position, LevelState.Instance.Player.transform.position) <= m_range;

            return playerNear;
        }
    }
}
