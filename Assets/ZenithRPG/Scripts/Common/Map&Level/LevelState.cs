using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class LevelState : MonoSingleton<LevelState>, IDependency<Player>
    {
        private Tile[] m_levelTileField;
        public Tile[] LevelTileField => m_levelTileField;

        #region MAP

        private TileMarker[] m_tileMarkers;

        private int gridSize;

        private Vector2 min;
        private Vector2 max;

        private Map currentMap;
        public Map CurrentMap => currentMap;

        private MapConstructor m_mapConstructor;

        #endregion

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

            // Load Map From Save - TO DO

            // if loaded - TO DO

            // if don't have saved map

            SetupTileMarkers();

            currentMap = new Map(m_tileMarkers, "test", gridSize);

            m_mapConstructor = GetComponentInChildren<MapConstructor>();

            m_mapConstructor.ConstructMap(currentMap);

            //

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

        private void SetupTileMarkers()
        {
            m_tileMarkers = FindObjectsOfType<TileMarker>();

            float posX = m_tileMarkers[0].transform.localPosition.x + 0.5f;
            float posZ = m_tileMarkers[0].transform.localPosition.z + 0.5f;

            //Debug.Log(posX + " " + posZ);

            min = new Vector2(posX, posZ);
            max = new Vector2(posX, posZ);

            for (int i = 1; i < m_tileMarkers.Length; i++)
            {
                UpdateMinAndMax(m_tileMarkers[i]);
            }

            //Debug.Log("MAX - " + max + "; MIN - " + min);

            gridSize = GetGridSize(out Vector2 centerOffset);

            //Debug.Log("GRIDSIZE: " + gridSize);

            UpdateTilesGridPosition(centerOffset);
        }

        private void UpdateMinAndMax(TileMarker tileMarker)
        {
            float posX = tileMarker.transform.localPosition.x + 0.5f;
            float posZ = tileMarker.transform.localPosition.z + 0.5f;

            //Debug.Log(posX + " " + posZ);

            if (min.x > posX) min.x = posX;
            if (max.x < posX) max.x = posX;

            if (min.y > posZ) min.y = posZ;
            if (max.y < posZ) max.y = posZ;
        }

        private int GetGridSize(out Vector2 centerOffset)
        {
            float GridX = max.x - min.x + 1;
            float GridY = max.y - min.y + 1;

            float value = GridX > GridY ? GridX : GridY;

            centerOffset = new Vector2((value - GridX)/2, (value - GridY)/2);

            return (int) value;
        }

        private void UpdateTilesGridPosition(Vector2 centerOffset)
        {
            float offsetX = centerOffset.x;
            float offsetY = centerOffset.y;

            if (min.x != 0)
            {
                if (min.x < 0 && max.x > 0) offsetX += Mathf.Abs(min.x);
                else if (min.x < 0 && max.x < 0 || min.x > 0 && max.x > 0) offsetX += gridSize - max.x;
                else if (max.x == 0) offsetX += gridSize;
            }

            if (min.y != 0)
            {
                if (min.y < 0 && max.y > 0) offsetY += Mathf.Abs(min.y);
                else if (min.y < 0 && max.y < 0 || min.y > 0 && max.y > 0) offsetY += gridSize - max.y;
                else if (max.y == 0) offsetY += gridSize;
            }

            //Debug.Log("OFFSET - " + offset);

            for (int i = 0; i < m_tileMarkers.Length; i++)
            {
                m_tileMarkers[i].ChangeGridPosition(offsetX, offsetY);
            }
        }
    }
}
