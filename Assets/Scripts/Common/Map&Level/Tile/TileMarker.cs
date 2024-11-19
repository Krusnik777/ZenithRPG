using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public enum MarkerType
    {
        Empty,
        Floor,
        Wall,
        Pit,
        HiddenPit,
        Trap,
        Door,
        Shop,
        Obstacle,
        Treasure,
        Water,
        StairsUp,
        StairsDown
    }

    public class TileMarker : MonoBehaviour
    {
        [SerializeField] private Tile m_parentTile;
        [SerializeField] private List<TileMarker> m_neighbours;
        [SerializeField] private MarkerType m_type;

        public Tile ParentTile => m_parentTile;
        public MarkerType Type => m_type;

        private Vector2 gridPos;
        public Vector2 PositionInGrid => gridPos;

        private UITile uITile;

        

        public void ChangeGridPosition(float offsetX, float offsetY)
        {
            gridPos = new Vector2(transform.localPosition.x + offsetX + 0.5f, transform.localPosition.z + offsetY + 0.5f);

            //Debug.Log("TransformPos: " + transform.localPosition + " : GridPos: " + gridPos);
        }

        public void SubscribeToDiscovered(UITile uITile)
        {
            this.uITile = uITile;

            if (m_parentTile == null) return;

            UnityAction onDiscovered = null;

            onDiscovered = () =>
            {
                this.uITile.SetIcon(m_type, true);

                for (int i = 0; i < m_neighbours.Count; i++)
                {
                    m_neighbours[i].uITile.SetIcon(m_neighbours[i].Type, true);
                }

                m_parentTile.EventOnDiscovered -= onDiscovered;
            };

            m_parentTile.EventOnDiscovered += onDiscovered;
        }

        public void SubscribeToChanges(UITile uITile)
        {
            if (m_parentTile == null) return;

            if (m_type == MarkerType.HiddenPit)
            {
                UnityAction onChange = null;

                onChange = () =>
                {
                    m_type = MarkerType.Pit;
                    uITile.ChangeIcon(m_type);
                    m_parentTile.EventOnStateChange -= onChange;
                };

                m_parentTile.EventOnStateChange += onChange;
            }
        }


        #if UNITY_EDITOR

        [ContextMenu(nameof(FindNeigboursForAllMarkers))]
        private void FindNeigboursForAllMarkers()
        {
            if (Application.isPlaying) return;

            TileMarker[] markers = FindObjectsOfType<TileMarker>();

            for (int i = 0; i < markers.Length; i++)
            {
                markers[i].FindNeigbours();
            }
        }

        private void FindNeigbours()
        {
            m_neighbours = new List<TileMarker>();

            LookForNeighbour(Vector3.forward);
            LookForNeighbour(-Vector3.forward);
            LookForNeighbour(Vector3.right);
            LookForNeighbour(-Vector3.right);
            LookForNeighbour(-Vector3.forward + Vector3.right);
            LookForNeighbour(-Vector3.forward - Vector3.right);
            LookForNeighbour(Vector3.forward + Vector3.right);
            LookForNeighbour(Vector3.forward - Vector3.right);

            UnityEditor.EditorUtility.SetDirty(this);
        }

        private void LookForNeighbour(Vector3 direction)
        {
            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
            Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

            foreach (var collider in colliders)
            {
                TileMarker tile = collider.GetComponentInParent<TileMarker>();

                if (collider.isTrigger) continue;

                if (tile != null && !m_neighbours.Contains(tile))
                {
                    m_neighbours.Add(tile);
                }
            }
        }

#endif
    }
}
