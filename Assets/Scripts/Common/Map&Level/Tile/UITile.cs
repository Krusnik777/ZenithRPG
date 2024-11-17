using UnityEngine;
using UnityEngine.UI;

namespace DC_ARPG
{
    public class UITile : MonoBehaviour
    {
        [SerializeField] private Image m_mapIcon;
        [SerializeField] private MapIconsInfo m_mapIconsInfo;

        private TileData parentTile;
        private Vector2 gridPos;
        public Vector2 PositionInGrid => gridPos;

        public void SetGridPos(Vector2 gridPos) => this.gridPos = gridPos;

        public void SetIcon(MarkerType marker)
        {
            m_mapIcon.sprite = m_mapIconsInfo.GetSpriteByMarker(marker);
        }
    }
}
