using UnityEngine.Events;

namespace DC_ARPG
{
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

        public UnityEvent OnDeath;

        #endregion

        public virtual void InitStats(T characterInfo)
        {
            Level = characterInfo.Level;
            HitPoints = characterInfo.Level;
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
        public void ChangeCurrentHitPoints(int change)
        {
            CurrentHitPoints += change;

            if (CurrentHitPoints <= 0)
            {
                OnDeath?.Invoke();
            }

            if (CurrentHitPoints >= HitPoints)
            {
                CurrentHitPoints = HitPoints;
            }
        }
    }
}
