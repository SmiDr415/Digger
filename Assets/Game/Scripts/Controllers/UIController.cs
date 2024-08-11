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

        [SerializeField] private GameObject _teleportButtonGO;
        [SerializeField] private GameObject _cancelButtonGO;

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
            _money.text = PlayerController.Instance.MoneyAmount.ToString();
        }

        public void UpdateFormUI()
        {
            var formIndex = GameManager.Instance.FormController.CurrentForm.Index;
            if(_currentIcon != null)
                _currentIcon.color = Color.white;
            _currentIcon = _formIconsBack[formIndex];
            _currentIcon.color = _selectedColor;
        }


        private void UpdateMoneyUI()
        {
            _money.text = PlayerController.Instance.MoneyAmount.ToString();
        }

        public void SetStrengthValue(int index, int val)
        {
            _formStrenghts[index].text = $"{val}%";
        }

        private void OnDestroy()
        {
            if(Instance == this)
            {
                GameEventManager.Instance.Unsubscribe(GameEvent.OnChangeForm, UpdateFormUI);
                GameEventManager.Instance.Unsubscribe(GameEvent.OnChangeMoney, UpdateMoneyUI);
            }
        }

        public void ShowCancelButton()
        {
            _teleportButtonGO.SetActive(false);
            _cancelButtonGO.SetActive(true);
        }
    }
}
