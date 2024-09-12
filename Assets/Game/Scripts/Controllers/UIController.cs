using System;
using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }

        [SerializeField] private Canvas _gameCanvas;

        [SerializeField] private GameObject _gameUIPanel;
        [SerializeField] private GameObject _levelsPanel;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _hotTopPanel;

        [SerializeField] private GameObject _dialogPanel;

        [SerializeField] private Image[] _formIconsBack;
        [SerializeField] private Color _selectedColor = Color.cyan;
        [SerializeField] private Text[] _formStrenghts;
        [SerializeField] private Text _moneyGameText;
        [SerializeField] private Text _moneyShopText;
        [SerializeField] private Text _moneyForgeText;

        [SerializeField] private PlayerController _playerController;

        [SerializeField] private GameObject _teleportButtonGO;
        [SerializeField] private GameObject _cancelButtonGO;

        private FormController _formController;
        private Image _currentIcon;
        private string _cachedMoneyText;  // Для кэширования значения денег

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            _formController ??= GameManager.Instance.FormController;
            _playerController ??= FindAnyObjectByType<PlayerController>();
            SubscribeEvents();
        }

        private void Start()
        {
            SetCanvasVisibility(true);
            UpdateMoneyUI();
        }

        public void StartLevel()
        {
            _gameUIPanel.SetActive(true);
            _levelsPanel.SetActive(false);
            _mainMenuPanel.SetActive(false);
            _hotTopPanel.SetActive(true);
            _gameCanvas.gameObject.SetActive(true);
        }

        private void SubscribeEvents()
        {
            GameEventManager.Instance.Subscribe(GameEvent.OnChangeForm, UpdateFormUI);
            GameEventManager.Instance.Subscribe(GameEvent.OnChangeMoney, UpdateMoneyUI);
        }

        private void UnsubscribeEvents()
        {
            GameEventManager.Instance.Unsubscribe(GameEvent.OnChangeForm, UpdateFormUI);
            GameEventManager.Instance.Unsubscribe(GameEvent.OnChangeMoney, UpdateMoneyUI);
        }

        public void SetCanvasVisibility(bool showMainMenu)
        {
            SetGameObjectActive(_gameCanvas.gameObject, !showMainMenu);
            SetGameObjectActive(_mainMenuPanel, showMainMenu);
            SetGameObjectActive(_levelsPanel, !showMainMenu);
        }

        public void ShowOptionsUI(bool isShow)
        {
            SetGameObjectActive(_optionsPanel, isShow);
            bool shouldShowUI = !isShow && GameManager.Instance.IsPlaying;
            SetGameObjectActive(_gameUIPanel, shouldShowUI);
            SetGameObjectActive(_hotTopPanel, shouldShowUI);
            SetGameObjectActive(_playerController.gameObject, shouldShowUI);
        }


        public void ShowDialog()
        {
            _gameCanvas.gameObject.SetActive(false);
            _mainMenuPanel.SetActive(false);
            _dialogPanel.SetActive(true);
            _levelsPanel.SetActive(false);
        }

        public void UpdateFormUI()
        {
            _formController ??= GameManager.Instance.FormController;

            int formIndex = _formController.CurrentForm.Index;
            if(!IsValidFormIndex(formIndex))
            {
                Debug.LogError($"Form index out of range: {formIndex}. Valid range: 0 to {_formIconsBack.Length - 1}");
                return;
            }

            if(_currentIcon != null)
            {
                _currentIcon.color = Color.white;
            }

            _currentIcon = _formIconsBack[formIndex];
            _currentIcon.color = _selectedColor;
        }

        private bool IsValidFormIndex(int index)
        {
            return index >= 0 && index < _formIconsBack.Length;
        }

        private void UpdateMoneyUI()
        {
            string moneyText = _playerController.MoneyAmount.ToString();
            if(moneyText != _cachedMoneyText)
            {
                _cachedMoneyText = moneyText;
                SetMoneyText(_moneyGameText, moneyText);
                SetMoneyText(_moneyShopText, moneyText);
                SetMoneyText(_moneyForgeText, moneyText);
            }
        }

        private void SetMoneyText(Text textComponent, string text)
        {
            if(textComponent != null)
            {
                textComponent.text = text;
            }
        }

        public void SetStrengthValue(int index, int val)
        {
            if(!IsValidStrengthIndex(index))
            {
                Debug.LogError($"Invalid index {index} for setting strength value.");
                return;
            }

            _formStrenghts[index].text = $"{val}/{_formController.AllForms[index].Durability}";
        }

        private bool IsValidStrengthIndex(int index)
        {
            return index >= 0 && index < _formStrenghts.Length &&
                   _formController != null && _formController.AllForms != null &&
                   index < _formController.AllForms.Length;
        }

        private void OnDestroy()
        {
            if(Instance == this)
            {
                UnsubscribeEvents();
            }
        }

        public void SetButtonVisibility(GameObject button, bool isVisible)
        {
            SetGameObjectActive(button, isVisible);
        }

        public void ShowCancelButton(bool isShow)
        {
            SetButtonVisibility(_teleportButtonGO, !isShow);
            SetButtonVisibility(_cancelButtonGO, isShow);
        }

        private void SetGameObjectActive(GameObject obj, bool isActive)
        {
            if(obj != null && obj.activeSelf != isActive)
            {
                obj.SetActive(isActive);
            }
        }

        internal void ShowLevelsMenu()
        {
            _levelsPanel.SetActive(true);
            _gameCanvas.gameObject.SetActive(false);
        }
    }
}
