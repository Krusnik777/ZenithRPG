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

        private Door[] m_allDoorsOnLevel;
        public Door[] AllDoorsOnLevel => m_allDoorsOnLevel;

        private ShopDoor[] m_allShops;
        public ShopDoor[] AllShops => m_allShops;

        private Player m_player;
        public void Construct(Player player) => m_player = player;
        public Player Player => m_player;

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

        public void ComputeAdjacencyList(Tile target)
        {
            foreach (var tile in m_levelTileField)
            {
                tile.FindNeighbors(target);
            }
        }

        private void Start()
        {
            m_levelTileField = FindObjectsOfType<Tile>();

            m_allEnemies = new List<Enemy>();
            m_allEnemies.AddRange(FindObjectsOfType<Enemy>());

            m_allDoorsOnLevel = FindObjectsOfType<Door>();
            m_allShops = FindObjectsOfType<ShopDoor>();

            StoryEventManager.Instance.EventOnStoryEventStarted += StopAllActivity;
            StoryEventManager.Instance.EventOnStoryEventEnded += ResumeAllActivity;
            
            foreach (var enemy in m_allEnemies)
            {
                enemy.EventOnDeath += OnEnemyDeath;
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
            m_allEnemies.Remove(enemy);
        }



        private void Update()
        {
            //ComputeAdjacencyList(m_levelTileField, null);
        }
    }
}
