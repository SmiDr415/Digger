using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Button[] _levelButtons;
        [SerializeField] private TileMapGenerator _tileMapGenerator;
        [SerializeField] private BrifingUI _brickUI;
        [SerializeField] private ResultLevelUI _resultLevelUI;
        [SerializeField] private TextAsset[] _levelDatas;            // Массив JSON-файлов для уровней
        [SerializeField] private LevelTimer _levelTimer;
        [SerializeField] private GameObject _levelsPanel;
        private int _currentLevel;
        private int _maxLevel;
        private LevelData _currentLevelData;

        private void Start()
        {
            if(PlayerPrefs.HasKey("MaxLevel"))
                _maxLevel = PlayerPrefs.GetInt("MaxLevel", 0);
            ShowOpenLevels();
        }

        private void LoadLevelData(TextAsset file)
        {
            if(file == null)
            {
                Debug.LogError("LoadLevelData: Указанный JSON-файл пуст.");
                return;
            }

            _currentLevelData = JsonUtility.FromJson<LevelData>(file.text);
        }

        private void ShowOpenLevels()
        {
            for(int i = 0; i < _levelButtons.Length; i++)
            {
                _levelButtons[i].interactable = i <= _maxLevel;
            }
        }

        public void UpLevel()
        {
            _currentLevel++;
            if(_currentLevel >= _maxLevel)
            {
                _maxLevel = Mathf.Clamp(_maxLevel + 1, 0, 2);
                PlayerPrefs.SetInt("MaxLevel", _maxLevel);
                ShowOpenLevels();
            }

        }

        public void StartLevel()
        {
            _tileMapGenerator.GenerateTiles(_currentLevelData);
            _brickUI.gameObject.SetActive(false);
            _levelTimer.StartTimer();
            GameManager.Instance.StartGame();
        }

        public void RestartLevel()
        {
            _currentLevel--;
            _levelsPanel.SetActive(true);
            _resultLevelUI.gameObject.SetActive(false);
            _brickUI.Init(_currentLevelData);
            GameManager.Instance.FormController.RestoreAllForms();
        }

        public void StartNextLevel()
        {
            _levelsPanel.SetActive(true);
            _resultLevelUI.gameObject.SetActive(false);
            ShowBrifing(_currentLevel);
            GameManager.Instance.FormController.RestoreAllForms();
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            _maxLevel = 0;
            ShowOpenLevels();
        }

        public void ShowBrifing(int levelNum)
        {
            _currentLevel = levelNum;
            LoadLevelData(_levelDatas[levelNum]);
            _brickUI.Init(_currentLevelData);
        }

        public void LevelComplete()
        {
            int time = _levelTimer.GetTime();
            bool newBestTime = false;
            var prevRecord = PlayerPrefs.GetInt($"Level{_currentLevel}", int.MaxValue);

            if(prevRecord > time)
            {
                PlayerPrefs.SetInt($"Level{_currentLevel}", time);
                newBestTime = true;
            }

            var stars = time <= _currentLevelData.timeTreeStars ? 3 : time <= _currentLevelData.timeTwoStars ? 2 : 1;

            Result result = new()
            {
                winStatus = true,
                level = _currentLevel,
                time = time, // время в секундах
                stars = stars,
                newBestTime = newBestTime,
            };

            _resultLevelUI.ShowResult(result);
            UpLevel();
        }

    }

    public struct Result
    {
        public bool winStatus;
        public int level;
        public int time;
        public int stars;
        public bool newBestTime;
    }
}