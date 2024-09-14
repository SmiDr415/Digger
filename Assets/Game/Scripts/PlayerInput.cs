using System;
using System.Collections.Generic;
using UnityEngine;

namespace MultiTool
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float _offsetBorder = 0.05f;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private float _topOffsetMultiply = 5;
        [SerializeField] private float _bottomOffsetMultiply = 5;
        [SerializeField] private float _leftRightOffsetMultiply = 3;

        private Camera _camera;

        private Vector3 _lastMousePosition;
        private bool _isDragging;

        public static event Action<string> OnCursorEdgeReached;
        public static event Action<string> OnPlayerMove;

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
            _playerController ??= FindFirstObjectByType<PlayerController>();
            _cameraController ??= FindFirstObjectByType<CameraController>();
        }

        private void Update()
        {
            if(IsReady())
            {
                HandleMovementInput();
                HandleJumpInput();
                HandleInteractibleInput();
                HandleTeleportInput();
                HandleCancelInput();
                HandleFormSwitchInput();

                HandleMouseInput();
            }
        }

        private bool IsReady()
        {
            return _playerController.gameObject.activeInHierarchy &&
                   !GUIWindowManager.Instance.IsActive &&
                   !_playerController.PlayerTeleportation.IsTeleporting;
        }

        private void FixedUpdate()
        {
            if(!GUIWindowManager.Instance.IsActive)
            {
                Vector3 targetPosition = CheckCursorPosition(_playerController.transform.position, _camera);
                _cameraController.SetTargetPosition(targetPosition);
            }
        }

        private Vector3 CheckCursorPosition(Vector3 playerPosition, Camera camera)
        {
            Vector3 cursorPos = Input.mousePosition;
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float edgeThresholdWidth = screenWidth * _offsetBorder;
            float edgeThresholdHeight = screenHeight * _offsetBorder;

            bool isLeft = cursorPos.x <= edgeThresholdWidth;
            bool isRight = cursorPos.x >= screenWidth - edgeThresholdWidth;
            bool isTop = cursorPos.y >= screenHeight - edgeThresholdHeight;
            bool isBottom = cursorPos.y <= edgeThresholdHeight;

            Vector3 offset = Vector3.zero;

            if(isLeft)
            {
                OnCursorEdgeReached?.Invoke("Посмотри слева");
                offset.x = -screenWidth / _leftRightOffsetMultiply;
            }
            else if(isRight)
            {
                OnCursorEdgeReached?.Invoke("Посмотра справа");
                offset.x = screenWidth / _leftRightOffsetMultiply;
            }

            if(isTop)
            {
                OnCursorEdgeReached?.Invoke("Посмотри вверх");
                offset.y = screenHeight / _topOffsetMultiply;
            }
            else if(isBottom)
            {
                OnCursorEdgeReached?.Invoke("Посмотри вниз");
                offset.y = -screenHeight / _bottomOffsetMultiply;
            }

            Vector3 screenOffset = cursorPos + offset;
            Vector3 worldOffset = camera.ScreenToWorldPoint(new Vector3(screenOffset.x, screenOffset.y, camera.nearClipPlane)) - camera.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, camera.nearClipPlane));

            return playerPosition + worldOffset;
        }

        private void HandleMovementInput()
        {
            float moveInput = Input.GetAxis("Horizontal");
            if(moveInput >= 1)
            {
                OnPlayerMove?.Invoke("Пройдись вправо");
            }
            else if(moveInput <= -1)
            {
                OnPlayerMove?.Invoke("Пройдись влево");
            }

            _playerController.Move(moveInput);
        }

        private void HandleMouseInput()
        {
            if(Input.GetMouseButtonDown(0))
            {
                _lastMousePosition = Input.mousePosition;
                _isDragging = true;
            }

            if(_isDragging && Input.GetMouseButton(0))
            {
                Vector3 currentMousePosition = Input.mousePosition;
                Vector3 mouseDelta = currentMousePosition - _lastMousePosition - Vector3.up * Screen.height / 10;
                float moveInput = Mathf.Clamp(mouseDelta.x, -1f, 1f);
                float jumpInput = Mathf.Clamp(mouseDelta.y, 0f, 1f);

                if(moveInput >= 1f)
                {
                    OnPlayerMove?.Invoke("Пройдись вправо");
                }
                else if(moveInput <= -1f)
                {
                    OnPlayerMove?.Invoke("Пройдись влево");
                }

                if(jumpInput >= 1f)
                {
                    OnPlayerMove?.Invoke("Подпрыгни");
                    _playerController.Jump();
                    _lastMousePosition = Input.mousePosition;
                }

                if(Mathf.Abs(mouseDelta.x) > Screen.width / 20)
                    _playerController.Move(moveInput);
                //_lastMousePosition = currentMousePosition;
            }

            if(Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                _playerController.Move(0f); // Останавливаем персонажа
            }
        }


        private void HandleJumpInput()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                OnPlayerMove?.Invoke("Подпрыгни");
                _playerController.Jump();
            }
        }

        private void HandleInteractibleInput()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _playerController.ReadyInteractible();
            }
        }

        private void HandleTeleportInput()
        {
            if(Input.GetKeyDown(KeyCode.H))
            {
                _playerController.PlayerTeleportation.StartTeleport();
            }
        }

        private void HandleCancelInput()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                _playerController.PlayerTeleportation.CancelTeleport();
                _playerController.PlayerShapeshift.CancelShapeshift();
            }
        }

        private void HandleFormSwitchInput()
        {
            var formBindings = new Dictionary<KeyCode, FormType>
                {
                    { KeyCode.Alpha1, FormType.Form_Sickle },
                    { KeyCode.Alpha2, FormType.Form_Shovel },
                    { KeyCode.Alpha3, FormType.Form_Pickaxe }
                };

            foreach(var binding in formBindings)
            {
                if(Input.GetKeyDown(binding.Key))
                {
                    FormType formType = binding.Value;
                    int formIndex = (int)formType;

                    if(GameManager.Instance.FormController.CurrentForm.Index != formIndex)
                    {
                        _playerController.PlayerShapeshift.SwitchForm(formType);
                    }

                    break;
                }
            }
        }
    }
}
