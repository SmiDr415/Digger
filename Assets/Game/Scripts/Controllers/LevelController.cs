using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Button[] _levelButtons;
        [SerializeField] private TileMapGenerator _tileMapGenerator;
        private int _currentLevel;
        private int _maxLevel;


        private void Start()
        {
            if(PlayerPrefs.HasKey("MaxLevel"))
                _maxLevel = PlayerPrefs.GetInt("MaxLevel", 0);
            ShowOpenLevels();
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
            _maxLevel = Mathf.Clamp(_maxLevel + 1, 0, 2);
            PlayerPrefs.SetInt("MaxLevel", _maxLevel);
            ShowOpenLevels();
        }

        public void StartLevel(int level)
        {
            _currentLevel = level;
            _tileMapGenerator.SetLevel(level);
            GameManager.Instance.StartGame();
        }

        public void RestartLevel()
        {
            _tileMapGenerator.SetLevel(_currentLevel);
            GameManager.Instance.StartGame();
        }

        public void StartNextLevel()
        {
            _tileMapGenerator.SetLevel(++_currentLevel);
            GameManager.Instance.StartGame();
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

    }
}