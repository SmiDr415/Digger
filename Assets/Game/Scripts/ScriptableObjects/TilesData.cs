using UnityEngine;
using UnityEngine.Tilemaps;

namespace MultiTool
{
    [CreateAssetMenu(fileName = "Tile Data SO", menuName = "ScriptableObjects/Tile Data SO", order = 1)]
    public class TilesData : ScriptableObject
    {
        [SerializeField] private TileData[] _tileDatas;

        public TileData[] TileDatas => _tileDatas;

        public TileType GetTileType(string tileName)
        {
            foreach(var tile in _tileDatas)
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
            foreach(var tileData in _tileDatas)
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
            foreach(var tileData in _tileDatas)
            {
                if(tileName.Contains(tileData.NameEn) || tileData.NameEn.Contains(tileName) || tileData.NameEn.EndsWith(tileName)|| tileData.NameEn.ToLower().Contains(tileName.ToLower()))
                {
                    return tileData;
                }
            }
            return null;
        }

        internal string GetRandomTileName(float y)
        {
            int index = y > Random.Range(0.0f, 1) ? 1 : 2;

            return _tileDatas[index].NameEn;
        }

        internal TileBase GetCrackTile(string name, int currentStrength)
        {
            var tileData = GetTileDataByName(name);
            var durability = (int)(((float)currentStrength / tileData.Durability[0]) * 10);

            var val = durability >= 5 ? durability >= 7 ? 0 : 1 : durability < 3 ? 3 : 2;
            var cracksData = GetTileDataByName("Crack");
            var res = cracksData.Tiles[val];
            return res;
        }
    }

}
