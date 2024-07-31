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

        private Image _currentIcon;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                GameEventManager.Instance.Subscribe(GameEvent.OnChangeForm, UpdateFormUI);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

        }

        public void UpdateFormUI()
        {
            var formIndex = GameManager.Instance.FormController.CurrentForm.Index;
            if(_currentIcon != null)
                _currentIcon.color = Color.white;
            _currentIcon = _formIconsBack[formIndex];
            _currentIcon.color = _selectedColor;
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
            }
        }
    }
}
