using System;
using UnityEngine.Tilemaps;

namespace Digger
{

    [Serializable]
    public class TileData
    {
        public TileType Type;
        public TileBase[] tiles;
        public int[] strength;
    }

}
