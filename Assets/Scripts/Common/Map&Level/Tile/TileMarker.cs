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
        [SerializeField] private Tile[] m_neighbourTiles;
        [SerializeField] private MarkerType m_type;

        public Tile ParentTile => m_parentTile;
        public Tile[] NeighbourTiles => m_neighbourTiles;
        public MarkerType Type => m_type;

        private Vector2 gridPos;
        public Vector2 PositionInGrid => gridPos;

        public void ChangeGridPosition(float offsetX, float offsetY)
        {
            gridPos = new Vector2(transform.localPosition.x + offsetX + 0.5f, transform.localPosition.z + offsetY + 0.5f);

            //Debug.Log("TransformPos: " + transform.localPosition + " : GridPos: " + gridPos);
        }

        public void SubscribeToChanges(TileData tileData, UITile uITile)
        {
            UnityAction onDiscovered = null;

            if (m_parentTile != null)
            {
                onDiscovered = () =>
                {
                    OnTileMarketDiscovered(tileData, uITile);
                    Debug.Log($"HERE - {tileData.PositionInGrid}");
                    m_parentTile.EventOnDiscovered -= onDiscovered;
                };

                m_parentTile.EventOnDiscovered += onDiscovered;

                if (m_type == MarkerType.HiddenPit)
                {
                    UnityAction onChange = null;

                    onChange = () =>
                    {
                        m_type = MarkerType.Pit;
                        tileData.MarkerType = m_type;
                        uITile.SetIcon(m_type);
                        m_parentTile.EventOnStateChange -= onChange;
                    };

                    m_parentTile.EventOnStateChange += onChange;
                }
            }
            else
            {
                if (m_neighbourTiles.Length == 0)
                {
                    //Debug.LogError("NeighbourTiles are empty");
                    return;
                }

                onDiscovered = () =>
                {
                    OnTileMarketDiscovered(tileData, uITile);
                    Debug.Log($"HERE - {tileData.PositionInGrid}");

                    for (int j = 0; j < m_neighbourTiles.Length; j++)
                    {
                        m_neighbourTiles[j].EventOnDiscovered -= onDiscovered;
                    }
                };

                for (int i = 0; i < m_neighbourTiles.Length; i++)
                {
                    m_neighbourTiles[i].EventOnDiscovered += onDiscovered;
                }

            }
        }

        private void OnTileMarketDiscovered(TileData tileData, UITile uITile)
        {
            tileData.Discovered = true;
            uITile.SetIcon(m_type);
        }
    }
}
