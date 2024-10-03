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

        private CharacterAvatar m_occupiedBy;
        public CharacterAvatar OccupiedBy => m_occupiedBy;

        // Path Finding

        // Breadth-first search (BFS)
        public Tile ParentTile { get; set; }
        public CharacterAvatar TargetedBy { get; set; }

        // A*
        public float f { get; set; }
        public float g { get; set; }
        public float h { get; set; }

        public void SetTileOccupied(CharacterAvatar characterAvatar) => m_occupiedBy = characterAvatar;

        public bool CheckClosed()
        {
            if (m_type != TileType.Closable) return false;

            if (m_objectOnTile == null)
            {
                m_type = TileType.Walkable;
                return false;
            }

            return !m_objectOnTile.Disabled;
        }

        public bool CheckMechanismDisabled()
        {
            if (m_type != TileType.Mechanism) return false;

            if (m_objectOnTile == null)
            {
                m_type = TileType.Walkable;
                return true;
            }

            return m_objectOnTile.Disabled;
        }

        public void GetTileReaction(CharacterAvatar characterAvatar = null)
        {
            if (m_type != TileType.Mechanism && m_type != TileType.Pit
                || m_objectOnTile == null || m_objectOnTile.Disabled) return;

            if (m_objectOnTile is not IActivableObject) return;

            (m_objectOnTile as IActivableObject).Activate(characterAvatar);
        }

        public void ReturnMechanismToDefault()
        {
            if (m_type != TileType.Mechanism || m_objectOnTile == null || m_objectOnTile.Disabled) return;

            if (m_objectOnTile is not IReturnableObject) return;

            (m_objectOnTile as IReturnableObject).ReturnToDefault();
        }

        public int GetDamageFromPit()
        {
            if (m_type != TileType.Pit || m_objectOnTile == null) return 0;

            if (m_objectOnTile is not Pit) return 0;

            var pit = m_objectOnTile as Pit;

            return pit.DamageAfterFall;
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
        }

        public void Reset()
        {
            neighborTiles.Clear();

            ParentTile = null;

            f = g = h = 0;

            //m_occupiedBy = null;

            TargetedBy = null;
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
