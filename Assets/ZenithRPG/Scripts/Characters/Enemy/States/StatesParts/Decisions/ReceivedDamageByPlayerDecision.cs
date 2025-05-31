namespace DC_ARPG
{
    [System.Serializable]
    public class ReceivedDamageByPlayerDecision : EnemyDecision
    {
        private EnemyAIController controller;
        private bool receivedDamage;

        public override void OnStart(EnemyAIController controller)
        {
            receivedDamage = false;

            this.controller = controller;

            this.controller.Enemy.Character.Stats.EventOnHitPointsChange += OnHealthChange;
        }

        public override bool Decide(EnemyAIController controller)
        {
            return receivedDamage;
        }

        private void OnHealthChange(object sender, int HitPoints)
        {
            if (sender is Player)
            {
                receivedDamage = true;
                controller.Enemy.Character.Stats.EventOnHitPointsChange -= OnHealthChange;
            }
        }
    }
}
