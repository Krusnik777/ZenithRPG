using UnityEngine;

namespace DC_ARPG
{
    public class LevelState : MonoSingleton<LevelState>, IDependency<Player>
    {
        private Tile[] m_levelTileField;
        public Tile[] LevelTileField => m_levelTileField;

        private Enemy[] m_allEnemies;
        public Enemy[] AllEnemies => m_allEnemies;

        private Player m_player;
        public void Construct(Player player) => m_player = player;

        private void Start()
        {
            m_levelTileField = FindObjectsOfType<Tile>();
            //ComputeAdjacencyList(m_levelTileField, null);
        }

        private void Update()
        {
            //ComputeAdjacencyList(m_levelTileField, null);
        }

        private void ComputeAdjacencyList(Tile[] tileField, Tile target)
        {
            foreach (var tile in tileField)
            {
                tile.FindNeighbors(target);
            }
        }
    }
}
