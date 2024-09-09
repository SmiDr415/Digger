using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }

        [SerializeField] private Canvas _mainMenuCanvas;
        [SerializeField] private Canvas _gameCanvas;

        [SerializeField] private GameObject _gameUIPanel;
        [SerializeField] private GameObject _levelsPanel;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private GameObject _hotTopPanel;

        [SerializeField] private Image[] _formIconsBack;
        [SerializeField] private Color _selectedColor = Color.cyan;
        [SerializeField] private Text[] _formStrenghts;
        [SerializeField] private Text _money;
        [SerializeField] private Text _shopMoney;

        [SerializeField] private PlayerController _playerController;

        [SerializeField] private GameObject _teleportButtonGO;
        [SerializeField] private GameObject _cancelButtonGO;

        private FormController _formController;

        private Image _currentIcon;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                GameEventManager.Instance.Subscribe(GameEvent.OnChangeForm, UpdateFormUI);
                GameEventManager.Instance.Subscribe(GameEvent.OnChangeMoney, UpdateMoneyUI);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

        }


        private void Start()
        {
            UpdateMoneyUI();

            _formController = GameManager.Instance.FormController;
            ShowMainMenu(true);
        }


        public void ShowMainMenu(bool isShow)
        {
            _mainMenuCanvas.gameObject.SetActive(isShow);
            _gameCanvas.gameObject.SetActive(!isShow);
            _gameUIPanel.SetActive(isShow);
            _levelsPanel.SetActive(!isShow);

        }

        public void ShowGameUI(bool isShow)
        {
            _mainMenuCanvas.gameObject.SetActive(!isShow);
            _gameCanvas.gameObject.SetActive(isShow);
            _gameUIPanel.SetActive(isShow);
            _hotTopPanel.SetActive(isShow);

        }


        public void ShowOptionsUI(bool isShow)
        {
            _optionsPanel.SetActive(isShow);
            if(GameManager.Instance.IsPlaying)
            {
                _gameUIPanel.SetActive(!isShow);
                _hotTopPanel.SetActive(!isShow);
                _playerController.gameObject.SetActive(!isShow);
            }
            else
            {
                _gameCanvas.gameObject.SetActive(isShow);
                _gameUIPanel.SetActive(!isShow);
                _hotTopPanel.SetActive(!isShow);
            }
        }

        public void UpdateFormUI()
        {
            if(_formController == null)
                _formController = GameManager.Instance.FormController;

            var formIndex = _formController.CurrentForm.Index;
            if(_currentIcon != null)
                _currentIcon.color = Color.white;
            _currentIcon = _formIconsBack[formIndex];
            _currentIcon.color = _selectedColor;
        }


        private void UpdateMoneyUI()
        {
            _money.text = _playerController.MoneyAmount.ToString();
            _shopMoney.text = _playerController.MoneyAmount.ToString();
        }

        public void SetStrengthValue(int index, int val)
        {
            _formStrenghts[index].text = $"{val}/{_formController.AllForms[index].Durability}";
        }

        private void OnDestroy()
        {
            if(Instance == this)
            {
                GameEventManager.Instance.Unsubscribe(GameEvent.OnChangeForm, UpdateFormUI);
                GameEventManager.Instance.Unsubscribe(GameEvent.OnChangeMoney, UpdateMoneyUI);
            }
        }

        public void ShowCancelButton(bool isShow)
        {
            _teleportButtonGO.SetActive(!isShow);
            _cancelButtonGO.SetActive(isShow);
        }


    }
}
