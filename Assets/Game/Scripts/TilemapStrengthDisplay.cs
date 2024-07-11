using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using TMPro;

namespace Digger
{
    public class TilemapStrengthDisplay : MonoBehaviour
    {
        [SerializeField]
        private Tilemap _tilemap;

        [SerializeField]
        private GameObject _textPrefab;

        [SerializeField]
        private TilesData _tileData;

        [SerializeField]
        private PlayerController _playerController;


        private Dictionary<TileBase, int> _tileStrengthDict;
        private Dictionary<Vector3Int, int> _tileCurrentStrengthDict;
        private Dictionary<Vector3Int, GameObject> _tileTextObjects;

        private void Start()
        {
            InitializeTileStrengthDict();
            DisplayStrengthOnTiles();
        }

        private void InitializeTileStrengthDict()
        {
            _tileStrengthDict = new Dictionary<TileBase, int>();
            _tileCurrentStrengthDict = new Dictionary<Vector3Int, int>();
            _tileTextObjects = new Dictionary<Vector3Int, GameObject>();

            foreach(TileData tileData in _tileData.tileDatas)
            {
                for(int i = 0; i < tileData.tiles.Length; i++)
                {
                    TileBase tile = tileData.tiles[i];
                    _tileStrengthDict[tile] = tileData.strength[i];
                }
            }

        }

        private void DisplayStrengthOnTiles()
        {
            foreach(Vector3Int pos in _tilemap.cellBounds.allPositionsWithin)
            {
                if(!_tilemap.HasTile(pos))
                    continue;

                TileBase tile = _tilemap.GetTile(pos);
                int strength = GetTileStrength(tile);

                _tileCurrentStrengthDict[pos] = strength;

                GameObject textObj = Instantiate(_textPrefab, _tilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, _tilemap.transform);
                TextMesh textMesh = textObj.GetComponent<TextMesh>();

                if(textMesh != null)
                {
                    textMesh.text = strength.ToString();

                    if(strength <= 0)
                    {
                        textMesh.color = new Color(0, 0, 0, 0);
                    }
                }

                _tileTextObjects[pos] = textObj;
            }
        }

        private int GetTileStrength(TileBase tile)
        {
            return _tileStrengthDict.TryGetValue(tile, out int strength) ? strength : 0;
        }


        public int GetCurrentTileStrength(Vector3Int position)
        {
            if(_tileCurrentStrengthDict.TryGetValue(position, out int currentStrength))
            {
                return currentStrength;
            }
            return 0; // Возвращаем 0, если позиция не найдена
        }


        public void SetCurrentTileStrength(Vector3Int position, int newStrength)
        {
            if(_tileCurrentStrengthDict.ContainsKey(position))
            {
                _tileCurrentStrengthDict[position] = newStrength;

                // Обновляем текстовое отображение прочности
                UpdateStrengthDisplay(position, newStrength);
            }
        }

        private void UpdateStrengthDisplay(Vector3Int position, int newStrength)
        {
            if(_tileTextObjects.TryGetValue(position, out GameObject textObj))
            {
                TextMesh textMesh = textObj.GetComponent<TextMesh>();
                if(textMesh != null)
                {
                    textMesh.text = newStrength.ToString();
                    if(newStrength <= 0)
                    {
                        textMesh.color = new Color(0, 0, 0, 0);
                    }
                    else
                    {
                        textMesh.color = Color.white;
                    }
                }
            }
        }


        public void UpdateTileStrengthColor(Vector3 playerPosition, float radius, Color suitableColor, Color unsuitableColor, PlayerForm playerForm)
        {
            Vector3Int playerCell = _tilemap.WorldToCell(playerPosition);

            foreach(Vector3Int pos in _tilemap.cellBounds.allPositionsWithin)
            {
                if(_tileTextObjects.ContainsKey(pos))
                {
                    float distance = Vector3.Distance(_tilemap.CellToWorld(pos), playerCell);
                    if(distance <= radius)
                    {
                        TextMesh textMesh = _tileTextObjects[pos].GetComponent<TextMesh>();
                        if(textMesh != null)
                        {
                            int strength = int.Parse(textMesh.text);
                            if(strength > 0)
                            {
                                TileBase tile = _tilemap.GetTile(pos);
                                if(tile != null)
                                {
                                    if(IsSuitableTile(tile, playerForm.SuitableResources))
                                    {
                                        textMesh.color = suitableColor;
                                    }
                                    else if(IsSuitableTile(tile, playerForm.UnsuitableResources))
                                    {
                                        textMesh.color = unsuitableColor;
                                    }
                                    else
                                    {
                                        textMesh.color = Color.white;
                                    }
                                }
                            }
                            else
                            {
                                textMesh.color = new Color(0, 0, 0, 0);
                            }
                        }
                    }
                    else
                    {
                        TextMesh textMesh = _tileTextObjects[pos].GetComponent<TextMesh>();
                        if(textMesh != null)
                        {
                            textMesh.color = new Color(0, 0, 0, 0);
                        }
                    }
                }
            }
        }

        private bool IsSuitableTile(TileBase tile, TileType[] resourceTypes)
        {
            foreach(var resource in resourceTypes)
            {
                if(_tileData.GetTileType(tile.name) == resource)
                {
                    return true;
                }
            }
            return false;
        }


        // Метод для уменьшения прочности тайла
        public void ReduceTileStrength(Vector3Int tilePos)
        {
            if(_tileTextObjects.ContainsKey(tilePos))
            {
                TextMesh textMesh = _tileTextObjects[tilePos].GetComponent<TextMesh>();
                if(textMesh != null && textMesh.color == Color.green)
                {
                    _playerController.GetDamage(1);
                    int currentStrength = int.Parse(textMesh.text);
                    currentStrength--;

                    if(currentStrength <= 0)
                    {
                        // Удаляем тайл или заменяем его поврежденным тайлом
                        _tilemap.SetTile(tilePos, null);
                        textMesh.text = "0";
                        textMesh.color = new Color(0, 0, 0, 0); // Скрываем текст
                    }
                    else
                    {
                        textMesh.text = currentStrength.ToString();
                    }
                }
            }
        }


    }

}