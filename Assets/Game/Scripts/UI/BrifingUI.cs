using System;
using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class BrifingUI : MonoBehaviour
    {
        [SerializeField] private Text _level;
        [SerializeField] private Text _bestTime;
        [SerializeField] private Text _targetTime2;
        [SerializeField] private Text _targetTime1;

        [SerializeField] private Button _start;

        public void Init(LevelData levelData)
        {
            _level.text = $"Уровень {levelData.level}";

            // Преобразуем время в секундах в минуты и секунды
            int targetTime1Minutes = levelData.timeTreeStars / 60;
            int targetTime1Seconds = levelData.timeTreeStars % 60;

            _targetTime1.text = $"{targetTime1Minutes:D2}:{targetTime1Seconds:D2}";

            // Преобразуем целевое время для двух звезд
            int targetTime2 = levelData.timeTwoStars;
            int targetTime2Minutes = targetTime2 / 60;
            int targetTime2Seconds = targetTime2 % 60;

            _targetTime2.text = $"{targetTime2Minutes:D2}:{targetTime2Seconds:D2}";

            int bestTimeInSeconds = PlayerPrefs.GetInt($"Level{levelData.level}", int.MaxValue);

            if(bestTimeInSeconds != int.MaxValue)
            {
                int bestTimeMinutes = bestTimeInSeconds / 60;
                int bestTimeSeconds = bestTimeInSeconds % 60;
                _bestTime.text = $"Рекордное время: {bestTimeMinutes:D2}:{bestTimeSeconds:D2}";
            }
            else
            {
                _bestTime.text = string.Empty;
            }

            gameObject.SetActive(true);
        }

    }

}