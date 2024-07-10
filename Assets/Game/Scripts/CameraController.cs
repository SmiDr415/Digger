using UnityEngine;

namespace Digger
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]private Transform _player; // Цель, за которой следит камера
        [SerializeField]private float _smoothSpeed = 0.125f; // Скорость плавного следования камеры

        private Vector3 offset; // Отступ между камерой и игроком

        private void Start()
        {
            offset = transform.position - _player.position; // Вычисляем начальный отступ
        }

        private void FixedUpdate()
        {
            if(_player != null)
            {
                Vector3 desiredPosition = _player.position + offset; // Желаемая позиция камеры

                // Используем SmoothDamp для плавного следования камеры
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
                transform.position = smoothedPosition;
            }
        }
    }
}
