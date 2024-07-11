using UnityEngine;
using UnityEngine.Tilemaps;

namespace Digger
{
    public class TilemapHighlight : MonoBehaviour
    {
        [SerializeField]
        private Tilemap _tilemap; // Ссылка на Tilemap

        [SerializeField]
        private TilemapStrengthDisplay _strengthDisplay;


        private Vector3Int _previousHighlightPos;
        private GameObject _previousHighlightObject;

        private void Update()
        {
            HighlightTileUnderCursor();

            if(Input.GetMouseButtonDown(0)) // Проверка нажатия левой кнопки мыши
            {
                HandleTileClick();
            }
        }

        private void HighlightTileUnderCursor()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = _tilemap.WorldToCell(mouseWorldPos);

            if(_tilemap.HasTile(tilePos))
            {
                // Убираем выделение с предыдущего тайла
                if(_previousHighlightObject != null && _previousHighlightPos != tilePos)
                {
                    _previousHighlightObject.GetComponent<SpriteRenderer>().enabled = false;
                    Debug.Log($"Unhighlighted tile at {_previousHighlightPos}");
                }

                // Выделяем текущий тайл
                GameObject highlightObject = _tilemap.GetInstantiatedObject(tilePos);
                if(highlightObject != null)
                {
                    highlightObject.GetComponent<SpriteRenderer>().enabled = true;
                    Debug.Log($"Highlighted tile at {tilePos}");
                }

                _previousHighlightPos = tilePos;
                _previousHighlightObject = highlightObject;
            }
            else if(_previousHighlightObject != null)
            {
                // Убираем выделение с предыдущего тайла
                _previousHighlightObject.GetComponent<SpriteRenderer>().enabled = false;
                Debug.Log($"Unhighlighted tile at {_previousHighlightPos}");
                _previousHighlightObject = null;
            }
        }


        private void HandleTileClick()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = _tilemap.WorldToCell(mouseWorldPos);

            if(_tilemap.HasTile(tilePos))
            {
                _strengthDisplay.ReduceTileStrength(tilePos);
            }
        }

    }
}
