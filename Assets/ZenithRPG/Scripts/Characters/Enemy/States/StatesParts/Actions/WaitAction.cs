using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class WaitAction : EnemyAction
    {
        [SerializeField] private float m_time = 5.0f;

        private Timer timer;

        public bool IsOver => timer.IsFinished;

        public void SetTime(float time) => m_time = time;

        public override void OnStart(EnemyAIController controller)
        {
            timer = new Timer(m_time);
        }

        public override void Act(EnemyAIController controller)
        {
            timer.RemoveTime(Time.deltaTime);
        }
    }
}
