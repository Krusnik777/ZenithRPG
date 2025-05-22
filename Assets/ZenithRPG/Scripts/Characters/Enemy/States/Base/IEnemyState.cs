namespace DC_ARPG
{
    public interface IEnemyState
    {
        IEnemyState NextState { get; set; }

        void Act(EnemyAIController controller);
        void StartState(EnemyAIController controller);
    }
}
