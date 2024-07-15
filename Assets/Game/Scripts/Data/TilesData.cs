using UnityEngine;
using UnityEngine.Tilemaps;

namespace Digger
{
    [CreateAssetMenu(fileName = "TilesData", menuName = "Tiles Data", order = 1)]
    public class TilesData : ScriptableObject
    {
        public TileData[] tileDatas;

        public TileType GetTileType(string tileName)
        {
            foreach(var tile in tileDatas)
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

        public TileBase GetTileByStrength(string tileName, int strength)
        {
            foreach(var tileData in tileDatas)
            {
                for(int i = 0; i < tileData.tiles.Length; i++)
                {
                    if(tileData.tiles[i].name.Contains(tileName))
                    {
                        if(tileData.strength[i] > strength)
                        {
                            return tileData.tiles[i + 1];
                        }
                    }
                }
            }
            return null;
        }
    }

}
