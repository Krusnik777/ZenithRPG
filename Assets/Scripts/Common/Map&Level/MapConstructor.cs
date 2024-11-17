using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DC_ARPG
{
    public class MapConstructor : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup m_gridLayout;
        [SerializeField] private UITile m_uiTilePrefab;

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
                            if (tileData.Discovered) uiTile.SetIcon(tileData.MarkerType);
                            else
                            {
                                uiTile.SetIcon(MarkerType.Empty);
                                tileData.TileMarker.SubscribeToChanges(tileData, uiTile);
                            }

                            // Subscribe to events - TO DO
                        }
                    }
                    else uiTile.SetIcon(MarkerType.Empty);

                    //uiTiles.Add(uiTile);
                }
            }

            //Debug.Log("Map Elements: " + uiTiles.Count);
        }

    }
}
