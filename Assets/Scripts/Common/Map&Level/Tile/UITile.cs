using UnityEngine;
using UnityEngine.UI;

namespace DC_ARPG
{
    public class UITile : MonoBehaviour
    {
        [SerializeField] private Image m_mapIcon;
        [SerializeField] private MapIconsInfo m_mapIconsInfo;

        private TileData parentTile;
        public TileData ParentTile => parentTile;
        private Vector2 gridPos;
        public Vector2 PositionInGrid => gridPos;

        public void SetGridPos(Vector2 gridPos) => this.gridPos = gridPos;
        public void SetTileData(TileData tileData) => parentTile = tileData;

        public void ChangeIcon(MarkerType marker)
        {
            parentTile.MarkerType = marker;
            m_mapIcon.sprite = m_mapIconsInfo.GetSpriteByMarker(marker);
        }

        public void SetIcon(MarkerType marker, bool discovered)
        {
            m_mapIcon.sprite = m_mapIconsInfo.GetSpriteByMarker(marker);

            Color color = m_mapIcon.color;

            color.a = discovered ? 1f : 0f;

            m_mapIcon.color = color;

            if (parentTile != null) parentTile.Discovered = discovered;
        }
    }
}
