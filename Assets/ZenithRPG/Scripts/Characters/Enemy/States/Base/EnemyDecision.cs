namespace DC_ARPG
{
    public abstract class EnemyDecision
    {
        public abstract void OnStart(EnemyAIController controller);
        public abstract bool Decide(EnemyAIController controller);
    }
}
