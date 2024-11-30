using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace DC_ARPG
{
    public class MapConstructor : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup m_gridLayout;
        [SerializeField] private UITile m_uiTilePrefab;
        // TEMP
        [SerializeField] private RectTransform m_playerIcon;
        [SerializeField] private RectTransform m_field;
        bool isReady;

        int _grindSize;

        Vector2 _offset;

        // TEMP END

        private Dictionary<Vector2, TileData> tilesDictionary;
        private Dictionary<TileData, UITile> uiTilesDictionary;

        //private List<UITile> uiTiles;

        public void ConstructMap(Map map)
        {
            TileData[] tiles = map.Tiles;

            tilesDictionary = new Dictionary<Vector2, TileData>();

            for (int i = 0; i < tiles.Length; i++)
            {
                tilesDictionary.Add(tiles[i].PositionInGrid, tiles[i]);
            }

            int gridSize = map.GridSize;
            _grindSize = gridSize;

            var rect = (m_gridLayout.transform as RectTransform).rect;

            m_gridLayout.cellSize = new Vector2(rect.width / gridSize, rect.height / gridSize);

            //uiTiles = new List<UITile>();

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    var uiTile = Instantiate(m_uiTilePrefab, m_gridLayout.transform);

                    var uiTilePosInGrid = new Vector2(j, i);

                    uiTile.SetGridPos(uiTilePosInGrid);

                    if (tilesDictionary.TryGetValue(uiTilePosInGrid, out TileData tileData))
                    {
                        if (tileData != null)
                        {
                            uiTile.SetTileData(tileData);

                            // Check for player
                            if (tileData.TileMarker.ParentTile != null)
                            {
                                var tile = tileData.TileMarker.ParentTile;

                                if (tile == LevelState.Instance.Player.CurrentTile)
                                {
                                    m_playerIcon.sizeDelta = new Vector2(m_gridLayout.cellSize.x, m_gridLayout.cellSize.y);

                                    //Debug.Log(tileData.TileMarker.transform.position + " " + tileData.TileMarker.PositionInGrid);

                                    _offset = new Vector2(tileData.TileMarker.PositionInGrid.y * m_gridLayout.cellSize.y - m_gridLayout.cellSize.x,
                                        tileData.TileMarker.PositionInGrid.x * m_gridLayout.cellSize.x - m_gridLayout.cellSize.y);

                                    //Debug.Log(_offset);
                                }
                            }

                            if (tileData.Discovered)
                            {
                                uiTile.SetIcon(tileData.MarkerType, true);
                            }
                            else
                            {
                                uiTile.SetIcon(MarkerType.Empty, false);
                                // If current level
                                tileData.TileMarker.SubscribeToDiscovered(uiTile);
                            }

                            // Subscribe to events if current level
                            tileData.TileMarker.SubscribeToChanges(uiTile);
                        }
                    }
                    else uiTile.SetIcon(MarkerType.Empty, false);

                    //uiTiles.Add(uiTile);
                }
            }

            //Debug.Log("Map Elements: " + uiTiles.Count);
            isReady = true;
        }

        private void LateUpdate()
        {
            if (!isReady) return;

            Vector3 normalizedPosition = new Vector3(LevelState.Instance.Player.transform.position.x / (_grindSize * 0.5f), 0, LevelState.Instance.Player.transform.position.z / (_grindSize * 0.5f));
            Vector3 positionInMinimap = new Vector3(normalizedPosition.x * m_field.sizeDelta.x * 0.5f, normalizedPosition.z * m_field.sizeDelta.y * 0.5f, 0);

            m_playerIcon.transform.position = m_field.transform.position + positionInMinimap - new Vector3(_offset.x, _offset.y);
            m_playerIcon.transform.rotation = new Quaternion(0, 0, -LevelState.Instance.Player.transform.rotation.y, LevelState.Instance.Player.transform.rotation.w);
        }

    }
}
