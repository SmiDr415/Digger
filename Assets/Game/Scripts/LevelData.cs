using System.Collections.Generic;
using UnityEngine;

namespace MultiTool
{
    /// <summary>
    /// Представляет данные уровня, полученные в результате парсинга JSON.
    /// </summary>
    [System.Serializable]
    public class LevelData
    {
        public int level; // номер уровня от нуля
        public int timeTreeStars; // время на три звезды
        public int timeTwoStars;
        public int gridWidth;                        // Ширина тайловой сетки
        public int gridHeight;                       // Высота тайловой сетки
        public Vector2Int startPosition;           // Координаты старта
        public Vector2Int finishPosition;          // Координаты финиша
        public List<CellData> cells;                     // Массив данных для каждой клетки
    }
}
