namespace DC_ARPG
{
    public abstract class EnemyAction
    {
        public abstract void OnStart(EnemyAIController controller);
        public abstract void Act(EnemyAIController controller);
    }
}
