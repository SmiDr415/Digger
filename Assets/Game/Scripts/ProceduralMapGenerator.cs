using System.Collections.Generic;
using UnityEngine;

namespace MultiTool
{
    public class ProceduralMapGenerator
    {
        private const int Border = 2;
        private int _width;
        private int _height;

        public ProceduralMapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
        }


        public LevelData GenerateRandomLevelData()
        {
            LevelData generatedLevelData = new()
            {
                cells = new List<CellData>()
            };

            for(int x = 0; x < _width; x++)
            {
                for(int y = 0; y < _height; y++)
                {
                    var cell = new CellData
                    {
                        position = new Vector2Int(x, y)
                    };

                    if(x == 0 || x == _width - 1 || y == 0 || y == _height - 1)
                    {
                        cell.tileName = "NotDestroy";
                    }
                    else
                    {
                        cell.tileName = (float)y / _height < Random.Range(0, 1.0f) ? "Stone" : "Dirt";
                    }

                    generatedLevelData.cells.Add(cell);
                }
            }

            generatedLevelData.startPosition = new Vector2Int(Random.Range(Border, _width - Border), Random.Range(Border, _height - Border));
            CellData startCell = GetPortalField(generatedLevelData, generatedLevelData.startPosition, "Start");
            generatedLevelData.cells.Add(startCell);

            generatedLevelData.finishPosition = new Vector2Int(Random.Range(Border, _width - Border), Random.Range(Border, _height - Border));
            var attempts = 1000;
            while(Vector2Int.Distance(generatedLevelData.startPosition, generatedLevelData.finishPosition) < _height / 2 - Border)
            {
                generatedLevelData.finishPosition = new Vector2Int(Random.Range(Border, _width - Border), Random.Range(Border, _height - Border));
                if(attempts-- <= 0)
                {
                    Debug.Log("Бесконечный цикл");
                    break;
                }
            }
            CellData finishCell = GetPortalField(generatedLevelData, generatedLevelData.finishPosition, "Finish");
            generatedLevelData.cells.Add((finishCell));

            _width++;
            _height++;

            return generatedLevelData;
        }


        private CellData GetPortalField(LevelData generatedLevelData, Vector2Int pos, string namePortal)
        {
            CellData portalCell = new()
            {
                position = pos,
                tileName = namePortal
            };

            for(var x = portalCell.position.x - 1; x <= portalCell.position.x + 1; x++)
            {
                for(var y = portalCell.position.y - 1; y <= portalCell.position.y + 1; y++)
                {
                    var emtyCell = generatedLevelData.cells.Find(cell => cell.position == new Vector2Int(x, y));
                    generatedLevelData.cells.Remove(emtyCell);
                }
            }

            return portalCell;
        }
    }

}
