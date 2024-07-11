using UnityEngine;

namespace Digger
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayer;

        [SerializeField] private BoxCollider2D _boxCollider;

        [SerializeField] private TilemapStrengthDisplay _tilemapStrengthDisplay;
        [SerializeField] private int _detectRadius = 10;
        [SerializeField] private UIController _controller;

        private Rigidbody2D _rigidbody2D;
        private bool _isGrounded;
        private PlayerForm _form;

        private void OnValidate()
        {
            _boxCollider.size = _spriteRenderer.sprite.bounds.size;
            _groundCheck.localPosition = _spriteRenderer.sprite.bounds.size;

            Vector2 colliderBottom = (Vector2)transform.position + _boxCollider.offset - new Vector2(0, _boxCollider.size.y / 2);
            _groundCheck.transform.position = new Vector3(colliderBottom.x, colliderBottom.y, _groundCheck.transform.position.z);

        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }


        public void Move(float moveInput)
        {
            Vector2 moveVelocity = new(moveInput * _moveSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = moveVelocity;
            _tilemapStrengthDisplay.UpdateTileStrengthColor(transform.position, _detectRadius, Color.green, Color.red, _form);
        }

        public void Jump()
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.1f, _groundLayer);

            if(_isGrounded)
            {
                _rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            }
        }


        public void ChangePlayerForm(PlayerForm form)
        {
            _form = form;
            var sprite = form.Sprite;
            _spriteRenderer.sprite = sprite;

            _boxCollider.size = _spriteRenderer.sprite.bounds.size;

            // Вычисляем нижнюю границу BoxCollider2D
            Vector2 colliderBottom = (Vector2)transform.position + _boxCollider.offset - new Vector2(0, _boxCollider.size.y / 2);

            // Устанавливаем позицию целевого объекта на нижнюю границу BoxCollider2D
            _groundCheck.transform.position = new Vector3(colliderBottom.x, colliderBottom.y, _groundCheck.transform.position.z);
        }


        public void GetDamage(int val)
        {
            _form.GetDamage(val);
            _controller.SetStrenghtValue(_form.Index, _form.Strength);
        }

    }
}
