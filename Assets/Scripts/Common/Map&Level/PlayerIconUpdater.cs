using UnityEngine;

namespace DC_ARPG
{
    public class PlayerIconUpdater : MonoBehaviour
    {
        [SerializeField] private RectTransform m_playerIcon;
        [SerializeField] private RectTransform m_map;

        bool isReady;

        int gridSize;

        Vector2 mapOffset;

        public void Init(int gridSize, Vector2 cellSize, Vector2 posInGrid)
        {
            this.gridSize = gridSize;

            m_playerIcon.sizeDelta = new Vector2(cellSize.x, cellSize.y);

            mapOffset = new Vector2(posInGrid.y * cellSize.y - cellSize.x, posInGrid.x * cellSize.x - cellSize.y);

            //Debug.Log(tileData.TileMarker.transform.position + " " + tileData.TileMarker.PositionInGrid);

            //Debug.Log(mapOffset);

            isReady = true;
        }

        private void LateUpdate()
        {
            if (!isReady) return;

            Vector3 normalizedPosition = new Vector3(LevelState.Instance.Player.transform.position.x / (gridSize * 0.5f), 0, LevelState.Instance.Player.transform.position.z / (gridSize * 0.5f));
            Vector3 positionInMinimap = new Vector3(normalizedPosition.x * m_map.sizeDelta.x * 0.5f, normalizedPosition.z * m_map.sizeDelta.y * 0.5f, 0);

            m_playerIcon.transform.position = m_map.transform.position + positionInMinimap - new Vector3(mapOffset.x, mapOffset.y);
            m_playerIcon.transform.rotation = new Quaternion(0, 0, -LevelState.Instance.Player.transform.rotation.y, LevelState.Instance.Player.transform.rotation.w);
        }
    }
}
