using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    internal class ResultLevelUI : MonoBehaviour
    {
        [SerializeField] private Text _statusInfo;
        [SerializeField] private Text _level;
        [SerializeField] private Text _time;
        [SerializeField] private GameObject _newRecordIcon;
        [SerializeField] private GameObject[] _stars;
        [SerializeField] private Button _nextLevelButton;

        internal void ShowResult(Result result)
        {
            _statusInfo.text = result.winStatus ? "Победа" : "Провал";
            _level.text = $"Уровень: {result.level}";

            int time = result.time;
            int targetTime2Minutes = time / 60;
            int targetTime2Seconds = time % 60;

            _time.text = $"Время уровня: {targetTime2Minutes:D2}:{targetTime2Seconds:D2}";
            _newRecordIcon.SetActive(result.newBestTime);

            for(int i = 0; i < _stars.Length; i++)
            {
                _stars[i].SetActive(i < result.stars);
            }
            gameObject.SetActive(true);

            _nextLevelButton.interactable = result.level < 3;
        }
    }
}