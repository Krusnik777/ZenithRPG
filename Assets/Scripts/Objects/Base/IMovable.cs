namespace DC_ARPG
{
    public interface IMovable
    {
        public Tile CurrentTile { get; }
        public bool InMovement { get; }

        public Tile GetCurrentTile();
    }
}
