using UnityEngine;
using UnityEngine.Tilemaps;

namespace MultiTool
{

    /// <summary>
    /// Генерирует тайловую карту на основе данных из JSON-файлов для каждого уровня.
    /// </summary>
    public class TileMapGenerator : MonoBehaviour
    {
        [SerializeField] private TilemapStrengthDisplay _strengthDisplay;
        [SerializeField] private Tilemap _tilemap;                  // Ссылка на компонент Tilemap
        [SerializeField] private TilesData _tileDataSO;              // Ссылка на ScriptableObject с информацией о тайлах
        [SerializeField] private TextAsset[] _levelData;            // Массив JSON-файлов для уровней

        private LevelData _currentLevelData;                        // Хранение текущих данных уровня

        /// <summary>
        /// Очищает текущую тайловую карту.
        /// </summary>
        public void ClearTileMap()
        {
            _tilemap.ClearAllTiles();
        }

        /// <summary>
        /// Загружает данные уровня из JSON-файла.
        /// </summary>
        /// <param name="file">JSON-файл уровня в формате TextAsset.</param>
        private void LoadLevelData(TextAsset file)
        {
            if(file == null)
            {
                Debug.LogError("LoadLevelData: Указанный JSON-файл пуст.");
                return;
            }

            _currentLevelData = JsonUtility.FromJson<LevelData>(file.text);
        }

        /// <summary>
        /// Генерирует тайлы на карте на основе данных уровня.
        /// </summary>
        private void GenerateTiles()
        {
            ClearTileMap();

            if(_currentLevelData == null)
            {
                Debug.LogError("GenerateTiles: Данные уровня не были загружены.");
                return;
            }

            foreach(var cell in _currentLevelData.cells)
            {
                TileBase tile = _tileDataSO.GetTileDataByName(cell.tileName)?.Tiles[0];

                if(tile != null)
                {
                    _tilemap.SetTile((Vector3Int)cell.position, tile);
                }
            }

            Debug.Log($"Стартовая позиция: {_currentLevelData.startPosition}, Финишная позиция: {_currentLevelData.finishPosition}");
            PlayerController.Instance.transform.position = new Vector3(_currentLevelData.startPosition.x, _currentLevelData.startPosition.y, 0);
            _strengthDisplay.InitializeTileStrengthDict();
            // Здесь можно добавить логику для использования стартовой и финишной позиции в игре
        }

        /// <summary>
        /// Устанавливает уровень и загружает его данные.
        /// </summary>
        /// <param name="levelIndex">Индекс уровня в массиве levelData.</param>
        public void SetLevel(int levelIndex)
        {
            if(levelIndex < 0 || levelIndex >= _levelData.Length)
            {
                Debug.LogError("SetLevel: Недопустимый индекс уровня.");
                return;
            }

            LoadLevelData(_levelData[levelIndex]);
            GenerateTiles();
        }
    }
}
