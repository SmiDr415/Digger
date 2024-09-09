using UnityEngine;
using UnityEngine.Tilemaps;

namespace MultiTool
{
    public class TileMapGenerator : MonoBehaviour
    {
        [SerializeField] private TilemapStrengthDisplay _strengthDisplay;
        [SerializeField] private Tilemap _tilemap;                  // —сылка на компонент Tilemap
        [SerializeField] private TilesData _tileDataSO;              // —сылка на ScriptableObject с информацией о тайлах

        public void ClearTileMap()
        {
            _tilemap.ClearAllTiles();
        }


        public void GenerateTiles(LevelData levelData)
        {
            ClearTileMap();

            foreach(var cell in levelData.cells)
            {
                TileBase tile = _tileDataSO.GetTileDataByName(cell.tileName)?.Tiles[0];

                if(tile != null)
                {
                    _tilemap.SetTile((Vector3Int)cell.position, tile);
                }
            }

            PlayerController.Instance.transform.position = new Vector3(levelData.startPosition.x, levelData.startPosition.y, 0);
            _strengthDisplay.InitializeTileStrengthDict();
        }


    }
}
