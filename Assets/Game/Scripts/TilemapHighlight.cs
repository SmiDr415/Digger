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
                    if(highlightObject.TryGetComponent(out SpriteRenderer spriteRenderer))
                    {
                        var tm = highlightObject.GetComponentInChildren<TextMesh>();
                        if(tm != null)
                        {
                            spriteRenderer.color = tm.color;
                            //if(tm.color == Color.green)
                            //{
                            //    spriteRenderer.color = Color.green;
                            //}
                            //else if(tm.color == Color.yellow)
                            //{
                            //    spriteRenderer.color = Color.yellow;
                            //}
                            //else
                            //{
                            //    spriteRenderer.color = Color.red;
                            //}
                        }

                        spriteRenderer.enabled = true;
                        _previousHighlightPos = tilePos;
                        _previousHighlightObject = highlightObject;
                    }
                }
                else
                {
                    _previousHighlightObject = null;
                }

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
                int flip = tilePos.x + 0.5f > _playerController.transform.position.x ? 1 : -1;
                _playerController.Move(flip);
                _strengthDisplay.ReduceTileStrength(tilePos);
            }
        }

    }
}
