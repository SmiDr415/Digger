using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }

        [SerializeField] private Image[] _formIconsBack;
        [SerializeField] private Color _selectedColor = Color.cyan;
        [SerializeField] private Text[] _formStrenghts;
        [SerializeField] private Text _money;

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
            _money.text = _playerController.MoneyAmount.ToString();
            _formController = GameManager.Instance.FormController;
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
