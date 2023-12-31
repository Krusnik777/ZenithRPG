using UnityEngine.Events;

namespace DC_ARPG
{
    public enum DamageType
    {
        None,
        Physic,
        Magic
    }

    public abstract class CharacterStats<T> where T : CharacterStatsInfo
    {
        #region BaseStats
        public int Level { get; protected set; }
        public int HitPoints { get; protected set; }
        public int MagicPoints { get; protected set; }
        public int Strength { get; protected set; }
        public int Intelligence { get; protected set; }
        public int MagicResist { get; protected set; }
        public int Luck { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        #endregion

        #region BaseParameters

        public int CurrentHitPoints { get; protected set; }
        public int CurrentMagicPoints { get; protected set; }

        #endregion

        #region BaseEvents

        public event UnityAction<int> EventOnHitPointsChange;
        public event UnityAction<object> EventOnDeath;
        public event UnityAction EventOnMagicPointsChange;

        #endregion

        public virtual void InitStats(T characterInfo)
        {
            Level = characterInfo.Level;
            HitPoints = characterInfo.HitPoints;
            MagicPoints = characterInfo.MagicPoints;
            Strength = characterInfo.Strength;
            Intelligence = characterInfo.Intelligence;
            MagicResist = characterInfo.MagicResist;
            Luck = characterInfo.Luck;

            SetBaseParametersPoints();
        }

        public void SetBaseParametersPoints()
        {
            CurrentHitPoints = HitPoints;
            CurrentMagicPoints = MagicPoints;
        }

        /// <summary>
        /// Change current HP. Damage or Heal based on parameter.
        /// </summary>
        /// <param name="change">Parameter with minus = damage to HP</param>
        /// <param name="damageType">DamageType.None(default),DamageType.Physic,DamageType.Magic</param>
        public void ChangeCurrentHitPoints(object sender, int change, DamageType damageType = DamageType.None)
        {
            if (change < 0)
            {
                if (damageType == DamageType.Physic) change += Defense;
                if (damageType == DamageType.Magic) change += MagicResist;

                if (change > 0) change = -1;
            }

            CurrentHitPoints += change;

            if (CurrentHitPoints >= HitPoints)
            {
                CurrentHitPoints = HitPoints;
            }

            if (CurrentHitPoints <= 0)
            {
                CurrentHitPoints = 0;

                EventOnDeath?.Invoke(sender);
            }

            EventOnHitPointsChange?.Invoke(change);
        }

        public bool TryUseMagicPoints(int magicPoints)
        {
            if (CurrentMagicPoints - magicPoints < 0) return false;

            CurrentMagicPoints -= magicPoints;

            EventOnMagicPointsChange?.Invoke();

            return true;
        }

        public void RecoverMagicPoints(object sender, int change)
        {
            if (CurrentMagicPoints >= MagicPoints) return;

            CurrentMagicPoints += change;

            EventOnMagicPointsChange?.Invoke();
        }
    }
}
