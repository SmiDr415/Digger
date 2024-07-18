using System;
using UnityEngine.Tilemaps;

namespace Digger
{

    [Serializable]
    public class TileData
    {
        public TileType TileType;
        public HarvestType HarvestType;
        public TileBase[] tiles;
        public int MiningLevel;
        public int[] Durability;
    }

}
