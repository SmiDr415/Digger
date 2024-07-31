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
            if(_playerController == null)
                _playerController = FindAnyObjectByType<PlayerController>();
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
            }
        }

        private bool IsReady()
        {
            return _playerController != null && !GUIWindowManager.Instance.IsActive && !_playerController.PlayerTeleportation.IsTeleporting;
        }

        private void FixedUpdate()
        {
            if(!GUIWindowManager.Instance.IsActive)
                _cameraController.SetTargetPosition(CheckCursorPosition(_playerController.transform.position, _camera));
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

            Vector3 offset;

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
                return playerPosition;
            }

            // Convert screen offset to world space
            Vector3 screenOffset = cursorPos + offset;
            Vector3 worldOffset = camera.ScreenToWorldPoint(new Vector3(screenOffset.x, screenOffset.y, camera.nearClipPlane)) - camera.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, camera.nearClipPlane));

            return playerPosition + worldOffset;
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
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(GameManager.Instance.FormController.CurrentForm.Index != (int)FormType.Form_Sickle)
                {
                    PlayerController.Instance.PlayerShapeshift.SwitchForm(FormType.Form_Sickle);
                }
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                if(GameManager.Instance.FormController.CurrentForm.Index != (int)FormType.Form_Pickaxe)
                {
                    PlayerController.Instance.PlayerShapeshift.SwitchForm(FormType.Form_Pickaxe);
                }
            }
        }
    }
}
