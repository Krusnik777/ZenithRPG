using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class TileData
    {
        public bool Discovered;
        public MarkerType MarkerType;
        public Vector2 PositionInGrid;

        public TileMarker TileMarker { get; private set; }

        public TileData(TileMarker marker)
        {
            Discovered = false;
            MarkerType = marker.Type;
            PositionInGrid = marker.PositionInGrid;

            TileMarker = marker;
        }
    }
}
