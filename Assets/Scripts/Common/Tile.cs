using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class Tile : MonoBehaviour
    {
        public List<Tile> NeighborTiles = new List<Tile>();

        public bool Current { get; set; }
        public bool Target { get; set; }
        public bool Selectable { get; set; }

        // Breadth-first search (BFS)
        public bool Visited { get; set; }
        public Tile ParentTile { get; set; }
        public int Distance { get; set; }

        public void FindNeighbors()
        {
            Reset();

            CheckTile(Vector3.forward);
            CheckTile(-Vector3.forward);
            CheckTile(Vector3.right);
            CheckTile(-Vector3.right);
        }

        public void Reset()
        {
            NeighborTiles.Clear();

            Current = false;
            Target = false;
            Selectable = false;

            Visited = false;
            ParentTile = null;
            Distance = 0;
        }

        private void CheckTile(Vector3 direction)
        {
            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
            Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

            foreach (var collider in colliders)
            {
                Tile tile = collider.GetComponentInParent<Tile>();
                if (tile != null)
                {
                    if (!Physics.Raycast(tile.transform.position, Vector3.up, 1f, 1, QueryTriggerInteraction.Ignore))
                    {
                        NeighborTiles.Add(tile);
                    }
                }   
            }
        }

    }
}
