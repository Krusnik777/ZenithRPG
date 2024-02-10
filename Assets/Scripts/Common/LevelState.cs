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

        private Door[] m_allDoorsOnLevel;
        public Door[] AllDoorsOnLevel => m_allDoorsOnLevel;

        private ShopDoor[] m_allShops;
        public ShopDoor[] AllShops => m_allShops;

        private Player m_player;
        public void Construct(Player player) => m_player = player;
        public Player Player => m_player;

        public List<Tile> GetNeighborsTilesToPlayer()
        {
            Tile playerTile = null;

            Ray ray = new Ray(m_player.transform.position + new Vector3(0, 0.1f, 0), -Vector3.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1f, 1, QueryTriggerInteraction.Ignore))
            {
                playerTile = hit.collider.GetComponentInParent<Tile>();
            }

            if (playerTile == null) return null;

            if (playerTile.NeighborTiles == null) playerTile.FindNeighbors();

            return playerTile.NeighborTiles;
        }

        public void StopAllActivity()
        {
            foreach(var enemy in m_allEnemies)
            {
                if (enemy.EnemyAI != null) enemy.EnemyAI.StopActivity();
            }
        }

        public void ResumeAllActivity()
        {
            foreach (var enemy in m_allEnemies)
            {
                if (enemy.EnemyAI != null) enemy.EnemyAI.ResumeActivity();
            }
        }

        public void ComputeAdjacencyList(bool notIncludeTileWithPlayer = true)
        {
            foreach (var tile in m_levelTileField)
            {
                tile.FindNeighbors(null, notIncludeTileWithPlayer);
            }
        }

        public void ComputeAdjacencyList(Tile target)
        {
            foreach (var tile in m_levelTileField)
            {
                tile.FindNeighbors(target);
            }
        }

        public Tile GetTargetNearPlayer(EnemyAIController chasingEnemyAI)
        {
            if (!m_chasingEnemies.Contains(chasingEnemyAI)) return null;

            var possibleTargetTiles = GetNeighborsTilesToPlayer();

            if (possibleTargetTiles == null) return null;

            for (int i = 0; i < possibleTargetTiles.Count; i++)
            {
                if (m_chasingEnemies.IndexOf(chasingEnemyAI) == i)
                {
                    return possibleTargetTiles[i];
                }
            }

            return null;
        }

        private void Start()
        {
            m_levelTileField = FindObjectsOfType<Tile>();

            m_allEnemies = new List<Enemy>();
            m_allEnemies.AddRange(FindObjectsOfType<Enemy>());

            m_chasingEnemies = new List<EnemyAIController>();

            m_allDoorsOnLevel = FindObjectsOfType<Door>();
            m_allShops = FindObjectsOfType<ShopDoor>();

            StoryEventManager.Instance.EventOnStoryEventStarted += StopAllActivity;
            StoryEventManager.Instance.EventOnStoryEventEnded += ResumeAllActivity;
            
            foreach (var enemy in m_allEnemies)
            {
                enemy.EventOnDeath += OnEnemyDeath;

                enemy.EnemyAI.EventOnChaseStarted += AddChasingEnemyToList;
                enemy.EnemyAI.EventOnChaseStopped += RemoveChasingEnemyToList;
            }

            foreach(var door in m_allDoorsOnLevel)
            {
                door.EventOnDoorOpened += OnAnyDoorStateChanged;
                door.EventOnDoorClosed += OnAnyDoorStateChanged;
            }

            foreach(var shop in m_allShops)
            {
                shop.EventOnShopEntered += StopAllActivity;
                shop.EventOnShopExited += ResumeAllActivity;
            }
        }

        private void OnDestroy()
        {
            StoryEventManager.Instance.EventOnStoryEventStarted -= StopAllActivity;
            StoryEventManager.Instance.EventOnStoryEventEnded -= ResumeAllActivity;

            foreach (var door in m_allDoorsOnLevel)
            {
                door.EventOnDoorOpened -= OnAnyDoorStateChanged;
                door.EventOnDoorClosed -= OnAnyDoorStateChanged;
            }

            foreach (var shop in m_allShops)
            {
                shop.EventOnShopEntered -= StopAllActivity;
                shop.EventOnShopExited -= ResumeAllActivity;
            }
        }

        private void OnAnyDoorStateChanged()
        {
            foreach (var enemy in m_allEnemies)
            {
                if (enemy.EnemyAI != null) enemy.EnemyAI.UpdateActivity();
            }
        }

        private void OnEnemyDeath(Enemy enemy)
        {
            enemy.EventOnDeath -= OnEnemyDeath;
            enemy.EnemyAI.EventOnChaseStarted -= AddChasingEnemyToList;
            enemy.EnemyAI.EventOnChaseStopped -= RemoveChasingEnemyToList;

            RemoveChasingEnemyToList(enemy.EnemyAI);

            m_allEnemies.Remove(enemy);
        }

        private void AddChasingEnemyToList(EnemyAIController chasingEnemyAI)
        {
            m_chasingEnemies.Add(chasingEnemyAI);
        }

        private void RemoveChasingEnemyToList(EnemyAIController chasingEnemyAI)
        {
            if (m_chasingEnemies.Contains(chasingEnemyAI))
            {
                m_chasingEnemies.Remove(chasingEnemyAI);
            }
        }
    }
}
