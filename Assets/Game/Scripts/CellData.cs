using UnityEngine;

namespace MultiTool
{
    /// <summary>
    /// Представляет данные для одной клетки уровня.
    /// </summary>
    [System.Serializable]
    public class CellData
    {
        public Vector2Int position;
        public string tileName;      // Название тайла
    }
}
