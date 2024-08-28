using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace MultiTool
{
    public class TilemapStrengthDisplay : MonoBehaviour
    {
        [SerializeField]
        private Tilemap _tilemap;

        [SerializeField]
        private GameObject _textPrefab;

        [SerializeField]
        private GameObject _dropPrefab;

        [SerializeField]
        private TilesData _tileData;

        [SerializeField]
        private DropItemDatabase _dropItemDatabase;

        [SerializeField]
        private PlayerController _playerController;


        [SerializeField]
        private BlockHitController _blockHitController;

        private Dictionary<TileBase, int> _tileStrengthDict;
        private Dictionary<Vector3Int, int> _tileCurrentStrengthDict;
        private Dictionary<Vector3Int, TextMesh> _tileTextObjects;

        private Color _disableColor = new(0, 0, 0, 0);


        public void InitializeTileStrengthDict()
        {
            _tileStrengthDict = new Dictionary<TileBase, int>();
            _tileCurrentStrengthDict = new Dictionary<Vector3Int, int>();
            _tileTextObjects = new Dictionary<Vector3Int, TextMesh>();

            foreach(TileData tileData in _tileData.TileDatas)
            {
                for(int i = 0; i < tileData.Tiles.Length; i++)
                {
                    TileBase tile = tileData.Tiles[i];
                    if(tileData.Durability.Length > 0)
                        _tileStrengthDict[tile] = tileData.Durability[i];
                }
            }

            DisplayStrengthOnTiles();

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

                GameObject textObj = _tilemap.GetInstantiatedObject(pos);
                if(textObj == null)
                    continue;
                TextMesh textMesh = textObj.GetComponentInChildren<TextMesh>();
                if(textMesh != null)
                {
                    _tileTextObjects[pos] = textObj.GetComponentInChildren<TextMesh>();
                    textMesh.text = strength.ToString();
                    if(strength <= 0)
                    {
                        textMesh.color = _disableColor;
                    }
                }

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



        public void UpdateTileStrengthColor(Vector3 playerPosition, float radius, PlayerForm playerForm)
        {
            Vector3Int playerCell = _tilemap.WorldToCell(playerPosition);

            foreach(Vector3Int pos in _tilemap.cellBounds.allPositionsWithin)
            {
                if(_tileTextObjects != null && _tileTextObjects.ContainsKey(pos))
                {
                    float distance = Vector3.Distance(_tilemap.CellToWorld(pos), playerCell);
                    if(distance <= radius && PlayerController.Instance.IsReady)
                    {
                        if(_tileTextObjects[pos].TryGetComponent<TextMesh>(out var textMesh))
                        {
                            int strength = int.Parse(textMesh.text);
                            if(strength > 0)
                            {
                                TileBase tile = _tilemap.GetTile(pos);
                                TileBase tileUp = _tilemap.GetTile(pos + Vector3Int.up);


                                if(tile != null)
                                {

                                    var tileType = _tileData.GetTileType(tile.name);
                                    var typeHarvestable = playerForm.GetHarvestType(tileType);
                                    if(tileUp != null && _tilemap.GetColliderType(pos + Vector3Int.up) == Tile.ColliderType.None)
                                        typeHarvestable = HarvestType.Unharvestable;

                                    switch(typeHarvestable)
                                    {
                                        case HarvestType.Perfect:
                                            textMesh.color = Color.green;
                                            break;
                                        case HarvestType.Harvestable:
                                            textMesh.color = Color.yellow;
                                            break;
                                        case HarvestType.Unharvestable:
                                            textMesh.color = Color.red;
                                            break;
                                        default:
                                            textMesh.color = Color.white;
                                            break;
                                    }

                                }
                            }
                            else
                            {
                                textMesh.color = _disableColor;
                            }
                        }
                    }
                    else
                    {
                        if(_tileTextObjects[pos].TryGetComponent<TextMesh>(out var textMesh))
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


        public void ReduceTileStrength(Vector3Int tilePos)
        {
            if(_tileTextObjects.ContainsKey(tilePos))
            {
                TextMesh textMesh = _tileTextObjects[tilePos];
                if(textMesh != null && textMesh.color != Color.red && textMesh.color != _disableColor && _playerController.IsReady && _playerController.Form.Strength > 0)
                {
                    _playerController.GetDamage(1);
                    int currentStrength = int.Parse(textMesh.text);
                    currentStrength -= _playerController.Form.Damage;

                    if(currentStrength <= 0)
                    {
                        var tile = _tilemap.GetTile(tilePos);
                        var dropName = _dropItemDatabase.GetNameByTileName(tile.name);

                        if(dropName != null)
                        {
                            Vector3 worldPos = _tilemap.CellToWorld(tilePos);
                            var dropGO = Instantiate(_dropPrefab, worldPos, Quaternion.identity);
                            dropGO.name = dropName;
                            dropGO.GetComponent<GroundItem>().Init();
                            _blockHitController.TileDestroy(true, tilePos);

                        }
                        _tilemap.SetTile(tilePos, null);
                        _tileTextObjects.Remove(tilePos);
                    }
                    else
                    {
                        var tile = _tilemap.GetTile(tilePos);
                        var newTile = _tileData.GetTileByStrength(tile.name, currentStrength);
                        _tilemap.SetTile(tilePos, newTile);
                        GameObject textObj = _tilemap.GetInstantiatedObject(tilePos);
                        TextMesh newtextMesh = textObj.GetComponentInChildren<TextMesh>();
                        newtextMesh.text = currentStrength.ToString();
                        _tileTextObjects[tilePos] = newtextMesh;
                        _blockHitController.TileDestroy(false, tilePos);

                    }
                }
            }
        }


    }

}