using UnityEngine;

namespace Digger
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private Sprite[] _spritesForms;

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayer;

        private Rigidbody2D _rigidbody2D;
        private bool _isGrounded;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Move();
            Jump();
        }

        private void Move()
        {
            float moveInput = Input.GetAxis("Horizontal");
            Vector2 moveVelocity = new(moveInput * _moveSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = moveVelocity;
        }

        private void Jump()
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.1f, _groundLayer);

            if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            }
        }

        // Метод для смены спрайта игрока
        public void ChangePlayerSprite(int val)
        {
            if(_spriteRenderer != null)
            {
                _spriteRenderer.sprite = _spritesForms[val];
                Debug.Log("Player sprite changed.");
            }
            else
            {
                Debug.LogWarning("SpriteRenderer is not assigned.");
            }
        }
    }
}
