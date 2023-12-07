using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class Tile : MonoBehaviour
    {
        private List<Tile> neighborTiles = new List<Tile>();
        public List<Tile> NeighborTiles { get => neighborTiles; set => neighborTiles = value; }

        [SerializeField] private bool m_walkable = true;
        [SerializeField] private Collider m_targetCollider;

        public bool Walkable { get => m_walkable; set => m_walkable = value; }
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

        public void SetTargetTileCollider(bool state)
        {
            m_targetCollider.enabled = state;
        }

        public void FindNeighbors(Tile target)
        {
            Reset();

            CheckTile(Vector3.forward, target);
            CheckTile(-Vector3.forward, target);
            CheckTile(Vector3.right, target);
            CheckTile(-Vector3.right, target);
        }

        public void Reset()
        {
            neighborTiles.Clear();

            m_targetCollider.enabled = false;

            Current = false;
            Target = false;
            Selectable = false;

            Visited = false;
            ParentTile = null;
            Distance = 0;

            f = g = h = 0;
        }

        private void CheckTile(Vector3 direction, Tile target)
        {
            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
            Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

            foreach (var collider in colliders)
            {
                Tile tile = collider.GetComponentInParent<Tile>();

                if (tile != null && tile.Walkable)
                {
                    if (!Physics.Raycast(tile.transform.position, Vector3.up, 1f, 1, QueryTriggerInteraction.Ignore) || tile == target)
                    {
                        neighborTiles.Add(tile);
                    }
                }   
            }
        }

    }
}
