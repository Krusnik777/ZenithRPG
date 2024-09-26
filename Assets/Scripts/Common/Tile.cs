using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public struct OccupiedState
    {
        private bool isOccupied;
        private CharacterAvatar occupiedBy;
        public bool State => isOccupied;
        public CharacterAvatar By => occupiedBy;

        public OccupiedState(bool isOccupied = false, CharacterAvatar occupiedBy = null)
        {
            this.isOccupied = isOccupied;
            this.occupiedBy = occupiedBy;
        }

        public void SetOccupied(bool isOccupied, CharacterAvatar occupiedBy)
        {
            this.isOccupied = isOccupied;
            this.occupiedBy = occupiedBy;
        }
    }

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

        private OccupiedState m_occupied;
        public OccupiedState Occupied => m_occupied;

        // Path Finding

        // Breadth-first search (BFS)
        public Tile ParentTile { get; set; }

        // A*
        public float f { get; set; }
        public float g { get; set; }
        public float h { get; set; }

        public void SetTileOccupied(CharacterAvatar occupiedBy) => m_occupied.SetOccupied(occupiedBy != null, occupiedBy);

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

        public Tile[] FindTwoForwardTiles(Vector3 direction)
        {
            Tile[] tiles = new Tile[2];

            tiles[0] = FindNeighbourByDirection(direction);

            if (tiles[0] != null) tiles[1] = tiles[0].FindNeighbourByDirection(direction);

            return tiles;
        }

        public void FindNeighbors()
        {
            Reset();

            CheckTile(Vector3.forward);
            CheckTile(-Vector3.forward);
            CheckTile(Vector3.right);
            CheckTile(-Vector3.right);
        }

        public void ResetPathFindingValues()
        {
            f = g = h = 0;

            ParentTile = null;

            //m_occupied = new OccupiedState();
        }

        public void Reset()
        {
            neighborTiles.Clear();

            ParentTile = null;

            f = g = h = 0;

            m_occupied = new OccupiedState();
        }

        private void CheckTile(Vector3 direction)
        {
            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
            Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

            foreach (var collider in colliders)
            {
                Tile tile = collider.GetComponentInParent<Tile>();

                if (tile != null && !neighborTiles.Contains(tile))
                {
                    neighborTiles.Add(tile);
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
