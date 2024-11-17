namespace DC_ARPG
{
    [System.Serializable]
    public class Map
    {
        public TileData[] Tiles;
        string Name;
        public int GridSize;

        public Map(TileMarker[] markers, string name, int gridSize)
        {
            Name = name;
            GridSize = gridSize;

            Tiles = new TileData[markers.Length];

            for (int i = 0; i < markers.Length; i++)
            {
                Tiles[i] = new TileData(markers[i]);
            }
        }
    }
}
