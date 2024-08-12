using UnityEngine;
using UnityEngine.Tilemaps;

namespace MultiTool
{
    public class TilemapHighlight : MonoBehaviour
    {
        [SerializeField]
        private Tilemap _tilemap;

        [SerializeField]
        private TilemapStrengthDisplay _strengthDisplay;

        [SerializeField]
        private PlayerController _playerController;

        private Vector3Int _previousHighlightPos;
        private GameObject _previousHighlightObject;

        private void Update()
        {
            if(!_playerController.gameObject.activeInHierarchy)
                return;

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
                }

                // Выделяем текущий тайл
                GameObject highlightObject = _tilemap.GetInstantiatedObject(tilePos);
                if(highlightObject != null)
                {
                    highlightObject.GetComponent<SpriteRenderer>().enabled = true;
                }

                _previousHighlightPos = tilePos;
                _previousHighlightObject = highlightObject;
            }
            else if(_previousHighlightObject != null)
            {
                // Убираем выделение с предыдущего тайла
                _previousHighlightObject.GetComponent<SpriteRenderer>().enabled = false;
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
