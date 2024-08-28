using UnityEngine;

namespace MultiTool
{
    public class PlayerMovementController : IMoveable
    {
        private Rigidbody2D _rigidbody2D;
        private float _moveSpeed;
        private float _jumpVelocity;

        public PlayerMovementController(Rigidbody2D rigidbody2D, float moveSpeed, float jumpVelocity)
        {
            _rigidbody2D = rigidbody2D;
            _moveSpeed = moveSpeed;
            _jumpVelocity = jumpVelocity;
        }

        public void Move(float moveInput)
        {
            var moveVelocity = new Vector2(moveInput * _moveSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = moveVelocity;
        }

        public void Jump()
        {
            // Логика прыжка
        }
    } 
}