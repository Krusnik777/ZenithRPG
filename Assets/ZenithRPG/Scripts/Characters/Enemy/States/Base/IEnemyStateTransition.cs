namespace DC_ARPG
{
    public interface IEnemyStateTransition
    {
        public EnemyDecision[] Decisions { get; }
        public EnemyState TargetState { get; }

        public void Init();
    }
}
