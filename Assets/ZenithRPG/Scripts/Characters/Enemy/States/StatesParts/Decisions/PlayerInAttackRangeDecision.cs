using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class PlayerInAttackRangeDecision : EnemyDecision
    {
        [SerializeField] private float m_distance = 1.0f;

        public override void OnStart(EnemyAIController controller) { }

        public override bool Decide(EnemyAIController controller)
        {
            Ray attackRay = new Ray(controller.transform.position + new Vector3(0, 0.25f, 0), controller.transform.forward);

            if (Physics.Raycast(attackRay, m_distance, controller.EnemyFOV.TargetMask))
            {
                return true;
            }

            return false;
        }
    }
}
