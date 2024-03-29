namespace DC_ARPG
{
    public class EnemyStats : CharacterStats<EnemyStatsInfo>
    {
        public int ExperiencePoints { get; private set; }
        public int DroppedGold { get; private set; }

        public DroppedItem[] DroppedItems { get; private set; }

        public override void InitStats(EnemyStatsInfo enemyInfo)
        {
            base.InitStats(enemyInfo);

            Attack = Strength + enemyInfo.AttackIncrease;
            Defense = enemyInfo.Defense;
            ExperiencePoints = enemyInfo.ExperiencePoints;
            DroppedGold = enemyInfo.DroppedGold;
            DroppedItems = enemyInfo.DroppedItems;
        }
    }
}
