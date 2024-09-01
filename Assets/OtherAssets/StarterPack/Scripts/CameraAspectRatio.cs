using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectRatio : MonoBehaviour
{
    [SerializeField] private float _targetAspect = 16.0f / 9.0f; // Целевое соотношение сторон
    private Camera _mainCamera;
    private float _lastAspectRatio;

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
        AdjustCamera(); // Первоначальная настройка камеры
    }

    private void Update()
    {
        // Проверяем, изменилось ли соотношение сторон
        float currentAspect = (float)Screen.width / Screen.height;
        if(Mathf.Abs(currentAspect - _lastAspectRatio) > 0.01f)
        {
            _lastAspectRatio = currentAspect;
            AdjustCamera(); // Корректируем камеру при изменении соотношения сторон
        }
    }

    private void AdjustCamera()
    {
        // Получаем текущее соотношение сторон экрана
        float windowAspect = (float)Screen.width / Screen.height;
        // Рассчитываем масштаб, необходимый для достижения целевого соотношения сторон
        float scaleHeight = windowAspect / _targetAspect;

        if(scaleHeight < 1.0f) // Если экран шире, чем нужно
        {
            // Добавляем фон сверху и снизу (letterboxing)
            Rect rect = _mainCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            _mainCamera.rect = rect;
        }
        else // Если экран уже, чем нужно
        {
            // Добавляем фон слева и справа (pillarboxing)
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = _mainCamera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            _mainCamera.rect = rect;
        }
    }
}
