using UnityEngine;
using UnityEngine.Tilemaps;

namespace MultiTool
{
    [CreateAssetMenu(fileName = "TilesData", menuName = "Tiles Data", order = 1)]
    public class TilesData : ScriptableObject
    {
        public TileData[] tileDatas;

        public TileType GetTileType(string tileName)
        {
            foreach(var tile in tileDatas)
            {
                foreach(var tileData in tile.Tiles)
                {
                    if(tileData.name.Contains(tileName))
                    {
                        return tile.TileType;
                    }
                }
            }
            return TileType.Undefinit;
        }

        public TileBase GetTileByStrength(string tileName, int strength)
        {
            foreach(var tileData in tileDatas)
            {
                for(int i = 0; i < tileData.Tiles.Length - 1; i++)
                {
                    if(tileData.Tiles[i].name.Contains(tileName))
                    {
                        if(tileData.Durability[i] > strength)
                        {
                            return tileData.Tiles[i + 1];
                        }
                        else
                        {
                            return tileData.Tiles[i];
                        }
                    }
                }
            }
            return null;
        }

        public TileData GetTileDataByName(string tileName)
        {
            foreach(var tileData in tileDatas)
            {
                if(tileData.NameEn.Contains(tileName))
                {
                    return tileData;
                }
            }
            return null;
        }
    }

}
