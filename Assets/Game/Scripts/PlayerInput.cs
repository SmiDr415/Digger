using UnityEngine;

namespace Digger
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float _offsetBorder = 0.05f;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CameraController _cameraController;

        [SerializeField] private float _topOffsetMultiply = 10;
        [SerializeField] private float _bottomOffsetMultiply = 5;
        [SerializeField] private float _leftRightOffsetMultiply = 3;

        private Camera _camera;

        private void OnValidate()
        {
            _offsetBorder = Mathf.Clamp(_offsetBorder, 0.01f, 0.2f);
            _topOffsetMultiply = Mathf.Clamp(_topOffsetMultiply, 1, 10);
            _bottomOffsetMultiply = Mathf.Clamp(_bottomOffsetMultiply, 1, 10);
            _leftRightOffsetMultiply = Mathf.Clamp(_leftRightOffsetMultiply, 1, 10);

        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if(_playerController != null)
            {
                HandleMovementInput();
                HandleJumpInput();
            }

        }


        private void FixedUpdate()
        {
            _cameraController.SetTargetPosition(CheckCursorPosition(_playerController.transform.position, _camera));
        }

        private Vector3 CheckCursorPosition(Vector3 playerPosition, Camera camera)
        {
            var cursorPos = Input.mousePosition;
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            // Определяем границу для краёв экрана (10% от ширины и высоты экрана)
            float edgeThresholdWidth = screenWidth * _offsetBorder;
            float edgeThresholdHeight = screenHeight * _offsetBorder;

            // Определяем позицию курсора на экране
            bool isLeft = cursorPos.x <= edgeThresholdWidth;
            bool isRight = cursorPos.x >= screenWidth - edgeThresholdWidth;
            bool isTop = cursorPos.y >= screenHeight - edgeThresholdHeight;
            bool isBottom = cursorPos.y <= edgeThresholdHeight;

            Vector3 offset = Vector3.zero; // Начальный отступ

            if(isLeft && isTop)
            {
                offset = new Vector3(-screenWidth / _leftRightOffsetMultiply, screenHeight / _topOffsetMultiply, 0);
            }
            else if(isLeft && isBottom)
            {
                offset = new Vector3(-screenWidth / _leftRightOffsetMultiply, -screenHeight / _bottomOffsetMultiply, 0);
            }
            else if(isRight && isTop)
            {
                offset = new Vector3(screenWidth / _leftRightOffsetMultiply, screenHeight / _topOffsetMultiply, 0);
            }
            else if(isRight && isBottom)
            {
                offset = new Vector3(screenWidth / _leftRightOffsetMultiply, -screenHeight / _bottomOffsetMultiply, 0);
            }
            else if(isLeft)
            {
                offset = new Vector3(-screenWidth / _leftRightOffsetMultiply, 0, 0);
            }
            else if(isRight)
            {
                offset = new Vector3(screenWidth / _leftRightOffsetMultiply, 0, 0);
            }
            else if(isTop)
            {
                offset = new Vector3(0, screenHeight / _topOffsetMultiply, 0);
            }
            else if(isBottom)
            {
                offset = new Vector3(0, -screenHeight / _bottomOffsetMultiply, 0);
            }
            else
            {
                return playerPosition; // Центр экрана, возвращаем позицию игрока без изменений
            }

            // Переводим координаты отступа из экранных в мировые
            Vector3 screenOffset = cursorPos + offset;
            Vector3 worldOffset = camera.ScreenToWorldPoint(new Vector3(screenOffset.x, screenOffset.y, camera.nearClipPlane)) - camera.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, camera.nearClipPlane));

            return playerPosition + worldOffset; // Возвращаем позицию игрока с отступом
        }


        private void HandleMovementInput()
        {
            float moveInput = Input.GetAxis("Horizontal");
            _playerController.Move(moveInput);
        }

        private void HandleJumpInput()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _playerController.Jump();
            }
        }
    }

    public enum Positions
    {
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        BottomLeft,
        TopRight,
        BottomRight,
        Center
    }
}
