using System;
using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class LevelState : MonoSingleton<LevelState>, IDependency<Player>
    {
        private Tile[] m_levelTileField;
        public Tile[] LevelTileField => m_levelTileField;

        private List<Enemy> m_allEnemies;
        public List<Enemy> AllEnemies => m_allEnemies;

        private List<EnemyAIController> m_chasingEnemies;
        public List<EnemyAIController> ChasingEnemies => m_chasingEnemies;

        private Player m_player;
        public void Construct(Player player) => m_player = player;
        public Player Player => m_player;

        public void StopAllActivity()
        {
            for (int i = m_allEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = m_allEnemies[i];

                if (enemy == null)
                {
                    m_allEnemies.Remove(enemy);
                    continue;
                }

                if (enemy.gameObject.activeInHierarchy)
                    if (enemy.EnemyAI != null) enemy.EnemyAI.StopActivity();
            }
        }

        public void ResumeAllActivity()
        {
            for (int i = m_allEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = m_allEnemies[i];

                if (enemy == null)
                {
                    m_allEnemies.Remove(enemy);
                    continue;
                }

                if (enemy.gameObject.activeInHierarchy)
                    if (enemy.EnemyAI != null) enemy.EnemyAI.ResumeActivity();
            }
        }

        public void ResetAllPathFindings()
        {
            foreach (var tile in m_levelTileField)
            {
                tile.ResetPathFindingValues();
            }
        }

        public List<Tile> GetAvailableNeighborTilesToPlayer()
        {
            List<Tile> tiles = new List<Tile>();

            var possibleTargetTiles = m_player.CurrentTile.NeighborTiles;

            if (possibleTargetTiles.Count == 0) return null;

            for (int i = 0; i < possibleTargetTiles.Count; i++)
            {
                if (possibleTargetTiles[i].Type == TileType.Obstacle || possibleTargetTiles[i].Type == TileType.Pit) continue;

                if (possibleTargetTiles[i].Type == TileType.Closable && possibleTargetTiles[i].CheckClosed()) continue;

                tiles.Add(possibleTargetTiles[i]);
            }

            return tiles;
        }

        private void Start()
        {
            m_levelTileField = FindObjectsOfType<Tile>();

            foreach (var tile in m_levelTileField)
            {
                tile.FindNeighbors();
            }

            m_allEnemies = new List<Enemy>();
            m_allEnemies.AddRange(FindObjectsOfType<Enemy>(true));

            m_chasingEnemies = new List<EnemyAIController>();

            StoryEventManager.Instance.EventOnStoryEventStarted += StopAllActivity;
            StoryEventManager.Instance.EventOnStoryEventEnded += ResumeAllActivity;
            
            foreach (var enemy in m_allEnemies)
            {
                enemy.EventOnDeath += OnEnemyDeath;

                enemy.EnemyAI.EventOnChaseStarted += AddChasingEnemyToList;
                enemy.EnemyAI.EventOnChaseEnded += RemoveChasingEnemyFromList;
            }
        }

        private void OnDestroy()
        {
            StoryEventManager.Instance.EventOnStoryEventStarted -= StopAllActivity;
            StoryEventManager.Instance.EventOnStoryEventEnded -= ResumeAllActivity;
        }

        private void OnEnemyDeath(Enemy enemy)
        {
            enemy.EventOnDeath -= OnEnemyDeath;
            enemy.EnemyAI.EventOnChaseStarted -= AddChasingEnemyToList;
            enemy.EnemyAI.EventOnChaseEnded -= RemoveChasingEnemyFromList;

            RemoveChasingEnemyFromList(enemy.EnemyAI);

            m_allEnemies.Remove(enemy);
        }

        private void AddChasingEnemyToList(EnemyAIController chasingEnemyAI)
        {
            if (!m_chasingEnemies.Contains(chasingEnemyAI))
            {
                m_chasingEnemies.Add(chasingEnemyAI);
            }
        }

        private void RemoveChasingEnemyFromList(EnemyAIController chasingEnemyAI)
        {
            if (m_chasingEnemies.Contains(chasingEnemyAI))
            {
                m_chasingEnemies.Remove(chasingEnemyAI);
            }
        }
    }
}
