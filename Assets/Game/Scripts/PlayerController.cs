using UnityEngine;

namespace Digger
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private BoxCollider2D _boxCollider;
        [SerializeField] private TilemapStrengthDisplay _tilemapStrengthDisplay;
        [SerializeField] private int _detectRadius = 10;

        private Rigidbody2D _rigidbody2D;
        private bool _isGrounded;
        private PlayerForm _form;
        private InteractiveSprite _currentInteractive;

        public InteractiveSprite InteractiveSprite => _currentInteractive;

        private void OnValidate()
        {
            UpdateColliderSize();
        }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();

            GameEventManager.Instance.Subscribe(GameEvent.OnChangeForm, ChangePlayerForm);
        }

        private void UpdateColliderSize()
        {
            _boxCollider.size = _spriteRenderer.sprite.bounds.size;
            _groundCheck.localPosition = _spriteRenderer.sprite.bounds.size;

            Vector2 colliderBottom = (Vector2)transform.position + _boxCollider.offset - new Vector2(0, _boxCollider.size.y / 2);
            _groundCheck.transform.position = new Vector3(colliderBottom.x, colliderBottom.y, _groundCheck.transform.position.z);
        }

        public void Move(float moveInput)
        {
            if(GUIWindowManager.Instance.IsActive)
                return;

            Vector2 moveVelocity = new(moveInput * _moveSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = moveVelocity;
            _tilemapStrengthDisplay.UpdateTileStrengthColor(transform.position, _detectRadius, Color.green, Color.red, _form);
        }

        public void Jump()
        {
            if(GUIWindowManager.Instance.IsActive)
                return;

            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.1f, _groundLayer);

            if(_isGrounded)
            {
                _rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            }
        }

        public void ChangePlayerForm()
        {
            _form = GameManager.Instance.FormController.CurrentForm;
            var sprite = _form.Sprite;
            _spriteRenderer.sprite = sprite;

            UpdateColliderSize();
        }

        public void GetDamage(int val)
        {
            _form.GetDamage(val);
            UIController.Instance.SetStrenghtValue(_form.Index, _form.Strength);
        }

        internal void ReadyInteractible()
        {
            if(_currentInteractive)
            {
                GUIWindowManager.Instance.ShowWindowAbovePosition(_currentInteractive.GetTopColliderPosition());
            }
        }

        internal void ActivateReadyInteractible(InteractiveSprite interactiveSprite)
        {
            _currentInteractive = interactiveSprite;
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.Unsubscribe(GameEvent.OnChangeForm, ChangePlayerForm);
        }

        internal void HideInteractibleWindow()
        {
            _currentInteractive = null;
        }
    }
}
