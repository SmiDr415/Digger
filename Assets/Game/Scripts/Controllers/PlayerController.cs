using System.Collections;
using UnityEngine;

namespace MultiTool
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(SpriteRenderer))]
    [ExecuteInEditMode]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _gravityCurve;
        [SerializeField] private AnimationCurve _jumpVelocityCurve;

        public static PlayerController Instance { get; private set; }

        [Header("Player Settings")]

        [Header("Радиус сбора ресурсов")]
        [Tooltip("Радиус, в котором все предметы притягиваются к герою")]
        [SerializeField]
        [Range(0, 5)]
        private int _gatherRadius = 0;

        [Header("Радиус разрушения ресурсов")]
        [Tooltip("Радиус, в котором игрок может выбрать блок для разрушения")]
        [SerializeField]
        [Range(1, 5)]
        private int _breakRadius = 3;

        [Header("Радиус взаимодействия")]
        [Tooltip("Радиус, в котором игрок может взаимодействовать с интерактивными объектами")]
        [SerializeField]
        [Range(0, 5)]
        private int _interactionRadius = 0;

        [Header("Текущий баланс денег")]
        [Tooltip("Количество денег на балансе героя")]
        [SerializeField]
        private int _moneyAmount = 0;

        [Header("Скорость движения по горизонтали")]
        [Tooltip("Скорость движения героя по горизонтали")]
        [SerializeField]
        [Range(1, 10)]
        private float _moveSpeed = 1.0f;

        [Header("Максимальная высота прыжка")]
        [Tooltip("Максимальная высота, на которую может прыгнуть герой")]
        [SerializeField]
        [Range(1, 5)]
        private int _maxJumpHeight = 1;

        [Header("Время достижения максимальной высоты прыжка")]
        [Tooltip("Время, за которое герой достигает максимальной высоты прыжка")]
        [SerializeField]
        private float _timeToJumpApex = 0.4f;

        [Space]

        private PlayerAnimation _playerAnimation;
        private PlayerShapeshift _playerShapeshift;
        private PlayerTeleportation _playerTeleportation;

        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private TilemapStrengthDisplay _tilemapStrengthDisplay;
        [SerializeField] private BoxCollider2D _interactionTrigger;
        [SerializeField] private BoxCollider2D _gatherTrigger;

        private BoxCollider2D _playerBoxCollider;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private bool _isGrounded;
        private PlayerForm _currentForm;
        private InteractiveObject _currentInteractive;

        private float _gravityScale;
        private float _jumpVelocity;
        private float _lastMineTime;

        public bool IsReady => !_playerTeleportation.IsTeleporting && !_playerShapeshift.IsShapeshifting && IsCooldown();

        public PlayerShapeshift PlayerShapeshift => _playerShapeshift;
        public PlayerTeleportation PlayerTeleportation => _playerTeleportation;
        public InteractiveObject InteractiveSprite => _currentInteractive;
        public int GatherRadius => _gatherRadius;
        public PlayerForm Form => _currentForm;


        #region Initialization
        private void Awake()
        {
            InitializeComponents();
            SetupSingleton();
            SubscribeToEvents();
            UpdateColliderSize();
            CalculateJumpForce(); // Вычислить силу прыжка при инициализации
        }

        private void Start()
        {
            if(GameManager.Instance.FormController != null)
                _currentForm = GameManager.Instance.FormController.CurrentForm;
            UpdateTileStrengthDisplay();
            _lastMineTime = Time.time;

        }

        private void OnValidate()
        {
            UpdateColliderSize();
            _interactionTrigger.edgeRadius = _interactionRadius;
            _gatherTrigger.edgeRadius = _gatherRadius;
            _moneyAmount = Mathf.Clamp(_moneyAmount, 0, int.MaxValue);
            CalculateJumpForce(); // Вычислить силу прыжка при изменении значений в инспекторе
        }

        private void InitializeComponents()
        {
            _playerAnimation = GetComponent<PlayerAnimation>();
            _playerShapeshift = GetComponent<PlayerShapeshift>();
            _playerTeleportation = GetComponent<PlayerTeleportation>();

            _playerBoxCollider = GetComponent<BoxCollider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

        }

        private static void SetupSingleton()
        {
            if(Instance == null)
            {
                Instance = FindFirstObjectByType<PlayerController>();
                if(Application.isPlaying)
                {
                    DontDestroyOnLoad(Instance.gameObject);
                }
            }
            else
            {
                Debug.LogWarning($"{nameof(PlayerController)}: Singleton  trying to initialize! Destroying duplicate object...", Instance.gameObject);
                Destroy(Instance.gameObject);
            }
        }

        public void SubscribeToEvents()
        {
            GameEventManager.Instance.Subscribe(GameEvent.OnChangeForm, OnChangeForm);
        }
        #endregion


        private void FixedUpdate()
        {
            //if(_rigidbody2D.velocity != Vector2.zero)
            UpdateTileStrengthDisplay();
            _gravityScale *= _gravityCurve.Evaluate(Time.time);
        }

        #region Movement

        public void Move(float moveInput)
        {
            var moveVelocity = new Vector2(moveInput * _moveSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = moveVelocity;
            if(moveVelocity.x != 0)
            {
                _spriteRenderer.flipX = moveVelocity.x > 0;
            }

            _playerAnimation.SetJump(Mathf.Abs(moveVelocity.y) > 1f);
            _playerAnimation.SetWalk(Mathf.Abs(moveVelocity.x) > 1f);
        }

        public void Jump()
        {
            CheckIfGrounded();

            if(_isGrounded)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity) * _jumpVelocityCurve.Evaluate(Time.time);
            }
        }

        private void CheckIfGrounded()
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.1f, _groundLayer);
        }

        private void UpdateTileStrengthDisplay()
        {
            if(_tilemapStrengthDisplay == null)
            {
                return;
            }

            if(_currentForm != null)
                _tilemapStrengthDisplay.UpdateTileStrengthColor(transform.position, _breakRadius, _currentForm);
        }

        #endregion


        #region Form Management

        private void OnChangeForm()
        {
            _currentForm = GameManager.Instance.FormController.CurrentForm;
            UpdateColliderSize();
            UpdatePlayerSprite();
        }

        public bool IsCooldown()
        {
            return Time.time - _lastMineTime > _currentForm.Cooldown;
        }

        private void UpdateColliderSize()
        {
            if(_playerBoxCollider == null)
            {
                _playerBoxCollider = GetComponent<BoxCollider2D>();
            }

            if(_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            _playerBoxCollider.size = _spriteRenderer.sprite.bounds.size;
            _groundCheck.localPosition = _spriteRenderer.sprite.bounds.size;

            UpdateGroundPosition();
        }

        private void UpdatePlayerSprite()
        {
            if(_spriteRenderer != null)
            {
                _spriteRenderer.sprite = _currentForm?.Sprite;
            }
        }

        private void UpdateGroundPosition()
        {
            Vector3 colliderBottom = GetGroundCheckPoint();
            _groundCheck.position = colliderBottom;
        }

        private Vector3 GetGroundCheckPoint()
        {
            Vector2 offset = _playerBoxCollider.offset;
            Vector2 size = _playerBoxCollider.size;

            offset.y -= size.y / 2;

            return transform.position + (Vector3)offset;
        }
        #endregion

        #region Interaction
        public void GetDamage(int val)
        {
            if(_currentForm == null)
            {
                return;
            }

            _playerAnimation.Mine();
            _currentForm.GetDamage(val);
            _lastMineTime = Time.time;
            UIController.Instance.SetStrengthValue(_currentForm.Index, _currentForm.Strength);
        }

        internal void ReadyInteractible()
        {
            TryShowInteractiveWindow(_currentInteractive);
        }

        private void TryShowInteractiveWindow(InteractiveObject interactiveSprite)
        {
            if(interactiveSprite == null)
            {
                return;
            }

            _rigidbody2D.velocity = Vector3.zero;
            GUIWindowManager.Instance.ShowWindowAbovePosition(interactiveSprite.GetTopColliderPosition());
        }

        internal void ActivateReadyInteractible(InteractiveObject interactiveSprite)
        {
            _currentInteractive = interactiveSprite;
        }

        internal void HideInteractableWindow()
        {
            _currentInteractive = null;
        }
        #endregion

        #region Cleanup
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        public void UnsubscribeFromEvents()
        {
            GameEventManager.Instance.Unsubscribe(GameEvent.OnChangeForm, OnChangeForm);
        }
        #endregion

        #region Jump Calculation
        private void CalculateJumpForce()
        {
            if(_rigidbody2D == null)
                _rigidbody2D = GetComponent<Rigidbody2D>();

            _gravityScale = (2 * _maxJumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
            _jumpVelocity = Mathf.Sqrt(2 * _gravityScale * _maxJumpHeight);

            _rigidbody2D.gravityScale = _gravityScale / Mathf.Abs(Physics2D.gravity.y);
        }
        #endregion
    }
}
