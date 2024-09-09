using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;
using UnityEditor;

namespace MultiTool
{
    public class TileMapConstruct : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;                // —сылка на Tilemap
        [SerializeField] private TilesData _tileDataSO;           // ScriptableObject с данными о тайлах
        [SerializeField] private Text _saveInfo;

        [SerializeField] private int _level;
        [SerializeField] private int _timeTreeStars = 90;
        [SerializeField] private int _timeTwoStars = 120;

        [SerializeField] private TextAsset _fileJson;

        private bool _saveProcess;
        private ProceduralMapGenerator _proceduralMapGenerator;

        public async void SaveTileMap()
        {
            if(_saveProcess)
            {
                return;
            }
            _saveProcess = true;

            _saveInfo.text = "сохранение карты в файл";

            Dictionary<Vector2Int, string> tileMapData = new();
            Vector2Int startPos = Vector2Int.zero;
            Vector2Int finishPos = Vector2Int.zero;

            // ѕроходим по всем €чейкам Tilemap и сохран€ем информацию о заполненных €чейках.
            BoundsInt bounds = _tilemap.cellBounds;
            for(int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for(int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int localPlace = new(x, y, 0);
                    TileBase tile = _tilemap.GetTile(localPlace);
                    if(tile != null)
                    {
                        if(tile.name == "Start")
                        {
                            startPos = new Vector2Int(x, y);
                        }
                        else if(tile.name == "Finish")
                        {
                            finishPos = new Vector2Int(x, y);
                        }

                        string tileName = _tileDataSO.GetTileDataByName(tile.name).NameEn;
                        if(!string.IsNullOrEmpty(tileName))
                        {
                            tileMapData[new Vector2Int(x, y)] = tileName;
                        }
                    }
                }
            }

            // —оздаем объект LevelData и заполн€ем его данными.
            List<CellData> cellDataList = new();
            foreach(var tileEntry in tileMapData)
            {
                cellDataList.Add(new CellData { position = tileEntry.Key, tileName = tileEntry.Value });
            }

            LevelData levelData = new()
            {
                level = _level,
                gridWidth = _tilemap.size.x,
                gridHeight = _tilemap.size.y,
                startPosition = startPos,
                finishPosition = finishPos,
                cells = cellDataList,
                timeTreeStars = _timeTreeStars,
                timeTwoStars = _timeTwoStars
            };

            // —охран€ем в JSON файл асинхронно.
            string json = JsonUtility.ToJson(levelData, true);
            string filePath = Path.Combine(Application.dataPath, $"Game/Data/JSON/Level{_level}.json");
            await WriteToFileAsync(filePath, json);
            _saveInfo.text = "сохранение успешно";

            await Task.Delay(2000);
            _saveInfo.text = string.Empty;
            _saveProcess = false;
        }

        private async Task WriteToFileAsync(string filePath, string content)
        {
            try
            {
                using(StreamWriter writer = new(filePath, false))
                {
                    await writer.WriteAsync(content);
                    await writer.FlushAsync();
                }
                Debug.Log($"‘айл успешно записан по пути: {filePath}");

#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif            
            }
            catch(Exception ex)
            {
                Debug.LogError($"ќшибка при записи в файл: {ex.Message}");
            }
        }

        public void GenerateTiles()
        {
            _tilemap.ClearAllTiles();

            LevelData levelData = JsonUtility.FromJson<LevelData>(_fileJson.text);

            foreach(var cell in levelData.cells)
            {
                TileBase tile = _tileDataSO.GetTileDataByName(cell.tileName)?.Tiles[0];

                if(tile != null)
                {
                    _tilemap.SetTile((Vector3Int)cell.position, tile);
                }
            }

            _level = levelData.level;
            _timeTreeStars = levelData.timeTreeStars;
        }

        public void GenerateRandomMap()
        {
            _tilemap.ClearAllTiles();
            if(_proceduralMapGenerator == null)
            _proceduralMapGenerator = new(10, 20);
            LevelData levelData = _proceduralMapGenerator.GenerateRandomLevelData();

            foreach(var cell in levelData.cells)
            {
                TileBase tile = _tileDataSO.GetTileDataByName(cell.tileName)?.Tiles[0];

                if(tile != null)
                {
                    _tilemap.SetTile((Vector3Int)cell.position, tile);
                }
            }

            _level = levelData.level;
            _timeTreeStars = levelData.timeTreeStars;

        }

    }
}
