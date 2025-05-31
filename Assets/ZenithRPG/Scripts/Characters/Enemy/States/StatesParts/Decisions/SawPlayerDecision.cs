using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class SawPlayerDecision : EnemyDecision
    {
        public override void OnStart(EnemyAIController controller) { }

        public override bool Decide(EnemyAIController controller)
        {
            return controller.EnemyFOV.CanSeePlayer;
        }


    }
}
