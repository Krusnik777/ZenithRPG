using System;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private Enemy m_enemy;
        public Enemy Enemy => m_enemy;
        [SerializeField] private EnemyState m_activeState;
        [SerializeField] private EnemyState m_dummyState; // TO DO
        [SerializeField] private FieldOfView m_enemyFOV;
        public FieldOfView EnemyFOV => m_enemyFOV;

        public static bool BusyFindingPath;

        public event UnityAction<EnemyAIController> EventOnChaseStarted;
        public void OnChaseStartInvoke() => EventOnChaseStarted?.Invoke(this);
        public event UnityAction<EnemyAIController> EventOnChaseEnded;
        public void OnChaseEndInvoke() => EventOnChaseEnded?.Invoke(this);

        private PathFinder pathFinder;
        public PathFinder PathFinder => pathFinder;

        private bool isStopped = false;

        private Tile targetedTile;
        public Tile TargetedTile { get => targetedTile; set => targetedTile = value; }

        public void StartState(EnemyState state, EnemyDecision decision = null)
        {
            m_activeState = state;

            m_activeState.OnStart(this);

            //Debug.Log(transform.name + " - New State: " + state.GetType() + " by Desision " + decision);
        }

        public void StopActivity()
        {
            isStopped = true;
        }

        public void ResumeActivity()
        {
            isStopped = false;
        }

        private void Start()
        {
            pathFinder = new PathFinder(m_enemy);

            m_activeState.OnStart(this);

            m_enemy.EventOnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            m_enemy.EventOnDeath -= OnDeath;
        }

        private void Update()
        {
            if (isStopped) return;

            if (m_enemy.IsPushedBack) return;

            m_activeState.DoActions(this);

            m_activeState.CheckTransitions(this);
        }

        private void OnDeath(Enemy enemy)
        {
            m_enemy.EventOnDeath -= OnDeath;

            if (targetedTile != null) targetedTile.SetTileOccupied(null);
            targetedTile = null;
        }
    }
}
