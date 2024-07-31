using System;
using UnityEngine;

namespace MultiTool
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _player; // Цель, за которой следит камера
        [SerializeField] private float _smoothSpeed = 0.125f; // Скорость плавного следования камеры

        private Vector3 _offset; // Отступ между камерой и игроком
        private Vector3 _targetPosition;

        private void Start()
        {
            transform.position = _player.position - Vector3.forward;
            _offset = transform.position - _player.position; // Вычисляем начальный отступ
        }

        private void FixedUpdate()
        {
            if(_player != null)
            {
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, _targetPosition, _smoothSpeed);
                transform.position = smoothedPosition;
            }
        }

        internal void SetTargetPosition(Vector3 target)
        {
            _targetPosition = target + _offset;

        }
    }
}