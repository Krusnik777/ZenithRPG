using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "MapIconsInfo", menuName = "ScriptableObjects/MapIconsInfo")]
    public class MapIconsInfo : ScriptableObject
    {
        public Sprite Empty;
        public Sprite Floor;
        public Sprite Wall;
        public Sprite Pit;
        public Sprite Trap;
        public Sprite Door;
        public Sprite Shop;
        public Sprite Obstacle;
        public Sprite Treasure;
        public Sprite Water;
        public Sprite StairsUp;
        public Sprite StairsDown;

        public Sprite GetSpriteByMarker(MarkerType marker)
        {
            Sprite sprite = Empty;

            switch(marker)
            {
                case MarkerType.Empty: sprite = Empty; break;
                case MarkerType.Floor: sprite = Floor; break;
                case MarkerType.Wall: sprite = Wall; break;
                case MarkerType.Pit: sprite = Pit; break;
                case MarkerType.HiddenPit: sprite = Floor; break;
                case MarkerType.Trap: sprite = Trap; break;
                case MarkerType.Door: sprite = Door; break;
                case MarkerType.Shop: sprite = Shop; break;
                case MarkerType.Obstacle: sprite = Obstacle; break;
                case MarkerType.Treasure: sprite = Treasure; break;
                case MarkerType.Water: sprite = Water; break;
                case MarkerType.StairsUp: sprite = StairsUp; break;
                case MarkerType.StairsDown: sprite = StairsDown; break;
                default: sprite = Empty; break;
            }

            return sprite;
        }
    }
}
