namespace DC_ARPG
{
    public class EnemyStats : CharacterStats<EnemyStatsInfo>
    {
        public int ExperiencePoints { get; private set; }
        public int DroppedGold { get; private set; }

        public override void InitStats(EnemyStatsInfo enemyInfo)
        {
            base.InitStats(enemyInfo);

            Attack = enemyInfo.Attack;
            Defense = enemyInfo.Defense;
            ExperiencePoints = enemyInfo.ExperiencePoints;
            DroppedGold = enemyInfo.DroppedGold;
        }

    }
}
