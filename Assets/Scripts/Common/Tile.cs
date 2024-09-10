using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public enum TileType
    {
        Walkable,
        Mechanism,
        Pit,
        Obstacle,
        Closable
    }

    public class Tile : MonoBehaviour
    {
        private List<Tile> neighborTiles = new List<Tile>();
        public List<Tile> NeighborTiles { get => neighborTiles; set => neighborTiles = value; }

        [SerializeField] private TileType m_type;
        [SerializeField] private InspectableObject m_objectOnTile;
        public TileType Type => m_type;

        private bool m_occupied;
        public bool Occupied => m_occupied;

        // Path Finding
        public bool Current { get; set; }
        public bool Target { get; set; }
        public bool Selectable { get; set; }

        // Breadth-first search (BFS)
        public bool Visited { get; set; }
        public Tile ParentTile { get; set; }
        public int Distance { get; set; }

        // A*
        public float f { get; set; }
        public float g { get; set; }
        public float h { get; set; }

        public void SetTileOccupied(bool occupied) => m_occupied = occupied;

        public bool CheckClosed()
        {
            if (m_objectOnTile == null)
            {
                m_type = TileType.Walkable;
                return false;
            }

            return !m_objectOnTile.Disabled;
        }

        public bool CheckMechanism()
        {
            if (m_objectOnTile == null)
            {
                m_type = TileType.Walkable;
                return true;
            }

            return m_objectOnTile.Disabled;
        }

        public bool CheckTileOccupied()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up * 0.6f, 0.45f);

            foreach (var collider in colliders)
            {
                if (!collider.isTrigger)
                {
                    return true;
                }  
            }
            return false;
        }

        public Tile FindNeighbourByDirection(Vector3 direction)
        {
            Tile tile = null;

            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
            Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

            foreach (var collider in colliders)
            {
                tile = collider.GetComponentInParent<Tile>();

                if (tile != null) return tile;
            }

            return tile;
        }

        public void FindNeighbors(bool notIncludeTileWithPlayer = true)
        {
            Reset();

            CheckTile(Vector3.forward, notIncludeTileWithPlayer);
            CheckTile(-Vector3.forward, notIncludeTileWithPlayer);
            CheckTile(Vector3.right, notIncludeTileWithPlayer);
            CheckTile(-Vector3.right, notIncludeTileWithPlayer);
        }

        public void Reset()
        {
            neighborTiles.Clear();

            Current = false;
            Target = false;
            Selectable = false;

            Visited = false;
            ParentTile = null;
            Distance = 0;

            f = g = h = 0;
        }

        private void CheckTile(Vector3 direction, bool notIncludeTileWithPlayer = true)
        {
            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
            Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

            foreach (var collider in colliders)
            {
                Tile tile = collider.GetComponentInParent<Tile>();

                if (tile != null /*&& tile.CheckTileOccupied() == false*/)
                {
                    if (!Physics.Raycast(tile.transform.position, Vector3.up, 1f, 1, QueryTriggerInteraction.Ignore))
                    {
                        neighborTiles.Add(tile);
                    }
                }   
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.6f, 0.45f);
        }
        #endif
    }
}
