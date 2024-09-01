using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private Text _timerText; // Ссылка на текстовый компонент UI
    private float _elapsedTime;              // Прошедшее время в секундах
    private bool _isRunning;                 // Флаг состояния таймера


    private void Update()
    {
        if(_isRunning)
        {
            _elapsedTime += Time.deltaTime; // Увеличиваем время на прошедшее время с предыдущего кадра
            UpdateTimerText(); // Обновляем текст отображения таймера
        }
    }

    public void StartTimer()
    {
        _isRunning = true;  // Устанавливаем флаг работы таймера
        _elapsedTime = 0f;  // Сбрасываем прошедшее время
        gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        _isRunning = false; // Останавливаем таймер
        gameObject.SetActive(false);
    }

    private void UpdateTimerText()
    {
        int minutes = (int)(_elapsedTime / 60);   // Вычисляем минуты
        int seconds = (int)(_elapsedTime % 60);   // Вычисляем секунды

        _timerText.text = $"{minutes:D2}:{seconds:D2}"; // Обновляем текстовый компонент
    }

    internal int GetTime()
    {
        return (int)_elapsedTime;
    }
}
