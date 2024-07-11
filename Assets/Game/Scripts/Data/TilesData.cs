using UnityEngine;

namespace Digger
{
    [CreateAssetMenu(fileName = "TilesData", menuName = "Tiles Data", order = 1)]
    public class TilesData : ScriptableObject
    {
        public TileData[] tileDatas;

        public TileType GetTileType(string tileName)
        {
            foreach (var tile in tileDatas)
            {
                foreach(var tileData in tile.tiles)
                {
                    if(tileData.name.Contains(tileName))
                    {
                        return tile.Type;
                    }
                }
            }
            return TileType.Undefinit;
        }
    }

}
