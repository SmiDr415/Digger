using System;
using UnityEngine;

namespace Digger
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _player; // Цель, за которой следит камера
        [SerializeField] private float _smoothSpeed = 0.125f; // Скорость плавного следования камеры

        private Vector3 _offset; // Отступ между камерой и игроком
        private Vector3 _targetPosition;

        private void Start()
        {
            _offset = transform.position - _player.position; // Вычисляем начальный отступ
        }

        private void FixedUpdate()
        {
            if(_player != null)
            {
                //_targetPosition = _player.position + _offset; // Желаемая позиция камеры

                // Используем SmoothDamp для плавного следования камеры
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