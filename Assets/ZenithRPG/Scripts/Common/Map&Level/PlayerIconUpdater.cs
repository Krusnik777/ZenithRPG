using UnityEngine;

namespace DC_ARPG
{
    public class PlayerIconUpdater : MonoBehaviour
    {
        [SerializeField] private RectTransform m_playerIcon;
        [SerializeField] private RectTransform m_map;

        private bool isReady;

        private int gridSize;

        private Vector2 mapOffset;
        private Vector2 worldOrigin;
        private float cellSize;

        public void Init(int gridSize, Vector2 worldOrigin, Vector2 cellSize)
        {
            this.gridSize = gridSize;
            this.worldOrigin = worldOrigin;
            this.cellSize = cellSize.x;

            m_playerIcon.sizeDelta = new Vector2(cellSize.x, cellSize.y);

            //mapOffset = new Vector2(posInGrid.y * cellSize.y - cellSize.x, posInGrid.x * cellSize.x - cellSize.y);

            float totalMapSize = gridSize * cellSize.x;
            mapOffset = new Vector2(-totalMapSize / 2f, -totalMapSize / 2f);

            //Debug.Log(tileData.TileMarker.transform.position + " " + tileData.TileMarker.PositionInGrid);

            //Debug.Log(gridSize + " " + posInGrid + " " + mapOffset);

            isReady = true;
        }

        private void LateUpdate()
        {
            if (!isReady) return;

            Vector2 currentGridPos = ConvertWorldToGridPosition(LevelState.Instance.Player.transform.position);
            UpdatePlayerIcon(currentGridPos);

            //Vector3 normalizedPosition = new Vector3(LevelState.Instance.Player.transform.position.x / (gridSize * 0.5f), 0, LevelState.Instance.Player.transform.position.z / (gridSize * 0.5f));
            //Vector3 positionInMinimap = new Vector3(normalizedPosition.x * m_map.sizeDelta.x * 0.5f, normalizedPosition.z * m_map.sizeDelta.y * 0.5f, 0);

            //m_playerIcon.transform.localPosition = m_map.transform.position + positionInMinimap - new Vector3(mapOffset.x, mapOffset.y);

            m_playerIcon.transform.rotation = new Quaternion(0, 0, -LevelState.Instance.Player.transform.rotation.y, LevelState.Instance.Player.transform.rotation.w);
        }

        private Vector2 ConvertWorldToGridPosition(Vector3 playerWorldPos)
        {
            float gridX = playerWorldPos.x - worldOrigin.x;
            float gridY = playerWorldPos.z - worldOrigin.y;

            return new Vector2(gridX, gridY);
        }

        private void UpdatePlayerIcon(Vector2 playerGridPos)
        {
            float posX = mapOffset.x + playerGridPos.x * cellSize;
            float posY = mapOffset.y + playerGridPos.y * cellSize;

            m_playerIcon.anchoredPosition = new Vector2(posX + cellSize * 0.5f, posY + cellSize * 0.5f);
        }
    }
}
